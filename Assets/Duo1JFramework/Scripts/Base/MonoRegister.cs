namespace Duo1JFramework
{
    /// <summary>
    /// Monobehaviour注册器
    /// </summary>
    public class MonoRegister : BaseMono
    {
        /// <summary>
        /// 注册器
        /// </summary>
        protected Register Register
        {
            get
            {
                if (register == null)
                {
                    register = new Register();
                }
                return register;
            }
        }
        private Register register;

        protected virtual void OnDestroy()
        {
            if (register != null)
            {
                register.Dispose();
            }
        }
    }
}