using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树节点
    /// </summary>
    public class QuadTreeNode : IQuadTreeNode, IGizmosDrawer
    {
        /// <summary>
        /// 归属四叉树
        /// </summary>
        public QuadTree Tree { get; private set; }

        /// <summary>
        /// 包围盒
        /// </summary>
        public Bounds Bounds { get; private set; }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// 评估激活状态
        /// </summary>
        public bool EvalActive { get; private set; }

        /// <summary>
        /// 子节点列表
        /// </summary>
        private QuadTreeNode[] childs;

        /// <summary>
        /// 管理对象列表
        /// </summary>
        private List<QuadTreeItem> itemList;

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(QuadTreeItem item)
        {
            CheckAndCreateChilds();

            QuadTreeNode tarNode = null;
            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    QuadTreeNode node = childs[i];
                    if (CheckItemInBounds(node, item))
                    {
                        if (tarNode != null)
                        {
                            tarNode = null;
                            break;
                        }
                        tarNode = node;
                    }
                }
            }

            if (tarNode == null)
            {
                AddToItemList(item);
            }
            else
            {
                tarNode.AddItem(item);
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(QuadTreeItem item)
        {
            if (childs == null)
            {
                return RemoveFromItemList(item);
            }

            bool flag = false;
            for (int i = 0; i < childs.Length; ++i)
            {
                QuadTreeNode node = childs[i];
                if (CheckItemInBounds(node, item))
                {
                    flag = node.RemoveItem(item);
                    break;
                }
            }
            if (!flag)
            {
                flag = RemoveFromItemList(item);
            }

            return flag;
        }

        /// <summary>
        /// 通过管理对象包围盒调整自身包围盒高度
        /// </summary>
        /// <param name="delay">延迟一帧执行</param>
        public void AdjustBoundsHeightByItem(bool delay = false)
        {
            //todo hlj
            float tarHeight = 1;
            if (itemList != null)
            {
                foreach (QuadTreeItem item in itemList)
                {
                    Bounds itemBounds = item.Bounds;
                    float itemHeight = itemBounds.center.y + itemBounds.extents.y;
                    if (itemHeight > tarHeight)
                    {
                        tarHeight = itemHeight;
                    }
                }
            }

            Bounds = new Bounds(Bounds.center, new Vector3(Bounds.size.x, tarHeight * 2, Bounds.size.z));
        }

        /// <summary>
        /// 重置对象的评估状态
        /// </summary>
        public void ResetEvaluate()
        {
            SetItemListState(false);

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    childs[i].ResetEvaluate();
                }
            }
        }

        /// <summary>
        /// 检测评估
        /// </summary>
        public void Evaluate(object param = null)
        {
            if (!Tree.EvalLogic(this, param))
            {
                return;
            }

            SetItemListState(true);

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    childs[i].Evaluate(param);
                }
            }
        }

        /// <summary>
        /// 触发检测评估后的状态
        /// </summary>
        public void TriggerEvaluate()
        {
            TriggerItemListState();

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    childs[i].TriggerEvaluate();
                }
            }
        }

        /// <summary>
        /// 创建四叉树节点
        /// </summary>
        public static QuadTreeNode Create(QuadTree tree, Bounds bounds, int depth)
        {
            return new QuadTreeNode(tree, bounds, depth);
        }

        /// <summary>
        /// 检查对象是否被节点包含
        /// </summary>
        private bool CheckItemInBounds(QuadTreeNode node, QuadTreeItem item)
        {
            Vector3 nodeMin = node.Bounds.min;
            Vector3 nodeMax = node.Bounds.max;
            Vector3 itemMin = item.Bounds.min;
            Vector3 itemMax = item.Bounds.max;

            return nodeMin.x < itemMax.x && nodeMin.z < itemMax.z &&
                nodeMax.x > itemMin.x && nodeMax.z > itemMin.z;
        }

        /// <summary>
        /// 设置对象列表的评估状态
        /// </summary>
        private void SetItemListState(bool evalActive)
        {
            EvalActive = evalActive;

            if (itemList == null)
            {
                return;
            }

            foreach (QuadTreeItem item in itemList)
            {
                item.SetQuadState(EvalActive);
            }
        }

        /// <summary>
        /// 触发对象列表的评估状态
        /// </summary>
        private void TriggerItemListState()
        {
            if (itemList == null)
            {
                return;
            }

            foreach (QuadTreeItem item in itemList)
            {
                item.TriggerQuad();
            }
        }

        /// <summary>
        /// 检查并创建子节点
        /// </summary>
        private void CheckAndCreateChilds()
        {
            if (Depth >= Tree.MaxDepth)
            {
                return;
            }
            if (childs != null)
            {
                return;
            }

            childs = new QuadTreeNode[QuadTree.CHILD_COUNT];

            int idx = 0;
            Vector3 size = Bounds.size;
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector3 centerOffset = new Vector3(size.x / 4 * i, 0, size.z / 4 * j);
                    Vector3 childSize = new Vector3(size.x / 2, size.y, size.z / 2);
                    Bounds childBounds = new Bounds(Bounds.center + centerOffset, childSize);
                    childs[idx++] = Create(Tree, childBounds, Depth + 1);
                }
            }
        }

        private void AddToItemList(QuadTreeItem item)
        {
            if (itemList == null)
            {
                itemList = new List<QuadTreeItem>();
            }
            itemList.Add(item);
            AdjustBoundsHeightByItem(true);
        }

        private bool RemoveFromItemList(QuadTreeItem item)
        {
            if (itemList == null)
            {
                return false;
            }
            bool flag = itemList.Remove(item);
            AdjustBoundsHeightByItem(true);
            return flag;
        }

        private QuadTreeNode(QuadTree tree, Bounds bounds, int depth)
        {
            this.Tree = tree;
            this.Bounds = bounds;
            this.Depth = depth;
        }

        /// <summary>
        /// 绘制Gizmos信息
        /// </summary>
        public void DrawGizmos()
        {
            if (itemList != null && itemList.Count != 0)
            {
                Gizmos.color = Color.blue;
            }
            else if (EvalActive)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            Vector3 size = Bounds.size - Vector3.one * 0.1f;
            Gizmos.DrawWireCube(Bounds.center, size);
            Gizmos.DrawWireSphere(Bounds.min, 0.2f);
            Gizmos.DrawWireSphere(Bounds.max, 0.2f);

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    childs[i].DrawGizmos();
                }
            }
        }
    }
}