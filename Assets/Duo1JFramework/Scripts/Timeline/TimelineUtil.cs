using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Duo1JFramework.Timeline
{
    /// <summary>
    /// Timeline工具类
    /// </summary>
    public static class TimelineUtil
    {
        /// <summary>
        /// 获取绑定的Track
        /// </summary>
        public static TrackAsset GetTrack(PlayableGraph graph, PlayableAsset playableAsset)
        {
            //todo hlj
            try
            {

                TimelineAsset timelineAsset = graph.GetResolver() as TimelineAsset;
                Assert.NotNull(timelineAsset, "PlayableGraph转TimelineAsset失败");
                foreach (TrackAsset trackAsset in timelineAsset.GetOutputTracks())
                {
                    foreach (TimelineClip clip in trackAsset.GetClips())
                    {
                        if (clip.asset == playableAsset)
                        {
                            return trackAsset;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                return null;
            }
        }
    }
}