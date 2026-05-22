using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// Unity Object 相关扩展
    /// </summary>
    public static class ObjectExtend
    {
        /// <summary>
        /// 获取名字和InstanceID字符串用于调试输出
        /// </summary>
        public static string GetNameInsID(this UObject obj)
        {
            return $"[{obj.name}#{obj.GetInstanceID()}]";
        }

        /// <summary>
        /// 立即销毁
        /// </summary>
        public static void DestroyImmediate(this UObject obj, bool allowDestroyingAssets = false)
        {
            UObject.DestroyImmediate(obj, allowDestroyingAssets);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public static void Destroy(this UObject obj, float t = 0)
        {
            UObject.Destroy(obj, t);
        }

        /// <summary>
        /// 智能销毁
        /// </summary>
        public static void DestroySmart(this UObject obj)
        {
            if (obj == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                obj.Destroy(0);
            }
            else
            {
                obj.DestroyImmediate(false);
            }
        }

        #region Convert

        /// <summary>
        /// object类型转换，转换失败打印错误
        /// </summary>
        public static T Convert<T>(this object obj, string msg = null)
        {
            return Assert.Convert<T>(obj, msg);
        }

        /// <summary>
        /// object类型转换，转换失败抛出异常
        /// </summary>
        public static T ConvertGuard<T>(this object obj, string msg = null)
        {
            return Assert.ConvertGuard<T>(obj, msg);
        }

        /// <summary>
        /// object类型结构体转换，转换失败打印错误
        /// </summary>
        public static T StructConvert<T>(this object obj, string msg = null) where T : struct
        {
            return Assert.StructConvert<T>(obj, msg);
        }

        /// <summary>
        /// object类型结构体转换，转换失败抛出异常
        /// </summary>
        public static T StructConvertGuard<T>(this object obj, string msg = null) where T : struct
        {
            return Assert.StructConvertGuard<T>(obj, msg);
        }

        #endregion Convert
    }
}