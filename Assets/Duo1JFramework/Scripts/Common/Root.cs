using Duo1JFramework.Asset;
using Duo1JFramework.UI;
using Duo1JFramework.World;
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
                    if (Game.IsQuit)
                    {
                        Log.ErrorForce("游戏状态已退出，但仍在创建SingletonRoot");
                        return null;
                    }
                    singletonRoot = new GameObject("SingletonRoot");
                    DontDestroyOnLoad(singletonRoot);
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
                    GameObject uiRootGo = AssetManager.Instance.LoadResourceInsSync<GameObject>(Def.UI_ROOT_PATH);
                    uiRootGo.transform.position = Def.UI_ROOT_DEFAULT_POS;
                    uiRoot = uiRootGo.GetComponent<UIRoot>();
                }
                return uiRoot;
            }
        }
        private UIRoot uiRoot;

        /// <summary>
        /// Actor根节点
        /// </summary>
        public Transform ActorRoot => WorldManager.Instance.ActorRoot;

        /// <summary>
        /// 世界场景根节点
        /// </summary>
        public Transform WorldRoot => WorldManager.Instance.WorldRoot;

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

        /// <summary>
        /// GameObject对象池根节点
        /// </summary>
        public GameObject GoPoolRoot
        {
            get
            {
                if (goPoolRoot == null)
                {
                    goPoolRoot = new GameObject("GoPoolRoot");
                }
                return goPoolRoot;
            }
        }
        private GameObject goPoolRoot;

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}