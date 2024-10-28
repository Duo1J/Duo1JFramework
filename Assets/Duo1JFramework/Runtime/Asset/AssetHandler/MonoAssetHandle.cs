using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// Monobehaviour资源句柄控制
    /// </summary>
    public class MonoAssetHandle : MonoBehaviour
    {
        private IDispose handle;
        private HashSet<IDispose> handleSet;

        public void AddHandle(IDispose _handle)
        {
            if (handleSet == null && handle == null)
            {
                handle = _handle;
                return;
            }

            handle = null;
            handleSet = new HashSet<IDispose>();
            handleSet.Add(_handle);
        }

        private void OnDestroy()
        {
            if (handleSet != null)
            {
                foreach (IDispose _handle in handleSet)
                {
                    _handle.Dispose();
                }

                handleSet = null;
            }

            if (handle != null)
            {
                handle.Dispose();
                handle = null;
            }
        }
    }
}
