using System.Collections;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 协程管理器
    /// </summary>
    public class CoroManager : MonoSingleton<CoroManager>
    {
        public static Coroutine StartCoro(IEnumerator e)
        {
            return Instance.StartCoroutine(e);
        }

        public static void StopCoro(IEnumerator e)
        {
            Instance.StopCoroutine(e);
        }

        public static void StopCoro(Coroutine c)
        {
            Instance.StopCoroutine(c);
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}