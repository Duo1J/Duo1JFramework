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
                    if (node == null)
                    {
                        continue;
                    }

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
                if (itemList == null)
                {
                    itemList = new List<QuadTreeItem>();
                }
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
                if (node == null)
                {
                    continue;
                }

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
        /// 重置对象的评估状态
        /// </summary>
        public void ResetEvaluate()
        {
            SetItemListState(false);

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    childs[i]?.ResetEvaluate();
                }
            }
        }

        /// <summary>
        /// 检测评估
        /// </summary>
        public void Evaluate(object param = null)
        {
            SetItemListState(true);

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    QuadTreeNode node = childs[i];
                    if (node == null)
                    {
                        continue;
                    }

                    if (Tree.EvalLogic(node, param))
                    {
                        node.Evaluate(param);
                    }
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
                    childs[i]?.TriggerEvaluate();
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
            Vector3 min = node.Bounds.min;
            Vector3 max = node.Bounds.max;
            Vector3 pos = item.Pos;
            return pos.x > min.x && pos.x < max.x && pos.z > min.z && pos.z < max.z;
        }

        /// <summary>
        /// 设置对象列表的评估状态
        /// </summary>
        private void SetItemListState(bool active)
        {
            if (itemList == null)
            {
                return;
            }

            foreach (QuadTreeItem item in itemList)
            {
                item.SetState(active);
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
                item.Trigger();
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
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector3 centerOffset = new Vector3(Bounds.size.x / 4 * i, 0, Bounds.size.z / 4 * j);
                    Vector3 childSize = new Vector3(Bounds.size.x / 2, Bounds.size.y, Bounds.size.z / 2);
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
        }

        private bool RemoveFromItemList(QuadTreeItem item)
        {
            if (itemList == null)
            {
                return false;
            }
            return itemList.Remove(item);
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
            else
            {
                Gizmos.color = Color.green;
            }
            Vector3 size = Bounds.size - Vector3.one * 0.1f;
            Gizmos.DrawWireCube(Bounds.center, new Vector3(size.x, 1, size.z));

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