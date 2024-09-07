namespace Duo1JFramework
{
    /// <summary>
    /// 层级工具
    /// </summary>
    public static class LayerUtil
    {
        /// <summary>
        /// 开启层级
        /// </summary>
        public static int OpenLayer(int curLayer, params int[] layerList)
        {
            foreach (int layer in layerList)
            {
                curLayer |= 1 << layer;
            }

            return curLayer;
        }

        /// <summary>
        /// 仅开启层级
        /// </summary>
        public static int OnlyOpenLayer(params int[] layerList)
        {
            return OpenLayer(Def.LayerMask.NONE, layerList);
        }

        /// <summary>
        /// 关闭层级
        /// </summary>
        public static int CloseLayer(int curLayer, params int[] layerList)
        {
            foreach (int layer in layerList)
            {
                curLayer &= ~(1 << layer);
            }

            return curLayer;
        }

        /// <summary>
        /// 仅关闭层级
        /// </summary>
        public static int OnlyCloseLayer(params int[] layerList)
        {
            return CloseLayer(Def.LayerMask.ALL, layerList);
        }
    }
}