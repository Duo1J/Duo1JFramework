using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ActorAnimationClip : PlayableAsset
{
    public ActorAnimationBehaviour behaviour = new ActorAnimationBehaviour();

    public float param1;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        return ScriptPlayable<ActorAnimationBehaviour>.Create(graph, behaviour);
    }
}
