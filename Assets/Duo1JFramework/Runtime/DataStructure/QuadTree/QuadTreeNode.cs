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
        private List<IQuadTreeItem> itemList;

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(IQuadTreeItem item)
        {
            CheckAndCreateChilds();

            IQuadTreeNode tarNode = null;
            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    IQuadTreeNode node = childs[i];
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
        public bool RemoveItem(IQuadTreeItem item)
        {
            if (childs == null)
            {
                return RemoveFromItemList(item);
            }

            bool flag = false;
            for (int i = 0; i < childs.Length; ++i)
            {
                IQuadTreeNode node = childs[i];
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
        public bool CheckItemInBounds(IQuadTreeNode node, IQuadTreeItem item)
        {
            return QuadTreeEvalLogic.CheckByBounds(node, item);
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

            foreach (IQuadTreeItem item in itemList)
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

            foreach (IQuadTreeItem item in itemList)
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

        private void AddToItemList(IQuadTreeItem item)
        {
            if (itemList == null)
            {
                itemList = new List<IQuadTreeItem>();
            }
            itemList.Add(item);
            Tree.AdjustBoundsHeightByItem(true);
        }

        private bool RemoveFromItemList(IQuadTreeItem item)
        {
            if (itemList == null)
            {
                return false;
            }
            bool flag = itemList.Remove(item);
            Tree.AdjustBoundsHeightByItem(true);
            return flag;
        }

        private QuadTreeNode(QuadTree tree, Bounds bounds, int depth)
        {
            this.Tree = tree;
            this.Bounds = bounds;
            this.Depth = depth;
        }

        /// <summary>
        /// 通过管理对象包围盒调整树节点包围盒高度
        /// </summary>
        public float AdjustBoundsHeightByItem()
        {
            float childTarHeight = 1;
            float selfTarHeight = 1;

            if (childs != null)
            {
                for (int i = 0; i < childs.Length; ++i)
                {
                    float _childTarHeight = childs[i].AdjustBoundsHeightByItem();
                    if (_childTarHeight > childTarHeight)
                    {
                        childTarHeight = _childTarHeight;
                    }
                }
            }

            if (itemList != null)
            {
                foreach (IQuadTreeItem item in itemList)
                {
                    Bounds itemBounds = item.Bounds;
                    float itemHeight = itemBounds.center.y + itemBounds.extents.y;
                    if (itemHeight > selfTarHeight)
                    {
                        selfTarHeight = itemHeight;
                    }
                }
            }

            float tarHeight = Mathf.Max(childTarHeight, selfTarHeight);
            Bounds = new Bounds(Bounds.center, new Vector3(Bounds.size.x, tarHeight * 2, Bounds.size.z));
            return tarHeight;
        }

        /// <summary>
        /// 绘制Gizmos信息
        /// </summary>
        public void DrawGizmos()
        {
            if (EvalActive)
            {
                Gizmos.color = new Color(0, 1, 0, 0.2f);
            }
            else if (itemList != null && itemList.Count != 0)
            {
                Gizmos.color = new Color(0, 0, 1, 0.4f);
            }
            else
            {
                Gizmos.color = new Color(1, 1, 1, 0.1f);
            }

            Vector3 size = Bounds.size - Vector3.one * 0.1f;
            if (Tree.EnableHeight)
            {
                Gizmos.DrawWireCube(Bounds.center, size);
            }
            else
            {
                Gizmos.DrawWireCube(Bounds.center, new Vector3(size.x, 1, size.z));
            }

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