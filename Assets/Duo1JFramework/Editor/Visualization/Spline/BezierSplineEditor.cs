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
        private bool showInfo = true;

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

                for (int i = 0, len = nodeList.Length - 1; i <= len; i++)
                {
                    BezierSplineNode node = nodeList[i];

                    node.point = Handles.PositionHandle(node.point, Quaternion.identity);

                    if (i != 0)
                    {
                        node.controlLeft = Handles.Slider2D(
                            node.controlLeft,
                            Vector3.up,
                            Vector3.forward,
                            Vector3.right,
                            0.2f,
                            Handles.SphereHandleCap,
                            Vector2.zero);

                        Handles.DrawDottedLine(node.point, node.controlLeft, 5);

                        if (showInfo)
                        {
                            Handles.Label(node.controlLeft + labelOffset, $"<color=#00FF00><size=16>CL{i}</size></color> <color=#ABABAB>({node.controlLeft.x:F1}, {node.controlLeft.y:F1}, {node.controlLeft.z:F1})</color>");
                        }
                    }

                    if (i != len)
                    {
                        node.controlRight = Handles.Slider2D(
                             node.controlRight,
                             Vector3.up,
                             Vector3.forward,
                             Vector3.right,
                             0.2f,
                             Handles.SphereHandleCap,
                             Vector2.zero);

                        Handles.DrawDottedLine(node.point, node.controlRight, 5);

                        if (showInfo)
                        {
                            Handles.Label(node.controlRight + labelOffset, $"<color=#00FF00><size=16>CR{i}</size></color> <color=#ABABAB>({node.controlRight.x:F1}, {node.controlRight.y:F1}, {node.controlRight.z:F1})</color>");
                        }
                    }

                    if (showInfo)
                    {
                        Handles.Label(node.point + labelOffset, $"<color=#00FF00><size=16>P{i}</size></color> <color=#ABABAB>({node.point.x:F1}, {node.point.y:F1}, {node.point.z:F1})</color>");
                    }
                }

                for (int i = 0; i < nodeList.Length - 1; i++)
                {
                    Handles.DrawBezier(
                        nodeList[i].point,
                        nodeList[i + 1].point,
                        nodeList[i].controlRight,
                        nodeList[i + 1].controlLeft,
                        Color.white,
                        null,
                        2f);
                }
            });
        }

        protected override void DrawInspector()
        {
            showInfo = EditorGUILayout.Toggle("显示信息", showInfo);

            EditorGUILayout.PropertyField(nodeListProp, new GUIContent("节点列表"));

            if (GUILayout.Button("重置零点的控制点"))
            {
                ResetAllControl(true);
            }

            if (GUILayout.Button("重置所有控制点"))
            {
                ResetAllControl(false);
            }
        }

        /// <summary>
        /// 重置控制点
        /// </summary>
        /// <param name="justZero">是否只重置零点控制点</param>
        protected void ResetAllControl(bool justZero = false)
        {
            if (instance == null || instance.NodeList == null || instance.NodeList.Length == 0)
            {
                return;
            }

            BezierSplineNode[] nodeList = instance.NodeList;

            if (nodeList.Length == 1)
            {
                nodeList[0].controlLeft = Vector3.zero;
                nodeList[0].controlRight = Vector3.zero;
                return;
            }

            for (int i = 0, len = nodeList.Length - 1; i <= len; i++)
            {
                BezierSplineNode node = nodeList[i];

                if (!justZero || node.controlLeft != Vector3.zero)
                {
                    if (i == 0)
                    {
                        nodeList[i].controlLeft = nodeList[i].point;
                    }
                    else if (i == len)
                    {
                        nodeList[i].controlLeft = nodeList[i].point + (nodeList[i - 1].point - nodeList[i].point).normalized;
                    }
                    else
                    {
                        nodeList[i].controlLeft = nodeList[i].point + (nodeList[i - 1].point - nodeList[i].point).normalized;
                    }
                }

                if (!justZero || node.controlRight != Vector3.zero)
                {
                    if (i == 0)
                    {
                        nodeList[i].controlRight = nodeList[i].point + (nodeList[i + 1].point - nodeList[i].point).normalized;
                    }
                    else if (i == len)
                    {
                        nodeList[i].controlRight = nodeList[i].point;
                    }
                    else
                    {
                        nodeList[i].controlRight = nodeList[i].point + (nodeList[i + 1].point - nodeList[i].point).normalized;
                    }
                }
            }
        }
    }
}
