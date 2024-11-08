using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 直线样条线Editor
    /// </summary>
    [CustomEditor(typeof(LineSpline))]
    public class LineSplineEditor : BaseCustomEditor<LineSpline>
    {
        private bool showInfo = true;

        protected override bool ShowOriginDefault => true;

        private Vector3 labelOffset = new Vector3(0, -0.1f, 0);

        private SerializedProperty nodeListProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            nodeListProp = serializedObject.FindProperty("nodeList");
        }

        private void OnSceneGUI()
        {
            if (instance == null || instance.NodeList == null)
            {
                return;
            }

            RichText = true;

            ED.HandlesMatrix(instance.transform.localToWorldMatrix, () =>
            {
                Vector3[] nodeList = instance.NodeList;

                for (int i = 0; i < nodeList.Length; i++)
                {
                    nodeList[i] = Handles.PositionHandle(nodeList[i], Quaternion.identity);


                    if (showInfo)
                    {
                        Vector3 pos = nodeList[i];
                        Handles.Label(nodeList[i] + labelOffset, $"<color=#00FF00><size=16>P{i}</size></color> <color=#ABABAB>({pos.x:F1}, {pos.y:F1}, {pos.z:F1})</color>");
                    }
                }

                Handles.DrawPolyLine(nodeList);
            });
        }

        protected override void DrawInspector()
        {
            showInfo = EditorGUILayout.Toggle("显示信息", showInfo);

            EditorGUILayout.PropertyField(nodeListProp, new GUIContent("节点列表"));
        }
    }
}
