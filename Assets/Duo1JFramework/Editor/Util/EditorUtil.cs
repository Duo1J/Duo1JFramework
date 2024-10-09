using System;
using UnityEditor;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具类
    /// </summary>
    public static class EditorUtil
    {
        #region 选中

        /// <summary>
        /// 获取选中的Go
        /// </summary>
        public static bool GetActiveGo(out GameObject go, bool nullWarn = true)
        {
            go = Selection.activeGameObject;
            if (go == null)
            {
                if (nullWarn) Log.Error("未选中任何GameObject");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取选中的Obj
        /// </summary>
        public static bool GetActiveObj(out UnityEngine.Object obj, bool nullWarn = true)
        {
            obj = Selection.activeObject;
            if (obj == null)
            {
                if (nullWarn) Log.Error("未选中任何Object");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置选中Go为父节点
        /// </summary>
        public static void SetParentToActiveGo(GameObject go, bool worldPositionStays = false)
        {
            if (GetActiveGo(out GameObject parGo, false))
            {
                go.transform.SetParent(parGo.transform, worldPositionStays);
            }
        }

        /// <summary>
        /// 设置为选中物体
        /// </summary>
        public static void SetActiveGo(GameObject go)
        {
            Selection.activeObject = go;
        }

        #endregion 选中

        #region 资源编辑

        /// <summary>
        /// 编辑器资源编辑
        /// </summary>
        public static void AssetEditing(Action callback, string msg = null)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.EditorInfo("编辑器开始资源编辑");
                }
                else
                {
                    Log.EditorInfo($"编辑器开始资源编辑: {msg}");
                }

                AssetDatabase.StartAssetEditing();
                callback?.Invoke();
            }
            finally
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.EditorInfo("编辑器结束资源编辑");
                }
                else
                {
                    Log.EditorInfo($"编辑器结束资源编辑: {msg}");
                }

                AssetDatabase.StopAssetEditing();
            }
        }

        /// <summary>
        /// 编辑器保存并刷新
        /// </summary>
        public static void SaveAndRefresh(string msg = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Log.EditorInfo("编辑器保存并刷新");
            }
            else
            {
                Log.EditorInfo($"编辑器保存并刷新: {msg}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 检测是否需要切换平台并询问是否继续
        /// </summary>
        /// <returns>是否继续</returns>
        public static bool CheckPlatformChgAndAsk(BuildTarget buildTarget)
        {
            if (EditorUserBuildSettings.activeBuildTarget == buildTarget)
            {
                return true;
            }

            return EditorUtility.DisplayDialog("提示",
                $"平台将从{EditorUserBuildSettings.activeBuildTarget.GetName()}切换为{buildTarget.GetName()}, 是否继续?",
                "是", "否");
        }

        /// <summary>
        /// 创建临时实例
        /// </summary>
        public static void CreateTempInstance(GameObject template, Action<GameObject> action)
        {
            if (template == null || action == null)
            {
                return;
            }

            GameObject instance = null;
            try
            {
                instance = UObject.Instantiate(template);
                action(instance);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
            finally
            {
                if (instance != null)
                {
                    instance.DestroyImmediate(false);
                }
            }
        }

        #endregion 资源编辑

        #region 动画

        public static void AnimMode(Action action)
        {
            try
            {
                Log.EditorInfo("StartAnimationMode");
                AnimationMode.StartAnimationMode();
                action?.Invoke();
            }
            finally
            {
                Log.EditorInfo("StopAnimationMode");
                AnimationMode.StopAnimationMode();
            }
        }

        public static void AnimSampling(Action action)
        {
            try
            {
                AnimationMode.BeginSampling();
                action?.Invoke();
            }
            finally
            {
                AnimationMode.EndSampling();
            }
        }

        #endregion 动画

        #region 杂项

        /// <summary>
        /// 进度条
        /// </summary>
        public static void ProgressBar(Action action, string title, string content, float fakeProgress = 0.3f)
        {
            try
            {
                EditorUtility.DisplayProgressBar(title, content, fakeProgress);
                action?.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 通过回调修改进度的进度条
        /// </summary>
        public static void ProgressBar(Action<Action<float>> action, string title, string content)
        {
            try
            {
                EditorUtility.DisplayProgressBar(title, content, 0);
                action?.Invoke((progress) => EditorUtility.DisplayProgressBar(title, content, progress));
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        public static void CopyText(string text)
        {
            TextEditor editor = new TextEditor();
            editor.text = text;
            editor.SelectAll();
            editor.Copy();
            Log.EditorInfo($"已拷贝`{text}`到粘贴板");
        }

        #endregion 杂项

        #region 窗口

        /// <summary>
        /// 打开编辑器窗口
        /// </summary>
        /// <param name="wndName">窗口名，不填则使用配置的名称</param>
        public static T OpenEditorWnd<T>(string _wndName = null) where T : EditorWindow
        {
            string wndName = string.IsNullOrEmpty(_wndName) ? EditorDef.Menu.GetEditorWndName(typeof(T)) : _wndName;

            T wnd = EditorWindow.GetWindow<T>();
            if (!string.IsNullOrEmpty(wndName))
            {
                wnd.titleContent = new GUIContent(wndName);
            }
            wnd.Show();
            return wnd;
        }

        /// <summary>
        /// 聚焦控件
        /// </summary>
        public static void FocusControl(string name)
        {
            GUI.FocusControl(name);
        }

        /// <summary>
        /// 失焦控件
        /// </summary>
        public static void LostFocusControl()
        {
            FocusControl(null);
        }

        /// <summary>
        /// 聚焦窗口
        /// </summary>
        public static void FocusWindow(int windowID)
        {
            GUI.FocusWindow(windowID);
        }

        #endregion

        #region ScriptableObject

        /// <summary>
        /// 获取或创建ScriptableObject
        /// </summary>
        public static T GetOrCreateSO<T>(string path) where T : ScriptableObject
        {
            T so = AssetDatabase.LoadAssetAtPath<T>(path);
            if (so == null)
            {
                so = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(so, path);
            }
            return so;
        }

        /// <summary>
        /// 获取或创建编辑器配置ScriptableObject
        /// </summary>
        public static T GetOrCreateEditorCfgSO<T>() where T : ScriptableObject
        {
            string path = GetEditorCfgSOPath<T>();
            return GetOrCreateSO<T>(path);
        }

        /// <summary>
        /// 获取编辑器配置ScriptableObject默认路径
        /// </summary>
        public static string GetEditorCfgSOPath<T>()
        {
            return $"{EditorDef.EDITOR_CONFIG_PATH}/{typeof(T).Name}.asset";
        }

        #endregion ScriptableObject
    }
}
