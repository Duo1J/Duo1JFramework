using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI控制器 (挂载在UI预制体根节点上)
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class UIController : MonoBehaviour
    {
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        /// <summary>
        /// 更新此窗口及其子物体的层级
        /// </summary>
        /// <param name="layer"></param>
        public void UpdateLayer(int layer)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = layer;
        }
    }
}