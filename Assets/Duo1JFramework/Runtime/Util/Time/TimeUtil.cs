namespace Duo1JFramework
{
    /// <summary>
    /// 时间工具
    /// </summary>
    public class TimeUtil
    {
        /// <summary>
        /// 时间方法实现
        /// </summary>
        public static ITime Impl
        {
            get
            {
                if (impl == null)
                {
                    impl = new DefaultTime();
                }

                return impl;
            }
            set => impl = value;
        }
        private static ITime impl;

        /// <summary>
        /// 当前时间
        /// </summary>
        public static float CurTime => Impl.CurTime;

        private TimeUtil()
        {
        }
    }
}
