using Duo1JFramework.Config;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// GameMain基类
    /// </summary>
    [RequireComponent(typeof(GameConfig))]
    public class BaseGameMain : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Framework.Init();
        }
    }
}