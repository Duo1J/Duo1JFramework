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
    public class CollisionController : WorldQuadItem, IEditorDrawer
    {
        [Label("添加到四叉树")]
        public bool addToQuadTree = true;

        public CollisionType collisionType = CollisionType.Trigger;

        private Collider collision;

        public Action<CollisionController, Collision> ColliderEnter;
        public Action<CollisionController, Collision> ColliderStay;
        public Action<CollisionController, Collision> ColliderExit;

        public Action<CollisionController, Collider> TriggerEnter;
        public Action<CollisionController, Collider> TriggerStay;
        public Action<CollisionController, Collider> TriggerExit;

        public Collider GetCollider(bool refresh = false)
        {
            if (collision == null || refresh)
            {
                collision = this.GetAndAssertComponent<Collider>($"CollisionController上必须挂载Collider类型组件, 物体: {gameObject.name} - {gameObject.GetInstanceID()}");
            }
            return collision;
        }

        public void SetCollisionType(CollisionType collisionType)
        {
            this.collisionType = collisionType;
            GetCollider().isTrigger = collisionType == CollisionType.Trigger;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
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
            LU.Vertical(() =>
            {
                GUILayout.Label($"{gameObject.name} - {gameObject.GetInstanceID()}");

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

    public enum CollisionType
    {
        Collider,
        Trigger
    }
}