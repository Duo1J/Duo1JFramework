using Newtonsoft.Json;
using System;

namespace Duo1JFramework
{
    /// <summary>
    /// Json工具
    /// </summary>
    public class JsonUtil
    {
        /// <summary>
        /// object转Json
        /// </summary>
        public static string ToJson(object o, Formatting formatting = Formatting.None)
        {
            try
            {
                return JsonConvert.SerializeObject(o, formatting);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "object转Json异常");
                return "";
            }
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        public static T ToObject<T>(string jsonStr)
        {
            Assert.NotNullArg(jsonStr, "jsonStr");

            try
            {
                T ret = JsonConvert.DeserializeObject<T>(jsonStr);
                if (ret == null)
                {
                    Log.ErrorForce($"Json转object失败:\n{jsonStr}");
                }

                return ret;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "Json转object异常");
                return default(T);
            }
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        public static object ToObject(string jsonStr)
        {
            try
            {
                object ret = JsonConvert.DeserializeObject(jsonStr);
                if (ret == null)
                {
                    Log.ErrorForce($"Json转object失败:\n{jsonStr}");
                }

                return ret;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "Json转object异常");
                return null;
            }
        }

        private JsonUtil()
        {
        }
    }
}
