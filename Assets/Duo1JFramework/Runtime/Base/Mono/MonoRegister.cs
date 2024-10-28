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
        protected Register Reg
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

        /// <summary>
        /// 重置注册器
        /// </summary>
        protected void ResetRegister()
        {
            Reg.Reset();
        }

        /// <summary>
        /// 销毁注册器
        /// </summary>
        protected void DisposeRegister()
        {
            if (register != null)
            {
                register.Dispose();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeRegister();
        }
    }
}
