namespace Duo1JFramework.UI
{
    /// <summary>
    /// 泛型UI窗口逻辑
    /// </summary>
    public abstract class GenericWindow<T> : Window where T : Window, new()
    {
        /// <summary>
        /// 打开窗口
        /// </summary>
        public static T Open()
        {
            return UIManager.Instance.OpenWindow<T>();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static bool Close()
        {
            return UIManager.Instance.CloseWindow<T>();
        }

        /// <summary>
        /// 切换打开和关闭状态
        /// </summary>
        public static void Switch()
        {
            if (IsOpened())
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        /// <summary>
        /// 回退到此窗口
        /// </summary>
        public static Window BackToThis()
        {
            return UIManager.Instance.BackToWindow<T>();
        }

        /// <summary>
        /// 此窗口是否已打开
        /// </summary>
        public static bool IsOpened()
        {
            return UIManager.Instance.IsWindowOpened<T>();
        }
    }
}
