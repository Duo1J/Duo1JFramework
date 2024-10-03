using Duo1JFramework.World;
using System;
using UnityEngine;

namespace Duo1JFramework.PhysicsAPI
{
    /// <summary>
    /// 碰撞、触发控制器
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GizmosBounds))]
    public class CollisionController : WorldQuadItem, ICollisionController
    {
        [Label("添加到四叉树")]
        public bool addToQuadTree = true;

        [Label("碰撞体类型")]
        [SerializeField]
        private ECollisionType collisionType = ECollisionType.Trigger;

        /// <summary>
        /// 碰撞体
        /// </summary>
        private Collider collision;

        public Action<CollisionController, Collision> ColliderEnter;
        public Action<CollisionController, Collision> ColliderStay;
        public Action<CollisionController, Collision> ColliderExit;

        public Action<CollisionController, Collider> TriggerEnter;
        public Action<CollisionController, Collider> TriggerStay;
        public Action<CollisionController, Collider> TriggerExit;

        /// <summary>
        /// 获取碰撞体
        /// </summary>
        /// <param name="refresh">是否刷新缓存</param>
        public Collider GetCollider(bool refresh = false)
        {
            if (collision == null || refresh)
            {
                collision = this.GetAndAssertComponent<Collider>($"{ToString()}CollisionController上必须挂载Collider类型组件");
            }
            return collision;
        }

        /// <summary>
        /// 设置碰撞、触发类型
        /// </summary>
        public void SetCollisionType(ECollisionType collisionType)
        {
            this.collisionType = collisionType;
            GetCollider().isTrigger = collisionType == ECollisionType.Trigger;
        }

        public void SetEnable(bool enable)
        {
            enabled = enable;
        }

        protected virtual void Awake()
        {
            CollisionManager.Instance.AddToDict(this);
        }

        protected override void Start()
        {
            if (addToQuadTree)
            {
                AddToQuadTree();
            }
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
            GetCollider().enabled = true;
        }

        private void OnDisable()
        {
            GetCollider().enabled = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ColliderEnter?.Invoke(this, collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            ColliderStay?.Invoke(this, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            ColliderExit?.Invoke(this, collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(this, other);
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerStay?.Invoke(this, other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(this, other);
        }

        public void DrawEditorInfo()
        {
            ED.Vertical(() =>
            {
                GUILayout.Label(ToString());

                Collider col = GetCollider();
                if (col == null)
                {
                    GUILayout.Label("Collider为空");
                    return;
                }

                GUILayout.Label($"IsTrigger: {col.isTrigger}");
            });
        }
    }
}
