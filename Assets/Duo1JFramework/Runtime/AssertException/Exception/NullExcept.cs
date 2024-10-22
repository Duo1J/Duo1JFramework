namespace Duo1JFramework
{
    /// <summary>
    /// 空指针异常
    /// </summary>
    public class NullExcept : Except
    {
        public NullExcept(string message) : base(message)
        {
        }

        public override string Message
        {
            get
            {
                return $"[空指针] {base.Message}";
            }
        }
    }
}
