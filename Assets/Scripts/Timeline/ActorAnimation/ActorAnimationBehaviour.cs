using Duo1JFramework;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ActorAnimationBehaviour : PlayableBehaviour
{
    public ActorAnimationClip clip;
    public TrackAsset trackAsset;

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (clip == null) return;
    }
}
