namespace Duo1JFramework
{
    /// <summary>
    /// 参数异常
    /// </summary>
    public class ArgExcept : Except
    {
        public ArgExcept(string message) : base(message)
        {
        }

        public override string Message
        {
            get
            {
                return $"[参数异常] {base.Message}";
            }
        }
    }
}
