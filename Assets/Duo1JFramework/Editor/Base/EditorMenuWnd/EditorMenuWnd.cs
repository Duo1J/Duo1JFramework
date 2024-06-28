using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 左侧菜单右侧面板的编辑器窗口基类
    /// </summary>
    public abstract class EditorMenuWnd : BaseEditorWindow<EditorMenuWnd>
    {
        protected List<EditorMenuSubWnd> subWndList;
        protected int subWndIdx;

        private Vector2 leftScrollPos;
        private Vector2 rightScrollPos;

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
        public T GetSubWnd<T>() where T : EditorMenuSubWnd
        {
            Type t = typeof(T);
            foreach (EditorMenuSubWnd subWnd in subWndList)
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
            float width = position.width;

            LU.Area(new Rect(0, 0, 150, position.height), DrawLeftMenuList, "box");
            width -= 150;

            LU.Area(new Rect(150, 0, width, position.height), DrawRightSubPanel);
        }

        /// <summary>
        /// 绘制左侧菜单列表
        /// </summary>
        private void DrawLeftMenuList()
        {
            LU.Scroll(ref leftScrollPos, () =>
            {
                LU.SurrondColor(subWndList == null, Color.red, () =>
                {
                    if (GUILayout.Button("重新加载数据"))
                    {
                        _ReloadData();
                    }
                });

                if (subWndList != null)
                {
                    LU.SurrondSpace(10, () =>
                    {
                        for (int i = 0; i < subWndList.Count; i++)
                        {
                            LU.SurrondColor(i == subWndIdx, Color.green, () =>
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
            LU.Scroll(ref rightScrollPos, () =>
            {
                if (subWndList == null)
                {
                    GUILayout.Label("左侧菜单数据为空，请重新加载");
                    return;
                }

                EditorMenuSubWnd subWnd = subWndList[subWndIdx];
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
                    GUILayout.Label("请在游戏运行后使用");
                }
            });
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        private void _ReloadData()
        {
            subWndList = new List<EditorMenuSubWnd>();
            InitSubWndList(subWndList);
            for (int i = 0; i < subWndList.Count; i++)
            {
                EditorMenuSubWnd subWnd = subWndList[i];
                subWnd.Idx = i;
                subWnd.Parent = this;
            }

            ReloadData();
        }

        /// <summary>
        /// 初始化添加子面板列表
        /// </summary>
        protected abstract void InitSubWndList(List<EditorMenuSubWnd> subWndList);

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