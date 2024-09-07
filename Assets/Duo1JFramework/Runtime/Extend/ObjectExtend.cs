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

        #region Convert

        /// <summary>
        /// UObject类型转换，转换失败打印错误
        /// </summary>
        public static T Convert<T>(this UObject obj, string msg = null) where T : UObject
        {
            return Assert.Convert<T>(obj, msg);
        }

        /// <summary>
        /// object类型转换，转换失败打印错误
        /// </summary>
        public static T Convert<T>(this object obj, string msg = null) where T : class
        {
            return Assert.Convert<T>(obj, msg);
        }

        /// <summary>
        /// object类型结构体转换，转换失败打印错误
        /// </summary>
        public static T StructConvert<T>(this object obj, string msg = null) where T : struct
        {
            return Assert.StructConvert<T>(obj, msg);
        }

        #endregion Convert
    }
}