using Duo1JFramework.DataStructure;
using System.Collections.Generic;

namespace Duo1JFramework.Pattern.Pipeline
{
    /// <summary>
    /// 管线环境上下文
    /// </summary>
    public class PipelineContext : IPipelineContext
    {
        private TypeSet typeSet;

        private Dictionary<string, object> keyDict;

        /// <summary>
        /// 通过类型设置参数
        /// </summary>
        public void Set<T>(T obj)
        {
            if (typeSet == null)
            {
                typeSet = new TypeSet();
            }

            typeSet.Add<T>(obj);
        }

        /// <summary>
        /// 通过Key设置参数
        /// </summary>
        public void Set<T>(string key, T obj)
        {
            if (keyDict == null)
            {
                keyDict = new Dictionary<string, object>();
            }

            if (keyDict.ContainsKey(key))
            {
                keyDict[key] = obj;
            }
            else
            {
                keyDict.Add(key, obj);
            }
        }

        /// <summary>
        /// 尝试通过类型获取参数
        /// </summary>
        public bool TryGet<T>(out T value, bool suppressLog = false)
        {
            if (typeSet != null && typeSet.TryGetValue<T>(out value))
            {
                return true;
            }

            if (!suppressLog)
            {
                Log.EditorError($"无法获取到管线环境上下文, Type: `{typeof(T).FullName}`");
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// 尝试通过Key获取参数
        /// </summary>
        public bool TryGet<T>(string key, out T value, bool suppressLog = false)
        {
            if (keyDict != null && keyDict.TryGetValue(key, out object obj))
            {
                value = obj.Convert<T>();
                return true;
            }

            if (!suppressLog)
            {
                Log.EditorError($"无法获取到管线环境上下文, Key: `{key}`");
            }

            value = default(T);
            return false;
        }
    }
}
