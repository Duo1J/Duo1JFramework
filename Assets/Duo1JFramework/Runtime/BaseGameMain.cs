using Duo1JFramework.Config;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// GameMain基类
    /// </summary>
    [RequireComponent(typeof(GameOption))]
    public class BaseGameMain : MonoRegister
    {
        protected virtual void Awake()
        {
            Framework.Init();
        }
    }
}
