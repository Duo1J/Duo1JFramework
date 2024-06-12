using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Duo1JFramework.TimelineAPI
{
    [Serializable]
    public class EndPauseBehaviour : BaseTLBehaviour
    {
        public int resumeMouse = Def.INPUT_MOUSE_LEFT;
        public KeyCode resumeKey = KeyCode.Space;

        private bool canPause = false;
        private bool havePaused = false;
        private bool isPaused = false;

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (canPause && !havePaused && !isPaused)
            {
                canPause = false;
                havePaused = true;
                isPaused = true;

                if (Game.IsPlaying)
                {
                    TimelineManager.Instance.SetResumablePause(playable, resumeMouse, resumeKey);
                }
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (!canPause && !havePaused)
            {
                canPause = true;
            }
        }
    }
}