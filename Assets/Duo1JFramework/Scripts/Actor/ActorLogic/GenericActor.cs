namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 泛型通用角色
    /// </summary>
    public class GenericActor<T> : BaseActor where T : ActorController
    {
        /// <summary>
        /// 泛型Actor控制器
        /// </summary>
        protected T Con
        {
            get
            {
                if (con == null) con = Controller as T;
                return con;
            }
        }
        private T con;

        protected override void OnCreated()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}