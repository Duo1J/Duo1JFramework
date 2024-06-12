using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 层级管理
    /// </summary>
    public static class Layer
    {
        public const int UI = 5;

        public const int WORLD = 6;

        public static int OnlyLayer(params int[] layerList)
        {
            int ret = 0;
            foreach (int layer in layerList)
            {
                ret |= 1 << layer;
            }
            return ret;
        }
    }
}