using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI物体根节点
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        public Transform bottomLayer;
        public Transform normalLayer;
        public Transform topLayer;
        public Transform constLayer;

        public Camera uiCamera;
    }
}