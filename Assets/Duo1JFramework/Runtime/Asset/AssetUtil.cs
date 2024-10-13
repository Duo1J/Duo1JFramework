using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源工具类
    /// </summary>
    public class AssetUtil
    {
        /// <summary>
        /// 异步加载图集Sprite
        /// </summary>
        public static void LoadAtlasSprite(EAssetLoadType loadType, string atlasPath, string spritePath, Action<Sprite> callback)
        {
            Assert.NotNull(callback, "回调不可为空");

            AssetManager.Instance.LoadByType<SpriteAtlas>(atlasPath, (atlas) =>
            {
                if (atlas == null)
                {
                    Log.ErrorForce($"加载图集失败: `{atlasPath}`");
                    callback(null);
                    return;
                }

                Sprite sprite = atlas.GetSprite(spritePath);

                if (sprite == null)
                {
                    Log.ErrorForce($"加载图集Sprite失败: `{atlasPath}` - `{spritePath}`");
                    callback(null);
                    return;
                }

                callback(sprite);
            }, loadType);
        }

        /// <summary>
        /// 同步加载图集Sprite
        /// </summary>
        public static Sprite LoadAtlasSpriteSync(EAssetLoadType loadType, string atlasPath, string spritePath)
        {
            SpriteAtlas atlas = AssetManager.Instance.LoadByTypeSync<SpriteAtlas>(atlasPath, loadType);

            if (atlas == null)
            {
                Log.ErrorForce($"加载图集失败: `{atlasPath}`");
                return null;
            }

            Sprite sprite = atlas.GetSprite(spritePath);

            if (sprite == null)
            {
                Log.ErrorForce($"加载图集Sprite失败: `{atlasPath}` - `{spritePath}`");
                return null;
            }

            return sprite;
        }

        private AssetUtil()
        {
        }
    }
}
