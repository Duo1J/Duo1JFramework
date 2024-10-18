using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 游戏配置选项
    /// </summary>
    public class GameOption : MonoSingleton<GameOption>
    {
        /// <summary>
        /// 运行时游戏配置选项
        /// </summary>
        private static RuntimeGameOption Runtime => Instance.runtime;

        /// <summary>
        /// 编辑器下游戏配置选项
        /// </summary>
        private static EditorGameOption Editor => Instance.editor;

        [Header("运行时")]
        [SerializeField]
        private RuntimeGameOption runtime;

        [Header("编辑器")]
        [SerializeField]
        private EditorGameOption editor;

        #region API

        /// <summary>
        /// 资源加载器
        /// </summary>
        public static EAssetLoaderType AssetLoaderType
        {
            get
            {
#if UNITY_EDITOR
                return Editor.assetLoaderType;
#else
                return Runtime.assetLoaderType;
#endif
            }
            set
            {
#if UNITY_EDITOR
                Editor.assetLoaderType = value;
#else
                Runtime.assetLoaderType = value;
#endif
            }
        }

        /// <summary>
        /// 使用Log4Net日志
        /// </summary>
        public static bool UseLog4Net
        {
            get
            {
#if UNITY_EDITOR
                return Editor.useLog4Net;
#else
                return Runtime.useLog4Net;
#endif
            }
        }

        #endregion API

        protected override void OnInit()
        {
            if (runtime == null)
            {
                runtime = new RuntimeGameOption();
            }

            if (editor == null)
            {
                editor = new EditorGameOption();
            }
        }

        protected override void OnDispose()
        {
        }
    }
}
