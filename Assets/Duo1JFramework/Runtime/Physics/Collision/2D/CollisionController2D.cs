using Duo1JFramework.World;
using System;
using UnityEngine;

namespace Duo1JFramework.PhysicsAPI.Physics2D
{
    /// <summary>
    /// 2D碰撞、触发控制器
    /// </summary>
    [DisallowMultipleComponent]
    public class CollisionController2D : WorldItem2D, ICollisionController
    {
        [Label("碰撞体类型")]
        [SerializeField]
        private ECollisionType collisionType = ECollisionType.Trigger;

        /// <summary>
        /// 碰撞体
        /// </summary>
        private Collider2D collision;

        public Action<CollisionController2D, Collision2D> ColliderEnter;
        public Action<CollisionController2D, Collision2D> ColliderStay;
        public Action<CollisionController2D, Collision2D> ColliderExit;

        public Action<CollisionController2D, Collider2D> TriggerEnter;
        public Action<CollisionController2D, Collider2D> TriggerStay;
        public Action<CollisionController2D, Collider2D> TriggerExit;

        /// <summary>
        /// 获取碰撞体
        /// </summary>
        /// <param name="refresh">是否刷新缓存</param>
        public Collider2D GetCollider2D(bool refresh = false)
        {
            if (collision == null || refresh)
            {
                collision = this.GetAndAssertComponent<Collider2D>($"{ToString()}CollisionController2D上必须挂载Collider2D类型组件");
            }
            return collision;
        }

        /// <summary>
        /// 设置碰撞、触发类型
        /// </summary>
        public void SetCollisionType(ECollisionType collisionType)
        {
            this.collisionType = collisionType;
            GetCollider2D().isTrigger = collisionType == ECollisionType.Trigger;
        }

        public void SetEnable(bool enable)
        {
            enabled = enable;
        }

        private void Awake()
        {
            CollisionManager.Instance.AddToDict(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Game.IsQuit)
            {
                return;
            }

            CollisionManager.Instance.RemoveFromDict(this);
        }

        private void OnEnable()
        {
            GetCollider2D().enabled = true;
        }

        private void OnDisable()
        {
            GetCollider2D().enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ColliderEnter?.Invoke(this, collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            ColliderStay?.Invoke(this, collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            ColliderExit?.Invoke(this, collision);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEnter?.Invoke(this, other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TriggerStay?.Invoke(this, other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerExit?.Invoke(this, other);
        }

        public void DrawEditorInfo()
        {
            ED.Vertical(() =>
            {
                GUILayout.Label(ToString());

                Collider2D col = GetCollider2D();
                if (col == null)
                {
                    GUILayout.Label("Collider2D为空");
                    return;
                }

                GUILayout.Label($"Type: {col.GetType().Name}");
                GUILayout.Label($"IsTrigger: {col.isTrigger} {ED.S4} IsEnable: {col.enabled}");
            });
        }
    }
}
