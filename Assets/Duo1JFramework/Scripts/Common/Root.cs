using Duo1JFramework.Asset;
using Duo1JFramework.UI;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 节点管理
    /// </summary>
    public class Root : MonoSingleton<Root>
    {
        /// <summary>
        /// 单例物体根节点
        /// </summary>
        public GameObject SingletonRoot
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
        private GameObject singletonRoot;

        /// <summary>
        /// UI根节点
        /// </summary>
        public UIRoot UIRoot
        {
            get
            {
                if (uiRoot == null)
                {
                    GameObject uiRootGo = AssetManager.Instance.LoadResource<GameObject>(Def.UI_ROOT_PATH);
                    uiRoot = uiRootGo.GetComponent<UIRoot>();
                }
                return uiRoot;
            }
        }
        private UIRoot uiRoot;

        /// <summary>
        /// 虚拟相机根节点
        /// </summary>
        public GameObject VirtualCameraRoot
        {
            get
            {
                if (virtualCameraRoot == null)
                {
                    virtualCameraRoot = new GameObject("VirtualCameraRoot");
                }
                return virtualCameraRoot;
            }
        }
        private GameObject virtualCameraRoot;

        /// <summary>
        /// Timeline根节点
        /// </summary>
        public GameObject TimelineRoot
        {
            get
            {
                if (timelineRoot == null)
                {
                    timelineRoot = new GameObject("TimelineRoot");
                }
                return timelineRoot;
            }
        }
        private GameObject timelineRoot;

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}