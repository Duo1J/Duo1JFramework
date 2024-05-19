using Duo1JFramework.DataStructure;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 通用世界四叉树管理物体
    /// </summary>
    public class WorldQuadItem : QuadTreeItem
    {
        public override Bounds Bounds => GizmosBounds.Bounds;

        public GizmosBounds GizmosBounds
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
        private GizmosBounds gizmosBounds;

        public override void TriggerQuad()
        {
            this.SetActive(QuadActive);
        }

        private bool startAdd = false;

        protected virtual void Start()
        {
            startAdd = true;
            WorldQuadManager.Instance.AddItem(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!Game.IsQuit && startAdd)
            {
                WorldQuadManager.Instance.RemoveItem(this);
            }
        }
    }
}