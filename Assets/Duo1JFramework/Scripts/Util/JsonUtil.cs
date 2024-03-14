using Newtonsoft.Json;

namespace Duo1JFramework
{
    /// <summary>
    /// Json工具
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// 转对象转Json
        /// </summary>
        public static string ToJson(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        public static T ToObject<T>(string jsonStr)
        {
            Assert.NotNull(jsonStr, "Json字符串不可为null");

            T ret = JsonConvert.DeserializeObject<T>(jsonStr);
            if (ret == null)
            {
                Log.ErrorForce($"Json转object失败:\n{jsonStr}");
            }

            return ret;
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        public static object ToObject(string jsonStr)
        {
            object ret = JsonConvert.DeserializeObject(jsonStr);
            if (ret == null)
            {
                Log.ErrorForce($"Json转object失败:\n{jsonStr}");
            }

            return ret;
        }
    }
}