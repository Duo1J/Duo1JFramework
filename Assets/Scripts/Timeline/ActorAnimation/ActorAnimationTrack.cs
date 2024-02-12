using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(ActorAnimationClip))]
[TrackBindingType(typeof(GameObject))]
public class ActorAnimationTrack : TrackAsset
{
    public GameObject target;
}
