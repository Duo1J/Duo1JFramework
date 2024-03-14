using System.Collections.Generic;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// Assetbundle映射数据
    /// </summary>
    public class ABMapData
    {
        /// <summary>
        /// AssetBundle下的资源列表映射
        /// </summary>
        public Dictionary<string, List<string>> abAssetList;

        /// <summary>
        /// 资源与AssetBundle映射
        /// </summary>
        private Dictionary<string, string> assetMap;

        /// <summary>
        /// 解析Json
        /// </summary>
        public void Parse(string jsonStr)
        {
            abAssetList = JsonUtil.ToObject<Dictionary<string, List<string>>>(jsonStr);
            //todo hlj
        }

        public static ABMapData Load(string path)
        {
            //todo hlj
            return new ABMapData();
        }
    }
}