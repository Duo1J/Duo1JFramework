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
        public void Set<T>(T obj) where T : class
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
        public void Set<T>(string key, T obj) where T : class
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
        public bool TryGet<T>(out T value) where T : class
        {
            if (typeSet == null)
            {
                typeSet = new TypeSet();
            }

            return typeSet.TryGetValue<T>(out value);
        }

        /// <summary>
        /// 尝试通过Key获取参数
        /// </summary>
        public bool TryGet<T>(string key, out T value) where T : class
        {
            if (keyDict == null)
            {
                value = default(T);
                return false;
            }

            if (keyDict.TryGetValue(key, out object obj))
            {
                value = obj as T;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}
