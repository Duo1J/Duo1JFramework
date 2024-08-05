namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 对象池对象包装
    /// </summary>
    public class ObjectPoolItem<T> where T : class, new()
    {
        public T Value { get; set; }
        public bool Using { get; set; }

        public ObjectPoolItem(T o)
        {
            Value = o;
            Using = true;
        }
    }
}
