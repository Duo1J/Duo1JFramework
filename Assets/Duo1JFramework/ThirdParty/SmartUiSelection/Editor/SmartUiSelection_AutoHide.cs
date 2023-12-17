#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace kamgam.editor.smartuiselection
{
    public class ManagedScreenSpaceOverlayCanvas
    {
        public Canvas Canvas;
        public bool CanvasActiveDueToUser;
        public bool CanvasActiveDueToLogic;

        public ManagedScreenSpaceOverlayCanvas(Canvas canvas)
        {
            this.Canvas = canvas;
            this.CanvasActiveDueToUser = canvas.gameObject.activeSelf;
            this.CanvasActiveDueToLogic = canvas.gameObject.activeSelf;
        }
    }

    [InitializeOnLoad]
    [Serializable]
    public static class SmartUiSelection_AutoHide
    {
        [SerializeField]
        public static List<ManagedScreenSpaceOverlayCanvas> ManagedScreenSpaceOverlayCanvases;

        // To keep backwards compatibility (remove in 4.x release)
        public static List<ManagedScreenSpaceOverlayCanvas> HiddenScreenSpaceOverlayCanvases
        {
            get { return ManagedScreenSpaceOverlayCanvases; }
        }

        [SerializeField]
        static double _lastAutoHideCheckTime;

        static float _autoHideCheckIntervaInSec = 0.100f; // update every 100 ms

        static float _camToXYPlaneDistance;

        static Camera _lastKnownCamera;

        static SmartUiSelection_AutoHide()
        {
            if (SmartUiSelection_Settings.instance == null)
            {
                Debug.LogWarning("SmartUiSelection plugin did not find any settings and will do nothing.\nPlease create them in a 'Resources' folder via Assets -> Create -> SmartUiSelection Settings.");
            }

            EditorApplication.update += AutoHideScreenSpaceOverlayCanvases;
        }

        private static void AutoHideScreenSpaceOverlayCanvases()
        {
            AutoHideScreenSpaceOverlayCanvases(false);
        }

        private static Camera getSceneViewCamera()
        {
#if UNITY_2019_1_OR_NEWER
            var cam = SceneView.lastActiveSceneView.camera;
#else
            var cam = Camera.current;
#endif
            // Remember cam (if you Ping an object in the editor Camera.current becomes null, also if you change to play mode)
            if (cam != null)
            {
                _lastKnownCamera = cam;
            }

            return _lastKnownCamera;
        }

        private static void AutoHideScreenSpaceOverlayCanvases(bool forceExecution)
        {
            try
            {
                if (SmartUiSelection_Settings.enablePlugin && SmartUiSelection_Settings.enableAutoHide)
                {
#if !UNITY_2019_2_OR_NEWER
                    if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying == false)
                    {
                        forceExecution = true;
                    }
#endif
                    float autoHideInterval = (SmartUiSelection_Settings.autoHideAlways) ? _autoHideCheckIntervaInSec * 2 : _autoHideCheckIntervaInSec;
                    if (forceExecution || (EditorApplication.timeSinceStartup - _lastAutoHideCheckTime > autoHideInterval))
                    {
                        _lastAutoHideCheckTime = EditorApplication.timeSinceStartup;

                        if (ManagedScreenSpaceOverlayCanvases == null)
                        {
                            ManagedScreenSpaceOverlayCanvases = new List<ManagedScreenSpaceOverlayCanvas>();
                        }
                        UpdateManagedCanvasesList();

                        if (ManagedScreenSpaceOverlayCanvases.Count > 0)
                        {

                            bool shouldBeActiveDueToLogic = true;

                            // activate based on distance
                            if (getSceneViewCamera() != null)
                            {
                                // calculate the distance to the XY plane
                                var zeroPlane = new Plane(Vector3.forward, Vector3.zero); // screenspace canvases are in zero plane
                                _camToXYPlaneDistance = Mathf.Abs(zeroPlane.GetDistanceToPoint(_lastKnownCamera.transform.position));

                                // show/hide based on the distance
                                shouldBeActiveDueToLogic = _camToXYPlaneDistance > SmartUiSelection_Settings.autoHideDistanceThreshold;
                            }

                            // Is Editor refreshing?
                            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                            {
                                shouldBeActiveDueToLogic = true;
                            }

                            // Is a build in progress (buggy, EditorApplication.update is not called while building) ..
                            if (BuildPipeline.isBuildingPlayer)
                            {
                                shouldBeActiveDueToLogic = true;
                            }
                            // .. not a 100% solution but at least it works if the user uses the mouse to click the "build" button.
                            if (EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.ToString().Contains("BuildPlayerWindow"))
                            {
                                shouldBeActiveDueToLogic = true;
                            }

                            // Is mouse out of scene view and autoHideAlways is not enabled
                            bool isMouseInSceneView = (EditorWindow.mouseOverWindow != null && SceneView.sceneViews.Contains(EditorWindow.mouseOverWindow));
                            if (isMouseInSceneView == false && SmartUiSelection_Settings.autoHideAlways == false)
                            {
                                shouldBeActiveDueToLogic = true;
                            }

                            // Is Editor playing and autoHideDuringPlayback is set to false.
                            if (EditorApplication.isPlayingOrWillChangePlaymode && SmartUiSelection_Settings.autoHideDuringPlayback == false)
                            {
                                shouldBeActiveDueToLogic = true;
                            }

#if !UNITY_2019_2_OR_NEWER
                            // is about to switch to playmode (triggers a deserialization, thus we reset here)
                            if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying == false)
                            {
                                shouldBeActiveDueToLogic = true;
                            }
#endif

                            UpdateCanvases(shouldBeActiveDueToLogic);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("SmartUiSelection auto-hide caused an unexpected Error:\n" + e.Message + "\nStacktrace:\n" + e.StackTrace);
            }
        }

        public static void ShowAllCanvases()
        {
            foreach (var hiddenCanvas in ManagedScreenSpaceOverlayCanvases)
            {
                if (hiddenCanvas.Canvas != null)
                {
                    hiddenCanvas.CanvasActiveDueToLogic = true;
                    setCanvasActive(hiddenCanvas, true);
                }
            }
        }

        private static void UpdateManagedCanvasesList()
        {
            // add new screen space canvases to the list
            var overlayCanvases = GameObject.FindObjectsOfType<Canvas>().Where(canvas => canvas.renderMode == RenderMode.ScreenSpaceOverlay);
            if (overlayCanvases.Count() > 0)
            {
                foreach (var canvas in overlayCanvases)
                {
                    if (ManagedScreenSpaceOverlayCanvases.FirstOrDefault(c => c.Canvas == canvas) == null)
                    {
                        // add
                        ManagedScreenSpaceOverlayCanvases.Add(new ManagedScreenSpaceOverlayCanvas(canvas));
                    }
                }
            }

            // clean up in case a canvas got destroyed (i.e. scene change) or if it is no longer a ScreenSpaceOverlay canvas.
            for (int i = ManagedScreenSpaceOverlayCanvases.Count - 1; i >= 0; --i)
            {
                if (ManagedScreenSpaceOverlayCanvases[i].Canvas == null)
                {
                    ManagedScreenSpaceOverlayCanvases.RemoveAt(i);
                }
                else if (ManagedScreenSpaceOverlayCanvases[i].Canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    // override logic to true
                    ManagedScreenSpaceOverlayCanvases[i].CanvasActiveDueToLogic = true;
                    // update
                    updateCanvasVisibility(ManagedScreenSpaceOverlayCanvases[i], ManagedScreenSpaceOverlayCanvases[i].CanvasActiveDueToUser);
                    // remove
                    ManagedScreenSpaceOverlayCanvases.RemoveAt(i);
                }
            }
        }

        private static void UpdateCanvases(bool shouldBeActiveDueToLogic)
        {
            if (ManagedScreenSpaceOverlayCanvases.Count > 0)
            {
                foreach (var canvas in ManagedScreenSpaceOverlayCanvases)
                {
                    updateCanvasVisibility(canvas, shouldBeActiveDueToLogic);
                }
            }
        }

        private static void updateCanvasVisibility(ManagedScreenSpaceOverlayCanvas canvas, bool shouldBeActiveDueToLogic)
        {
            // Update the CanvasActiveDueToUser flag if the user changed the active state, meaning
            // if the canvas' current active state differs from CanvasActiveDueToLogic.
            bool isCurrentlyActive = isCanvasActive(canvas);
            if (canvas.CanvasActiveDueToLogic != isCurrentlyActive)
            {
                canvas.CanvasActiveDueToUser = isCurrentlyActive;
            }

            // user and logic both have to be true in order for the canvas to be visible
            bool activateCanvas = (canvas.CanvasActiveDueToUser == true && shouldBeActiveDueToLogic == true);
#if UNITY_2019_2_OR_NEWER
            // Ignore the whole user active stuff in unity versions which use SceneVis show/hide
            setCanvasActive(canvas, shouldBeActiveDueToLogic);
#else
            setCanvasActive(canvas, activateCanvas);
#endif
            canvas.CanvasActiveDueToLogic = activateCanvas;
        }

        private static bool isCanvasActive(ManagedScreenSpaceOverlayCanvas canvas)
        {
#if UNITY_2019_2_OR_NEWER
            return SceneVisibilityManager.instance.IsHidden(canvas.Canvas.gameObject) == false;
#else
            return canvas.Canvas.gameObject.activeSelf;
#endif
        }

        private static void setCanvasActive(ManagedScreenSpaceOverlayCanvas canvas, bool active)
        {
#if UNITY_2019_2_OR_NEWER
            if (active)
            {
                SceneVisibilityManager.instance.Show(canvas.Canvas.gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(canvas.Canvas.gameObject, true);
            }
#else
            canvas.Canvas.gameObject.SetActive(active);
#endif
        }

        public static float GetCamToXYPlaneDistance()
        {
            return _camToXYPlaneDistance;
        }
    }
}
#endif
