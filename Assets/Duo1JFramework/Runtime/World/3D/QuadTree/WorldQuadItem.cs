using Duo1JFramework.DataStructure;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 基础四叉树控制物体
    /// </summary>
    public class WorldQuadItem : QuadTreeItem
    {
        public override Bounds Bounds => GizmosBounds.Bounds;

        public virtual GizmosBounds GizmosBounds
        {
            get
            {
                if (gizmosBounds == null)
                {
                    gizmosBounds = GetComponent<GizmosBounds>();
                    if (gizmosBounds == null)
                    {
                        Log.ErrorForce($"{ToString()}需要添加组件 `GizmosBounds`");
                        gizmosBounds = gameObject.AddComponent<GizmosBounds>();
                    }
                }
                return gizmosBounds;
            }
        }
        protected GizmosBounds gizmosBounds;

        public override void TriggerQuad()
        {
            this.SetActive(QuadActive);
        }

        private bool addToQT = false;

        /// <summary>
        /// 添加到四叉树管理
        /// </summary>
        protected void AddToQuadTree()
        {
            if (addToQT)
            {
                return;
            }

            addToQT = true;
            WorldQuadManager.Instance.AddItem(this);
        }

        /// <summary>
        /// 更新四叉树管理
        /// </summary>
        public void UpdateQuadTree()
        {
            if (!addToQT)
            {
                AddToQuadTree();
                return;
            }

            WorldQuadManager.Instance.UpdateItem(this);
        }

        protected virtual void Start()
        {
            AddToQuadTree();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!Game.IsQuit && addToQT)
            {
                WorldQuadManager.Instance.RemoveItem(this);
            }
        }
    }
}
