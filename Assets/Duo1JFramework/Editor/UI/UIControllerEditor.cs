using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Duo1JFramework.UI
{
    [CustomEditor(typeof(UIController), true)]
    public class UIControllerEditor : BaseCustomEditor<UIController>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Draw()
        {
            if (GUILayout.Button("收集节点"))
            {
                instance.CollectNode();
                EditorUtility.SetDirty(instance);
            }
            DrawNodeList();
        }

        private void DrawNodeList()
        {
            List<Transform> nodeList = instance.GetNodeList();
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
