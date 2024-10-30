namespace Duo1JFramework.Event
{
    /// <summary>
    /// 类型事件委托
    /// </summary>
    public delegate void TypeEventFunc<in T>(T args) where T : BaseTypeEvent;

}
