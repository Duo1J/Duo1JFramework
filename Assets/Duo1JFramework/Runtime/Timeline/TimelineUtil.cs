using UnityEngine.Playables;

namespace Duo1JFramework.TimelineAPI
{
    /// <summary>
    /// Timeline工具类
    /// </summary>
    public static class TimelineUtil
    {
        /// <summary>
        /// 通过Playable获取其依赖的PlayableDirector
        /// </summary>
        public static PlayableDirector GetDirectorByPlayble(Playable playable)
        {
            PlayableDirector director = playable.GetGraph().GetResolver() as PlayableDirector;
            if (director == null)
            {
                Log.ErrorForce("Playable转换PlayableDirector异常");
            }

            return director;
        }
    }
}