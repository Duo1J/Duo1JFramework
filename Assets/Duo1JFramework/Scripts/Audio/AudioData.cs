using Duo1JFramework.Asset;
using System;

namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 音频播放数据
    /// </summary>
    [Serializable]
    public class AudioData
    {
        /// <summary>
        /// 音频路径
        /// </summary>
        public string audioPath;

        /// <summary>
        /// 资源加载方式
        /// </summary>
        public eAssetLoadType loadType = eAssetLoadType.AssetBundle;

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool sync = false;

        public AudioData SetLoadType(eAssetLoadType loadType)
        {
            this.loadType = loadType;
            return this;
        }

        public AudioData SetSync(bool sync)
        {
            this.sync = sync;
            return this;
        }

        public AudioData(string audioPath)
        {
            this.audioPath = audioPath;
        }

        public override string ToString()
        {
            return $"[loadType: {loadType}, sync: {sync}, audioPath: {audioPath}]";
        }
    }
}