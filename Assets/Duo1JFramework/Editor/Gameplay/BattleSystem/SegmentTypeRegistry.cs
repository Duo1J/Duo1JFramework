using System;
using System.Collections.Generic;
using System.Reflection;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// Segment类型注册
    /// </summary>
    public static class SegmentTypeRegistry
    {
        /// <summary>
        /// 每种轨道对应的候选Segment类型
        /// </summary>
        private static readonly Dictionary<ESequenceTrackType, List<Type>> map =
            new Dictionary<ESequenceTrackType, List<Type>>();

        /// <summary>
        /// 类型显示名
        /// </summary>
        private static readonly Dictionary<Type, string> displayNames = new Dictionary<Type, string>();

        static SegmentTypeRegistry()
        {
            Register(ESequenceTrackType.Animation, typeof(AnimationSegment), "动画片段");
            Register(ESequenceTrackType.HitBox, typeof(HitBoxSegment), "判定盒");
            Register(ESequenceTrackType.EffectApply, typeof(EffectApplySegment), "效果应用");
            Register(ESequenceTrackType.Vfx, typeof(VfxSegment), "特效");
            Register(ESequenceTrackType.Sfx, typeof(SfxSegment), "音效");
            Register(ESequenceTrackType.Movement, typeof(MovementSegment), "位移");
            Register(ESequenceTrackType.CameraShake, typeof(CameraShakeSegment), "相机震动");
            Register(ESequenceTrackType.Event, typeof(EventSegment), "自定义事件");

            AutoScan();
        }

        /// <summary>
        /// 注册片段
        /// </summary>
        public static void Register(ESequenceTrackType track, Type type, string displayName = null)
        {
            if (!typeof(SequenceSegment).IsAssignableFrom(type) || type.IsAbstract)
            {
                return;
            }

            if (!map.TryGetValue(track, out List<Type> list))
            {
                list = new List<Type>();
                map[track] = list;
            }
            if (!list.Contains(type))
            {
                list.Add(type);
            }
            displayNames[type] = string.IsNullOrEmpty(displayName) ? type.Name : displayName;
        }

        /// <summary>
        /// 获取轨道下所有片段类型
        /// </summary>
        public static IReadOnlyList<Type> Get(ESequenceTrackType track)
        {
            return map.TryGetValue(track, out List<Type> list) ? list : (IReadOnlyList<Type>)Array.Empty<Type>();
        }

        /// <summary>
        /// 获取显示名
        /// </summary>
        public static string GetDisplay(Type t)
        {
            return displayNames.TryGetValue(t, out string s) ? s : t.Name;
        }

        /// <summary>
        /// 自动扫描, 通过[SegmentMenu]自定义
        /// </summary>
        private static void AutoScan()
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < asms.Length; i++)
            {
                Type[] types;
                try
                {
                    types = asms[i].GetTypes();
                }
                catch
                {
                    continue;
                }

                foreach (Type t in types)
                {
                    if (t == null || t.IsAbstract)
                    {
                        continue;
                    }
                    if (!typeof(SequenceSegment).IsAssignableFrom(t))
                    {
                        continue;
                    }

                    SegmentMenuAttribute attr = t.GetCustomAttribute<SegmentMenuAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }

                    Register(attr.Track, t, attr.DisplayName);
                }
            }
        }
    }

    /// <summary>
    /// 用于标注自定义Segment的菜单项
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SegmentMenuAttribute : Attribute
    {
        public ESequenceTrackType Track;
        public string DisplayName;

        public SegmentMenuAttribute(ESequenceTrackType track, string displayName)
        {
            Track = track;
            DisplayName = displayName;
        }
    }
}
