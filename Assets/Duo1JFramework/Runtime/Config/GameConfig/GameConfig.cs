using UnityEngine;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    public class GameConfig : MonoSingleton<GameConfig>
    {
        /// <summary>
        /// 运行时游戏配置
        /// </summary>
        public static RuntimeGameConfig Runtime => Instance.runtime;

        /// <summary>
        /// 编辑器下游戏配置
        /// </summary>
        public static EditorGameConfig Editor => Instance.editor;

        [Header("运行时")]
        [SerializeField]
        private RuntimeGameConfig runtime;

        [Header("编辑器")]
        [SerializeField]
        private EditorGameConfig editor;

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}