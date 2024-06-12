using UnityEngine;
using UnityEngine.Playables;

namespace Duo1JFramework.TimelineAPI
{
    public class EndPauseClip : BaseTLClip
    {
        public EndPauseBehaviour behaviour = new EndPauseBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<EndPauseBehaviour>.Create(graph, behaviour);
        }
    }
}