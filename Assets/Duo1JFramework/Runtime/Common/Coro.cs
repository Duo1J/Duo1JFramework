using System.Collections;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 协程管理器
    /// </summary>
    public class Coro : MonoSingleton<Coro>
    {
        public Coroutine StartCoro(IEnumerator e)
        {
            return StartCoroutine(e);
        }

        public void StopCoro(IEnumerator e)
        {
            StopCoroutine(e);
        }

        public void StopCoro(Coroutine c)
        {
            StopCoroutine(c);
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}