using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 左侧菜单右侧面板的编辑器窗口基类
    /// </summary>
    public abstract class LeftMenuEditorWnd : BaseEditorWindow<LeftMenuEditorWnd>
    {
        private List<LeftMenuSubEditorWnd> subWndList;
        private int subWndIdx;

        private Vector2 leftScrollPos;
        private Vector2 rightScrollPos;

        private const int MinMenuWidth = 150;
        public float MenuWidth { get; protected set; } = MinMenuWidth;

        /// <summary>
        /// 切换菜单到
        /// </summary>
        public void SwitchTo(int subWndIdx)
        {
            if (subWndList == null)
            {
                Log.EditorError("subWndList为空，无法切换");
                return;
            }

            if (subWndIdx >= subWndList.Count)
            {
                Log.EditorError("目标索引超出subWndList的范围，无法切换");
                return;
            }

            this.subWndIdx = subWndIdx;
        }

        /// <summary>
        /// 获取子面板
        /// </summary>
        public T GetSubWnd<T>() where T : LeftMenuSubEditorWnd
        {
            Type t = typeof(T);
            foreach (LeftMenuSubEditorWnd subWnd in subWndList)
            {
                if (subWnd.GetType() == t)
                {
                    return subWnd.Convert<T>();
                }
            }

            Log.EditorError($"{GetType().FullName}未找到类型为{typeof(T).FullName}的子面板");
            return null;
        }

        private void OnGUI()
        {
            ES.SetRichText();

            float width = Width;
            MenuWidth = Mathf.Clamp(MenuWidth, MinMenuWidth, width);

            ED.Area(new Rect(0, 0, MenuWidth, position.height), DrawLeftMenuList, "box");
            width -= MenuWidth;

            ED.Area(new Rect(MenuWidth, 0, width, position.height), DrawRightSubPanel);
        }

        /// <summary>
        /// 绘制左侧菜单列表
        /// </summary>
        private void DrawLeftMenuList()
        {
            ED.Scroll(ref leftScrollPos, () =>
            {
                ED.SurrondColor(subWndList == null, Color.red, () =>
                {
                    if (GUILayout.Button("重新加载数据"))
                    {
                        _ReloadData();
                    }
                });

                if (subWndList != null)
                {
                    ED.SurrondSpace(10, () =>
                    {
                        for (int i = 0; i < subWndList.Count; i++)
                        {
                            ED.SurrondColor(i == subWndIdx, Color.green, () =>
                            {
                                if (GUILayout.Button(subWndList[i].MenuName))
                                {
                                    subWndIdx = i;
                                }

                                GUILayout.Space(2);
                            });
                        }
                    });
                }
            });
        }

        /// <summary>
        /// 绘制右侧子面板
        /// </summary>
        private void DrawRightSubPanel()
        {
            ED.Scroll(ref rightScrollPos, () =>
            {
                if (subWndList == null)
                {
                    ED.HelpBox_Editor("左侧菜单数据为空，请重新加载", MessageType.Error);
                    return;
                }

                LeftMenuSubEditorWnd subWnd = subWndList[subWndIdx];
                if (subWnd == null)
                {
                    subWndIdx = 0;
                    return;
                }


                if (!subWnd.PlayingOnly || Application.isPlaying)
                {
                    if (GUILayout.Button("重新加载面板数据"))
                    {
                        subWnd._ReloadData();
                    }

                    subWnd.CheckDataLoaded();
                    subWnd.Draw();
                }
                else
                {
                    ED.HelpBox_Editor("请在游戏运行后使用", MessageType.Warning);
                }
            });
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void _ReloadData()
        {
            subWndList = new List<LeftMenuSubEditorWnd>();
            InitSubWndList(subWndList);
            for (int i = 0; i < subWndList.Count; i++)
            {
                LeftMenuSubEditorWnd subWnd = subWndList[i];
                subWnd.Idx = i;
                subWnd.Parent = this;
            }

            ReloadData();
        }

        /// <summary>
        /// 初始化添加子面板列表
        /// </summary>
        protected abstract void InitSubWndList(List<LeftMenuSubEditorWnd> subWndList);

        /// <summary>
        /// 重新加载数据
        /// </summary>
        protected virtual void ReloadData()
        {
        }

        protected override void LoadData()
        {
            base.LoadData();
            _ReloadData();
        }
    }
}
