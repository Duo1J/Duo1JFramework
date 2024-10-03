using UnityEngine;
using UnityEngine.UI;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// ContentSizeFitter扩展
    /// </summary>
    public class ContentSizeFitterExt : ContentSizeFitter
    {
        /// <summary>
        /// 强制重建Layout
        /// </summary>
        public bool forceRebuild = false;

        /// <summary>
        /// 延迟
        /// </summary>
        public bool delay = true;

        /// <summary>
        /// 延迟帧数
        /// </summary>
        public int delayFrame = 1;

        /// <summary>
        /// 帧计数
        /// </summary>
        private int frameCnt = 2;

        /// <summary>
        /// 标脏
        /// </summary>
        public void SetDirtyDelay()
        {
            if (!delay)
            {
                frameCnt = delayFrame + 1;
                ForceRebuildLayout();
                SetDirty();
                return;
            }

            frameCnt = 0;
        }

        /// <summary>
        /// 强制重建Layout
        /// </summary>
        public void ForceRebuildLayout()
        {
            if (!forceRebuild)
            {
                return;
            }

            RectTransform rectTF = transform as RectTransform;
            if (rectTF == null)
            {
                Log.ErrorForce($"<ContentSizeFitterExt-{name}: {GetInstanceID()}> 未持有`RectTransform`组件, 无法重建布局");
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTF);
        }

        protected override void Awake()
        {
            base.Awake();
            delayFrame = Mathf.Clamp(delayFrame, 0, int.MaxValue - 1);
            frameCnt = delayFrame + 1;
        }

        protected override void OnEnable()
        {
            SetDirtyDelay();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirtyDelay();
        }

        private void LateUpdate()
        {
            if (delay && frameCnt <= delayFrame)
            {
                if (frameCnt == delayFrame)
                {
                    ForceRebuildLayout();
                    SetDirty();
                }

                ++frameCnt;
            }
        }
    }
}
