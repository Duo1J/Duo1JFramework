using Duo1JFramework.DataStructure;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 通用世界四叉树管理物体
    /// </summary>
    public class WorldQuadItem : QuadTreeItem
    {
        public override Vector3 Pos => transform.position;

        public override void Trigger()
        {
            this.SetActive(active);
        }

        private void Start()
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