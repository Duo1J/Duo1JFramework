using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 游戏管理器
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
            Root.StaticDispose();
        }
    }
}