using System;
using UnityEngine;

namespace Duo1JFramework.AnimationAPI
{
    /// <summary>
    /// Root Motion控制器
    /// </summary>
    [DisallowMultipleComponent]
    public class RootMotionController : BaseMono
    {
        private Action onAnimatorMove;

        /// <summary>
        /// 设置OnAnimatorMove回调
        /// </summary>
        public void SetOnAnimatorMove(Action onAnimatorMove)
        {
            this.onAnimatorMove = onAnimatorMove;
        }

        private void OnAnimatorMove()
        {
            onAnimatorMove?.Invoke();
        }
    }
}
