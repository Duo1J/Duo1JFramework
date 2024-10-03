namespace Duo1JFramework
{
    public class HideOnAwake : BaseMono
    {
        public bool hideOnAwake = true;

        private void Awake()
        {
            gameObject.SetActive(!hideOnAwake);
        }
    }
}