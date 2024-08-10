namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 泛型基础角色逻辑
    /// </summary>
    public abstract class GenericActor<T> : Actor where T : BaseActorController
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