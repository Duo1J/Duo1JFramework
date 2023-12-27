using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class ActorController : MonoBehaviour
    {
        [SerializeField]
        private GameObject go;

        [SerializeField]
        private Animator animator;

        #region Public

        #region Transform

        public void MoveByLocalAxis(float h, float v, float speed)
        {
            if (Mathf.Abs(h) <= Def.MIN_AXIS_MOVE &&
                Mathf.Abs(v) <= Def.MIN_AXIS_MOVE)
                return;
            transform.position += new Vector3(h, 0, v) * speed * Time.deltaTime;
        }

        #endregion Transform

        #region Animation

        private Animator Ani()
        {
            if (animator == null) Log.Error($"该角色无Animator组件: {name}");
            return animator;
        }

        public void AniSetTrigger(int id)
        {
            Ani()?.SetTrigger(id);
        }

        public void AniSetTrigger(string name)
        {
            Ani()?.SetTrigger(name);
        }

        public void AniSetBool(int id, bool b)
        {
            Ani()?.SetBool(id, b);
        }

        public void AniSetBool(string name, bool b)
        {
            Ani()?.SetBool(name, b);
        }

        public void AniSetFloat(int id, float val)
        {
            Ani()?.SetFloat(id, val);
        }

        public void AniSetFloat(string name, float val)
        {
            Ani()?.SetFloat(name, val);
        }

        public void Play(string stateName, int layer = -1)
        {
            Ani()?.Play(stateName, layer);
        }

        public void CrossFade(string stateName, float transitionRate, int layer = -1)
        {
            Ani()?.CrossFade(stateName, transitionRate, layer);
        }

        #endregion Animation

        #endregion Public

        #region Lifecycle

        private void Awake()
        {
            if (go == null)
            {
                Log.ErrorForce($"角色对象为空: {name}");
            }
        }

        #endregion Lifecycle
    }
}