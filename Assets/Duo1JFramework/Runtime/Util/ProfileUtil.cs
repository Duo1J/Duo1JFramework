using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

namespace Duo1JFramework
{
    /// <summary>
    /// 分析工具类
    /// </summary>
    public static class ProfileUtil
    {
        [Conditional(Def.Symbol.ENABLE_PROFILE)]
        public static void BeginSample(string name)
        {
            Profiler.BeginSample(name);
        }

        [Conditional(Def.Symbol.ENABLE_PROFILE)]
        public static void BeginSample(string name, GameObject go)
        {
            Profiler.BeginSample(name, go);
        }

        [Conditional(Def.Symbol.ENABLE_PROFILE)]
        public static void EndSample()
        {
            Profiler.EndSample();
        }
    }
}
