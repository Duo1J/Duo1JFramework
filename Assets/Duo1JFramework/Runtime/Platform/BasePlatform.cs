using UnityEngine;
using UnityEngine.Profiling;

namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台基础实现
    /// </summary>
    public abstract class BasePlatform : IPlatform
    {
        public abstract EPlatform Type { get; }

        public virtual bool IsEditor => Type == EPlatform.Editor;

        public virtual bool IsPC => Type == EPlatform.PC;

        public virtual bool IsMobile => Type == EPlatform.Android || Type == EPlatform.iOS;

        public int GetTotalMemory()
        {
            return SystemInfo.systemMemorySize;
        }

        public long GetTotalReservedMemory()
        {
            return Profiler.GetTotalReservedMemoryLong();
        }

        public long GetUsedHeapSize()
        {
            return Profiler.GetMonoUsedSizeLong();
        }

        public long GetTotalHeapSize()
        {
            return Profiler.GetMonoHeapSizeLong();
        }
    }
}
