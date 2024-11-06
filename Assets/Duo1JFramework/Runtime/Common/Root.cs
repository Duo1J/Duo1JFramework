using Duo1JFramework.Asset;
using Duo1JFramework.UI;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 场景节点管理
    /// </summary>
    public class Root
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
                    if (Game.IsQuit)
                    {
                        Log.ErrorForce("游戏状态已退出，但仍在创建SingletonRoot");
                        return null;
                    }
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
                    IAssetHandle<GameObject> uiRootGoHandle = AssetManager.Instance.LoadResourceSync<GameObject>(Def.UI.ROOT_PATH);
                    GameObject uiRootGo = uiRootGoHandle.Instantiate();
                    uiRootGoHandle.Release();
                    uiRootGo.transform.position = Def.UI.ROOT_DEFAULT_POS;
                    uiRoot = uiRootGo.GetComponent<UIRoot>();
                    Object.DontDestroyOnLoad(uiRootGo);
                }
                return uiRoot;
            }
        }
        private static UIRoot uiRoot;

        /// <summary>
        /// Actor根节点
        /// </summary>
        public static Transform ActorRoot
        {
            get
            {
                if (actorRoot == null)
                {
                    GameObject go = new GameObject("ActorRoot");
                    go.ResetSRT();
                    actorRoot = go.transform;
                }
                return actorRoot;
            }
        }
        private static Transform actorRoot;

        /// <summary>
        /// 世界场景根节点
        /// </summary>
        public static Transform WorldRoot
        {
            get
            {
                if (worldRoot == null)
                {
                    GameObject go = new GameObject("WorldRoot");
                    go.ResetSRT();
                    worldRoot = go.transform;
                }
                return worldRoot;
            }
        }
        private static Transform worldRoot;

        /// <summary>
        /// 虚拟相机根节点
        /// </summary>
        public static GameObject VirtualCameraRoot
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
        private static GameObject virtualCameraRoot;

        /// <summary>
        /// Timeline根节点
        /// </summary>
        public static GameObject TimelineRoot
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
        private static GameObject timelineRoot;

        /// <summary>
        /// GameObject对象池根节点
        /// </summary>
        public static GameObject GoPoolRoot
        {
            get
            {
                if (goPoolRoot == null)
                {
                    goPoolRoot = new GameObject("GoPoolRoot");
                    Object.DontDestroyOnLoad(goPoolRoot);
                }
                return goPoolRoot;
            }
        }
        private static GameObject goPoolRoot;

        private Root()
        {
        }
    }
}
