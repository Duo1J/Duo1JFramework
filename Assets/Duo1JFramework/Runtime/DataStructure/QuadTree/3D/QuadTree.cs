using Duo1JFramework.TimerUpdate;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 3D四叉树
    /// </summary>
    public class QuadTree : BaseQuadTree, IQuadTreeNode, IGizmosDrawer
    {
        /// <summary>
        /// 根节点
        /// </summary>
        protected QuadTreeNode Root
        {
            get
            {
                if (_root == null)
                {
                    _root = root as QuadTreeNode;
                }

                return _root;
            }
            set
            {
                root = value;
                _root = value;
            }
        }
        private QuadTreeNode _root;

        /// <summary>
        /// 评估检测算法
        /// </summary>
        public override Func<IQuadTreeNode, object, bool> EvalLogic
        {
            get
            {
                if (evalLogic == null)
                {
                    evalLogic = QuadTreeEvalLogic.EvalByConeOfVisionIgnoreY;
                }
                return evalLogic;
            }
            set => evalLogic = value;
        }

        /// <summary>
        /// 通过包围盒高度控制
        /// </summary>
        public bool EnableHeight { get; private set; } = false;

        /// <summary>
        /// 默认树深度
        /// </summary>
        public const int DEFAULT_DEPTH = 4;

        /// <summary>
        /// 检测评估
        /// </summary>
        public override void Evaluate(object param = null)
        {
            Root.ResetEvaluate();
            Root.Evaluate(param);
            Root.TriggerEvaluate();

#if UNITY_EDITOR
            evalParam = param;
#endif
        }

        public void UnActive()
        {
            Root.ResetEvaluate();
            Root.TriggerEvaluate();

#if UNITY_EDITOR
            evalParam = null;
#endif
        }

        /// <summary>
        /// 重建
        /// </summary>
        public void Rebuild(Bounds bounds, int maxDepth = DEFAULT_DEPTH)
        {
            MaxDepth = maxDepth;

            List<IQuadTreeItem> tempList = itemList;
            itemList = new List<IQuadTreeItem>();

            Root = QuadTreeNode.Create(this, bounds, 0);

            if (tempList != null)
            {
                foreach (IQuadTreeItem item in tempList)
                {
                    AddItem(item);
                }
            }
        }

        /// <summary>
        /// 创建四叉树
        /// </summary>
        public static QuadTree Create(Bounds bounds, int maxDepth = DEFAULT_DEPTH)
        {
            return new QuadTree(bounds, maxDepth);
        }

        private QuadTree(Bounds bounds, int maxDepth = 5)
        {
            if (itemList == null)
            {
                itemList = new List<IQuadTreeItem>();
            }
            else
            {
                itemList.Clear();
            }

            MaxDepth = maxDepth;
            Root = QuadTreeNode.Create(this, bounds, 0);
        }

#if UNITY_EDITOR
        private object evalParam;
#endif

        /// <summary>
        /// 绘制Gizmos信息
        /// </summary>
        public override void DrawGizmos()
        {
            Root.DrawGizmos();

#if UNITY_EDITOR
            if (EvalLogic == QuadTreeEvalLogic.EvalByRectArea && evalParam != null)
            {
                Bounds bounds = evalParam.StructConvert<Bounds>();
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
#endif
        }

        public override void Clear()
        {
            base.Clear();
            Root = null;
        }

        /// <summary>
        /// 通过管理对象包围盒调整树节点包围盒高度
        /// </summary>
        /// <param name="delay">延迟一帧执行</param>
        public void AdjustBoundsHeightByItem(bool delay = false)
        {
            if (!EnableHeight)
            {
                return;
            }

            if (delay)
            {
                UpdateManager.Instance.DelayOneFrame(_AdjustBoundsHeightByItem);
            }
            else
            {
                _AdjustBoundsHeightByItem();
            }
        }

        private void _AdjustBoundsHeightByItem()
        {
            Root.AdjustBoundsHeightByItem();
        }
    }
}
