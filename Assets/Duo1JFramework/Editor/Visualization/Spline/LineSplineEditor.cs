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
        protected override bool ShowOriginDefault => true;

        private Vector3 labelOffset = new Vector3(0, -0.1f, 0);

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

                    Vector3 pos = nodeList[i];
                    Handles.Label(nodeList[i] + labelOffset, $"<color=#00FF00>P{i} ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})</color>");
                }

                Handles.DrawPolyLine(nodeList);
            });
        }
    }
}