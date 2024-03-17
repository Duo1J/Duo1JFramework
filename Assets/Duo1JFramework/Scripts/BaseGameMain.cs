using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// GameMain基类
    /// </summary>
    public class BaseGameMain : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Framework.Init();
        }
    }
}