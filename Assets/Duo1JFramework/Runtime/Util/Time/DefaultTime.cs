using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 默认时间方法实现
    /// </summary>
    public class DefaultTime : ITime
    {
        public float CurTime => Time.time;
    }
}
