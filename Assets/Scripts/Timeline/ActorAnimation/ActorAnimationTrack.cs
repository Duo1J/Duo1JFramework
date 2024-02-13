using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(ActorAnimationClip))]
[TrackBindingType(typeof(GameObject))]
public class ActorAnimationTrack : TrackAsset
{
    public string trackParam = "trackParam";
}
