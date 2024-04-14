namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 通用对象池
    /// 需要在Pop后自行初始化
    /// </summary>
    public class CommonPool<T> : BaseObjectPool<T> where T : new()
    {
        public static CommonPool<T> Create()
        {
            return new CommonPool<T>();
        }
    }
}