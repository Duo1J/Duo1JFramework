namespace Duo1JFramework
{
    /// <summary>
    /// 自增ID
    /// </summary>
    public class AutoIncID
    {
        public static AutoIncID Create => new AutoIncID();

        public long NewId
        {
            get
            {
                long ret = curId;
                if (++curId >= long.MaxValue)
                {
                    curId = 0;
                }
                return ret;
            }
        }
        private long curId = 0;
    }
}