using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework
{
    public static partial class Def
    {
        /// <summary>
        /// 资源相关定义
        /// </summary>
        public static partial class Asset
        {
            /// <summary>
            /// 最大AssetBundle卸载空闲等待时间
            /// </summary>
            public const float MAX_AB_FREE_TIME = 5;

            /// <summary>
            /// 是否加密ABMapData
            /// </summary>
            public static bool EncryptABMapData = true;

            /// <summary>
            /// ABMapData加密AES私钥
            /// </summary>
            public static byte[] ABMapDataAESKeyByte => AesKeyByte;

            /// <summary>
            /// Resources.UnloadAsset忽略资源类型
            /// </summary>
            public static readonly Dictionary<Type, bool> UnloadIgnoreType = new Dictionary<Type, bool>()
            {
                [typeof(GameObject)] = true,
                [typeof(Component)] = true,
            };
        }
    }
}
