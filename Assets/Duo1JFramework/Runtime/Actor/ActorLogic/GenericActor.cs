namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 泛型通用角色逻辑
    /// </summary>
    public abstract class GenericActor<T> : BaseActor where T : BaseActorController
    {
        /// <summary>
        /// 泛型Actor控制器
        /// </summary>
        protected T Con
        {
            get
            {
                if (con == null)
                {
                    con = Controller.Convert<T>();
                }

                return con;
            }
        }

        private T con;
    }
}