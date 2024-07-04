namespace Duo1JFramework
{
    /// <summary>
    /// 层级工具
    /// </summary>
    public static class LayerUtil
    {
        /// <summary>
        /// 开启层级列表
        /// </summary>
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