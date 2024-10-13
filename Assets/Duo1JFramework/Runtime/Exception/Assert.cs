using System;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 断言
    /// </summary>
    public class Assert
    {
        /// <summary>
        /// 通用异常处理
        /// </summary>
        public static void ExceptHandle(Exception e, params object[] msg)
        {
            Log.Exception(e, msg);
        }

        /// <summary>
        /// 抛出一般异常
        /// </summary>
        public static void Throw(string msg)
        {
            CommonException.Throw(msg);
        }

        /// <summary>
        /// 断言不为空
        /// </summary>
        public static void NotNull(object o, string msg = null)
        {
            if (o == null)
            {
                if (msg == null)
                {
                    Throw("<空指针异常>");
                }
                else
                {
                    Throw($"<空指针异常>: {msg}");
                }
            }
        }

        /// <summary>
        /// 断言字符串是否不为null以及""，否则抛出异常
        /// </summary>
        public static void NotNullOrEmpty(string str, string msg = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                if (msg == null)
                {
                    Throw("<字符串异常>");
                }
                else
                {
                    Throw($"<字符串异常>: {msg}");
                }
            }
        }

        /// <summary>
        /// 判断是否为真，否则抛出异常
        /// </summary>
        public static void Guard(bool b, string msg = null)
        {
            if (!b)
            {
                if (msg == null)
                {
                    Throw("<Guard异常>");
                }
                else
                {
                    Throw($"<Guard异常>: {msg}");
                }
            }
        }

        /// <summary>
        /// 断言Editor下
        /// </summary>
        public static void GuardEditor(string msg = null)
        {
            if (!Game.IsEditor)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Throw("非Editor下不可调用");
                }
                else
                {
                    Throw($"非Editor下不可调用\n{msg}");
                }
            }
        }

        /// <summary>
        /// 断言Runtime下
        /// </summary>
        public static void GuardRuntime(string msg = null)
        {
            if (Game.IsEditor)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Throw("非Runtime下不可调用");
                }
                else
                {
                    Throw($"非Runtime下不可调用\n{msg}");
                }
            }
        }

        /// <summary>
        /// UObject类型转换，转换失败打印错误
        /// </summary>
        public static T Convert<T>(UObject target, string msg = null) where T : UObject
        {
            if (target == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.ErrorForce($"类型转换失败, 空值转{typeof(T).FullName}");
                }
                else
                {
                    Log.ErrorForce($"类型转换失败, 空值转{typeof(T).FullName}\n{msg}");
                }
                return null;
            }

            T ret = target as T;
            if (ret == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.ErrorForce($"类型转换失败, {target}转{typeof(T).FullName}");
                }
                else
                {
                    Log.ErrorForce($"类型转换失败, {target}转{typeof(T).FullName}\n{msg}");
                }
            }

            return ret;
        }

        /// <summary>
        /// object类型转换，转换失败打印错误
        /// </summary>
        public static T Convert<T>(object target, string msg = null) where T : class
        {
            if (target == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.ErrorForce($"类型转换失败, 空值转{typeof(T).FullName}");
                }
                else
                {
                    Log.ErrorForce($"类型转换失败, 空值转{typeof(T).FullName}\n{msg}");
                }
                return null;
            }

            T ret = target as T;
            if (ret == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.ErrorForce($"类型转换失败, {target}转{typeof(T).FullName}");
                }
                else
                {
                    Log.ErrorForce($"类型转换失败, {target}转{typeof(T).FullName}\n{msg}");
                }
            }

            return ret;
        }

        /// <summary>
        /// object类型结构体转换，转换失败打印错误
        /// </summary>
        public static T StructConvert<T>(object target, string msg = null) where T : struct
        {
            if (target == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    Log.ErrorForce($"类型转换失败, 空值转{typeof(T).FullName}");
                }
                else
                {
                    Log.ErrorForce($"类型转换失败, 空值转{typeof(T).FullName}\n{msg}");
                }
                return default(T);
            }

            T ret = default(T);
            try
            {
                ret = (T)target;
            }
            catch (Exception e)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    ExceptHandle(e, $"类型转换失败, {target}转{typeof(T).FullName}");
                }
                else
                {
                    ExceptHandle(e, $"类型转换失败, {target}转{typeof(T).FullName}\n{msg}");

                }
            }

            return ret;
        }

        private Assert()
        {
        }
    }
}
