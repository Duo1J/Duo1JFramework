using UnityEngine;

namespace Duo1JFramework.UI
{
    public abstract class Window
    {
        /// <summary>
        /// UI物体
        /// </summary>
        public GameObject Go { get; set; }

        /// <summary>
        /// 获取UI配置
        /// </summary>
        public abstract UIConfig GetUIConfig();
    }
}