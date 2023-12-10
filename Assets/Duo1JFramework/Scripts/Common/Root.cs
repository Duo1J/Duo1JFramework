using Duo1JFramework.Asset;
using Duo1JFramework.UI;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 节点管理
    /// </summary>
    public static class Root
    {
        /// <summary>
        /// 单例物体根节点
        /// </summary>
        public static GameObject SingletonRoot
        {
            get
            {
                if (singletonRoot == null)
                {
                    singletonRoot = new GameObject("SingletonRoot");
                    Object.DontDestroyOnLoad(singletonRoot);
                }
                return singletonRoot;
            }
        }
        private static GameObject singletonRoot;

        /// <summary>
        /// UI根节点
        /// </summary>
        public static UIRoot UIRoot
        {
            get
            {
                if (uiRoot == null)
                {
                    GameObject uiRootGo = AssetManager.Instance.LoadSync<GameObject>(Def.UI_ROOT_PATH);
                    uiRoot = uiRootGo.GetComponent<UIRoot>();
                }
                return uiRoot;
            }
        }
        private static UIRoot uiRoot;
    }
}