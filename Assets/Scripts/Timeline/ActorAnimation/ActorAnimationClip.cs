using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ActorAnimationClip : PlayableAsset, ITimelineClipAsset
{
    public ActorAnimationBehaviour behaviour = new ActorAnimationBehaviour();

    public float clipParam;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        ScriptPlayable<ActorAnimationBehaviour> playable = ScriptPlayable<ActorAnimationBehaviour>.Create(graph, behaviour);
        playable.GetBehaviour().clip = this;
        return playable;
    }
}
