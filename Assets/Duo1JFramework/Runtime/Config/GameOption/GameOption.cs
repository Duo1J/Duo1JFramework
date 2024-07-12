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
        public static RuntimeGameOption Runtime => Instance.runtime;

        /// <summary>
        /// 编辑器下游戏配置选项
        /// </summary>
        public static EditorGameOption Editor => Instance.editor;

        [Header("运行时")]
        [SerializeField]
        private RuntimeGameOption runtime;

        [Header("编辑器")]
        [SerializeField]
        private EditorGameOption editor;

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}