using UnityEngine;

namespace Duo1JFramework
{
    public class HideOnAwake : MonoBehaviour
    {
        public bool hideOnAwake = true;

        private void Awake()
        {
            gameObject.SetActive(!hideOnAwake);
        }
    }
}