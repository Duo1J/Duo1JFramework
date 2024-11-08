using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 贝塞尔样条线Editor
    /// </summary>
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineEditor : BaseCustomEditor<BezierSpline>
    {
        private Vector3 labelOffset = new Vector3(0, -0.1f, 0);

        private SerializedProperty nodeListProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            nodeListProp = serializedObject.FindProperty("nodeList");
        }

        private void OnSceneGUI()
        {
            if (instance == null || instance.NodeList == null || instance.NodeList.Length < 2)
            {
                return;
            }

            RichText = true;

            ED.HandlesMatrix(instance.transform.localToWorldMatrix, () =>
            {
                BezierSplineNode[] nodeList = instance.NodeList;

                for (int i = 0; i < nodeList.Length; i++)
                {
                    BezierSplineNode node = nodeList[i];

                    node.point = Handles.PositionHandle(node.point, Quaternion.identity);
                    Handles.DrawDottedLine(node.point, node.control, 5);
                    node.control = Handles.Slider2D(
                        node.control,
                        Vector3.up,
                        Vector3.forward,
                        Vector3.right,
                        0.2f,
                        Handles.SphereHandleCap,
                        Vector2.zero);

                    Handles.Label(node.point + labelOffset, $"<color=#00FF00>P{i} ({node.point.x:F1}, {node.point.y:F1}, {node.point.z:F1})</color>");
                    Handles.Label(node.control + labelOffset, $"<color=#00FF00>C{i} ({node.control.x:F1}, {node.control.y:F1}, {node.control.z:F1})</color>");
                }

                for (int i = 0; i < nodeList.Length - 1; i++)
                {
                    Handles.DrawBezier(
                        nodeList[i].point,
                        nodeList[i + 1].point,
                        nodeList[i].control,
                        nodeList[i + 1].control,
                        Color.white,
                        null,
                        2f);
                }
            });
        }

        protected override void Draw()
        {
            EditorGUILayout.PropertyField(nodeListProp, new GUIContent("节点列表"));

            if (GUILayout.Button("重置零点的控制点"))
            {
                if (instance == null || instance.NodeList == null)
                {
                    return;
                }

                BezierSplineNode[] nodeList = instance.NodeList;

                for (int i = 0; i < nodeList.Length; i++)
                {
                    if (nodeList[i].control == Vector3.zero)
                    {
                        nodeList[i].control = nodeList[i].point;
                    }
                }
            }

            if (GUILayout.Button("重置所有控制点"))
            {
                if (instance == null || instance.NodeList == null)
                {
                    return;
                }

                BezierSplineNode[] nodeList = instance.NodeList;

                for (int i = 0; i < nodeList.Length; i++)
                {
                    nodeList[i].control = nodeList[i].point;
                }
            }
        }
    }
}