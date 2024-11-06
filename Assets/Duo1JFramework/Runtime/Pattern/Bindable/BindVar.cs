using System;

namespace Duo1JFramework.Pattern.Bindable
{
    /// <summary>
    /// 可绑定变量
    /// </summary>
    public class BindVar<T>
    {
        /// <summary>
        /// 变量值
        /// </summary>
        public T Value
        {
            get => mValue;
            set
            {
                if (comparer == null)
                {
                    if ((mValue == null && value == null) || mValue.Equals(value))
                    {
                        return;
                    }
                }
                else if (comparer.Invoke(mValue, value))
                {
                    return;
                }

                mValue = value;
                onValueChange?.Invoke(mValue);
            }
        }

        private T mValue;

        /// <summary>
        /// 设置变量值, 不通知
        /// </summary>
        public T RawValue
        {
            set => mValue = value;
        }

        private event Action<T> onValueChange;

        private Func<T, T, bool> comparer;

        /// <summary>
        /// 注册变量值改变回调
        /// </summary>
        public BindVar<T> Register(Action<T> onValueChange)
        {
            this.onValueChange += onValueChange;
            return this;
        }

        /// <summary>
        /// 取消注册变量值改变回调
        /// </summary>
        public BindVar<T> UnRegister(Action<T> onValueChange)
        {
            this.onValueChange -= onValueChange;
            return this;
        }

        /// <summary>
        /// 设置相同变量值比较器
        /// </summary>
        public BindVar<T> SetComparer(Func<T, T, bool> comparer)
        {
            this.comparer = comparer;
            return this;
        }

        public BindVar(T defaultValue)
        {
            mValue = defaultValue;
        }
    }
}
