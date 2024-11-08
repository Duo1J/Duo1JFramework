using System.Collections;
using UnityEngine;

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// Mono协程管理器
    /// </summary>
    public class CoroManager : MonoSingleton<CoroManager>
    {
        public static readonly YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();

        public static readonly YieldInstruction WaitForFixedUpdate = new WaitForFixedUpdate();

        /// <summary>
        /// 开启Mono协程
        /// </summary>
        public static Coroutine StartCoro(IEnumerator e)
        {
            return Instance.StartCoroInner(e);
        }

        /// <summary>
        /// 停止Mono协程
        /// </summary>
        public static void StopCoro(IEnumerator e)
        {
            Instance.StopCoroInner(e);
        }

        /// <summary>
        /// 停止Mono协程
        /// </summary>
        public static void StopCoro(Coroutine c)
        {
            Instance.StopCoroInner(c);
        }

        #region Inner

        /// <summary>
        /// 内部开启Mono协程
        /// </summary>
        private Coroutine StartCoroInner(IEnumerator e)
        {
            Assert.NotNullArg(e, "e");
            return StartCoroutine(e);
        }

        /// <summary>
        /// 内部停止Mono协程
        /// </summary>
        private void StopCoroInner(IEnumerator e)
        {
            Assert.NotNullArg(e, "e");
            StopCoroutine(e);
        }

        /// <summary>
        /// 内部停止Mono协程
        /// </summary>
        private void StopCoroInner(Coroutine c)
        {
            Assert.NotNullArg(c, "c");
            StopCoroutine(c);
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }

        #endregion Inner
    }
}
