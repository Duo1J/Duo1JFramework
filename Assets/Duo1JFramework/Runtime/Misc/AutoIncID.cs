namespace Duo1JFramework
{
    /// <summary>
    /// 自增ID
    /// </summary>
    public class AutoIncID
    {
        public static AutoIncID Create(long startID = 0)
        {
            return new AutoIncID(startID);
        }

        public long NewID
        {
            get
            {
                long ret = curID;
                if (++curID >= long.MaxValue)
                {
                    Log.ErrorForce("自增ID超过最大上限");
                    curID = 0;
                }
                return ret;
            }
        }
        private long curID = 0;

        public void Reset()
        {
            curID = 0;
        }

        public AutoIncID(long startID)
        {
            curID = startID;
        }
    }
}