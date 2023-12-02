using System;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        public void Load<T>(string assetPath, Action<T> callback)
        {

        }

        public T LoadSync<T>(string assetPath)
        {
            return default(T);
        }

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}