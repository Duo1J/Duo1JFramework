using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Duo1JFramework.TimelineAPI
{
    [Serializable]
    public class EndPauseBehaviour : BaseTLBehaviour
    {
        private const double FINISH_TIME_TOLERANCE = 0.0001d;

        public int resumeMouse = Def.Input.INPUT_MOUSE_LEFT;
        public KeyCode resumeKey = KeyCode.Space;

        private bool canPause = false;
        private bool havePaused = false;

        public override void OnGraphStart(Playable playable)
        {
            ResetState();
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (havePaused)
            {
                ResetState();
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (canPause && !havePaused && IsPlayableFinished(playable))
            {
                canPause = false;
                havePaused = true;

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

        private void ResetState()
        {
            canPause = false;
            havePaused = false;
        }

        private bool IsPlayableFinished(Playable playable)
        {
            double duration = playable.GetDuration();
            if (double.IsInfinity(duration) || duration <= 0d)
            {
                return true;
            }

            return playable.GetTime() + FINISH_TIME_TOLERANCE >= duration;
        }
    }
}