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
            Assert.NotNullArg(callback, "callback");

            AssetManager.Instance.LoadByType<SpriteAtlas>(atlasPath, (handle) =>
            {
                if (handle == null || handle.Error())
                {
                    Log.ErrorForce($"加载图集失败: `{atlasPath}`");
                    callback(null);
                    return;
                }

                SpriteAtlas spriteAtlas = handle.Asset;
                Sprite sprite = spriteAtlas.GetSprite(spritePath);

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
            IAssetHandle<SpriteAtlas> handle = AssetManager.Instance.LoadByTypeSync<SpriteAtlas>(atlasPath, loadType);
            if (handle == null || handle.Error())
            {
                Log.ErrorForce($"加载图集失败: `{atlasPath}`");
                return null;
            }

            SpriteAtlas atlas = handle.Asset;
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
