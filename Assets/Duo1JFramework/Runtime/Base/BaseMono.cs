using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 基础MonoBehaviour
    /// </summary>
    public abstract class BaseMono : MonoBehaviour
    {
        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        public override string ToString()
        {
            return $"<{name}: {GetInstanceID()}>";
        }
    }
}