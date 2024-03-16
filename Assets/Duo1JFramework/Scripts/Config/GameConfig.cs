namespace Duo1JFramework.Config
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    public class GameConfig : MonoSingleton<GameConfig>
    {
        #region Config

        /// <summary>
        /// 编辑器下使用AssetBundle
        /// </summary>
        [Label("编辑器下使用AB")]
        public bool EditorUseAB = false;

        #endregion Config

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}