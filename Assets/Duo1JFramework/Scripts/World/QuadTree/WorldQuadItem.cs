using Duo1JFramework.DataStructure;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 通用世界四叉树管理物体
    /// </summary>
    public class WorldQuadItem : QuadTreeItem
    {
        public override Bounds Bounds
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
                return gizmosBounds.Bounds;
            }
        }
        protected GizmosBounds gizmosBounds;

        public override void TriggerQuad()
        {
            this.SetActive(QuadActive);
        }

        protected virtual void Start()
        {
            WorldQuadManager.Instance.AddItem(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!Game.IsQuit)
            {
                WorldQuadManager.Instance.RemoveItem(this);
            }
        }
    }
}