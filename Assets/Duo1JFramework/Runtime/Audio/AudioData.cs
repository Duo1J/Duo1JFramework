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
        public string AudioPath { get; private set; }

        /// <summary>
        /// 加载方式
        /// </summary>
        public EAssetLoadType LoadType { get; private set; } = EAssetLoadType.Bundle;

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool Sync { get; private set; } = false;

        public AudioData SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public AudioData SetSync(bool sync)
        {
            Sync = sync;
            return this;
        }

        public AudioData(string audioPath)
        {
            AudioPath = audioPath;
        }

        public override string ToString()
        {
            return $"[loadType: {LoadType}, sync: {Sync}, audioPath: {AudioPath}]";
        }
    }
}
