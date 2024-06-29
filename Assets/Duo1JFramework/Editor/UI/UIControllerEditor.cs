using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Duo1JFramework.UI
{
    [CustomEditor(typeof(UIController), true)]
    public class UIControllerEditor : Editor
    {
        private UIController uiController;

        private void OnEnable()
        {
            uiController = target as UIController;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("收集节点"))
            {
                uiController.CollectNode();
                EditorUtility.SetDirty(uiController);
            }
            DrawNodeList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNodeList()
        {
            List<Transform> nodeList = uiController.GetNodeList();
            if (nodeList != null)
            {
                ED.Vertical(() =>
                {
                    ED.DisableGroup_Editor(() =>
                    {
                        for (int i = 0, len = nodeList.Count; i < len; i++)
                        {
                            ED.Horizontal(() =>
                            {
                                EditorGUILayout.LabelField($"({i})", GUILayout.Width(32));
                                EditorGUILayout.ObjectField(nodeList[i], typeof(Transform), false);
                            });
                        }
                    });
                }, "box");
            }
        }
    }
}
