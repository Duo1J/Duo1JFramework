using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 左侧菜单的右侧子面板
    /// </summary>
    public abstract class LeftMenuSubEditorWnd
    {
        /// <summary>
        /// 子面板索引
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// 父面板
        /// </summary>
        public LeftMenuEditorWnd Parent { get; set; }

        /// <summary>
        /// 数据是否已加载
        /// </summary>
        private bool dataLoaded;

        /// <summary>
        /// 菜单中的名称
        /// </summary>
        public abstract string MenuName { get; }

        /// <summary>
        /// 子面板绘制
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// 重载数据
        /// </summary>
        public abstract void ReloadData();

        /// <summary>
        /// 是否仅运行时可用
        /// </summary>
        public virtual bool PlayingOnly => false;

        /// <summary>
        /// 重载数据
        /// </summary>
        public void _ReloadData()
        {
            if (PlayingOnly && !Application.isPlaying)
            {
                return;
            }

            ReloadData();
            dataLoaded = true;
        }

        /// <summary>
        /// 检查数据是否已加载，否则加载
        /// </summary>
        public void CheckDataLoaded()
        {
            if (!dataLoaded)
            {
                _ReloadData();
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            Parent.SwitchTo(Idx);
        }
    }
}
