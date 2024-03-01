using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 文件夹快速选中工具
    /// </summary>
    public class FolderFastJumpEditor : EditorWindowBase
    {
        private List<FolderFastJumpData> defaultDataList;
        private List<FolderFastJumpData> dataList;

        private Vector2 scrollPos;
        private Color oriColor;

        private void OnGUI()
        {
            oriColor = GUI.color;

            if (dataList == null || defaultDataList == null)
            {
                ReadSoData();
            }

            LU.Scroll(ref scrollPos, () =>
            {
                LU.Vertical(() =>
                {
                    DrawButtonList();

                    GUILayout.Space(10);

                    DrawDefaultDataList();
                    GUILayout.Space(3);
                    DrawCustomDataList();
                });
            });
        }

        private void DrawButtonList()
        {
            LU.Horizontal(() =>
            {
                if (GUILayout.Button("加载数据"))
                {
                    ReadSoData();
                }

                LU.SurrondColor(EditorStyle.NormalBtnColor1, () =>
                {
                    if (GUILayout.Button("保存数据"))
                    {
                        SaveSoData();
                    }
                });

                if (GUILayout.Button("选中配置文件"))
                {
                    ProjectViewUtil.SelectProjectAsset(EditorUtil.GetEditorCfgSOPath<FolderFastJumpDataSo>());
                }
            });

            LU.Horizontal(() =>
            {
                LU.SurrondColor(EditorStyle.NormalBtnColor1, () =>
                {
                    if (GUILayout.Button("添加数据"))
                    {
                        AddData();
                    }

                    if (GUILayout.Button("添加当前选中目标"))
                    {
                        Object target = Selection.activeObject;
                        if (target != null)
                        {
                            FolderFastJumpData data = AddData();
                            string p = AssetDatabase.GetAssetPath(target);
                            p = p.Replace("/", "\\");
                            string[] splited = p.Split('\\');
                            data.name = splited[splited.Length - 1] ?? "";
                            data.path = p;
                        }
                        else
                        {
                            Log.EditorError("当前Project视图未选中目标，无法添加快速选中");
                        }
                    }
                });
            });
        }

        private void DrawDefaultDataList()
        {
            GUILayout.Label("默认列表");

            if (defaultDataList.Count == 0)
            {
                GUILayout.Label("请点击 `加载数据`");
                return;
            }

            bool closed = true;
            float wndWidth = position.width;
            float halfWndWidth = position.width / 2;
            for (int i = 0; i < defaultDataList.Count; i++)
            {
                int iModTwo = i % 2;
                if (iModTwo == 0)
                {
                    GUILayout.BeginHorizontal("box", GUILayout.Width(wndWidth));
                    closed = false;
                }

                LU.Horizontal(() =>
                {
                    FolderFastJumpData data = defaultDataList[i];
                    if (GUILayout.Button("选中", GUILayout.Width(45)))
                    {
                        ProjectViewUtil.SelectProjectAsset(data.path);
                    }

                    GUILayout.Label(data.name);
                }, "box", GUILayout.Width(halfWndWidth));

                if (iModTwo == 1)
                {
                    GUILayout.EndHorizontal();
                    closed = true;
                }
            }

            if (!closed) GUILayout.EndHorizontal();
        }

        private void DrawCustomDataList()
        {
            int removeIdx = -1;

            GUILayout.Label("自定义列表");
            for (int i = 0; i < dataList.Count; i++)
            {
                FolderFastJumpData data = dataList[i];
                LU.Vertical(() =>
                {
                    LU.Horizontal(() =>
                    {
                        GUILayout.Label("备注", GUILayout.Width(35));
                        data.name = GUILayout.TextField(data.name, GUILayout.MaxWidth(120));

                        GUILayout.Label("目标", GUILayout.Width(35));
                        data.path = GUILayout.TextField(data.path, GUILayout.MaxWidth(120));

                        GUILayout.Space(20);

                        bool nullPath = string.IsNullOrEmpty(data.path);
                        LU.SurrondColor(nullPath ? oriColor : Color.cyan, () =>
                        {
                            if (GUILayout.Button("选中", GUILayout.Width(45)))
                            {
                                ProjectViewUtil.SelectProjectAsset(data.path);
                            }
                        });

                        LU.SurrondColor(nullPath ? Color.cyan : oriColor, () =>
                        {
                            if (GUILayout.Button("设置为当前选中", GUILayout.Width(100)))
                            {
                                Object target = Selection.activeObject;
                                if (target != null)
                                {
                                    string p = AssetDatabase.GetAssetPath(target);
                                    p = p.Replace("/", "\\");
                                    string[] splited = p.Split('\\');
                                    data.name = splited[splited.Length - 1] ?? "";
                                    data.path = p;
                                }
                                else
                                {
                                    Log.EditorError("当前Project视图未选中目标，无法设置快速选中");
                                }
                            }
                        });

                        if (GUILayout.Button("删除", GUILayout.Width(45)))
                        {
                            removeIdx = i;
                        }
                    });
                }, "box");
                GUILayout.Space(3);
            }

            if (removeIdx != -1)
            {
                DeleteData(removeIdx);
            }
        }

        private FolderFastJumpData AddData()
        {
            FolderFastJumpData ret = new FolderFastJumpData();
            dataList.Add(ret);

            return ret;
        }

        private void DeleteData(int idx)
        {
            dataList.RemoveAt(idx);
        }

        private void ReadSoData()
        {
            InitDefaultDataList();
            dataList = new List<FolderFastJumpData>();

            if (So.list == null)
            {
                return;
            }

            foreach (FolderFastJumpData data in So.list)
            {
                dataList.Add(data.Clone());
            }
        }

        private void SaveSoData()
        {
            if (dataList == null)
            {
                return;
            }

            try
            {
                EditorUtility.DisplayProgressBar("", "保存文件夹快速选择配置", 0.7f);

                So.list = new List<FolderFastJumpData>();
                foreach (FolderFastJumpData data in dataList)
                {
                    So.list.Add(data.Clone());
                }

                EditorUtility.SetDirty(So);
                AssetDatabase.SaveAssets();
                System.GC.Collect();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private bool CheckChange()
        {
            if (So.list == null || dataList == null)
            {
                return false;
            }

            if (So.list.Count != dataList.Count)
            {
                return true;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                FolderFastJumpData memData = dataList[i];
                FolderFastJumpData soData = So.list[i];
                if ((memData.name != null && soData.name != null && !memData.name.Equals(soData.name)) ||
                    (memData.path != null && soData.path != null && !memData.path.Equals(soData.path)))
                {
                    return true;
                }
            }

            return false;
        }

        public FolderFastJumpDataSo So
        {
            get
            {
                if (so == null)
                {
                    so = EditorUtil.GetOrCreateEditorCfgSO<FolderFastJumpDataSo>();
                }

                return so;
            }
        }

        private FolderFastJumpDataSo so;

        private void InitDefaultDataList()
        {
            defaultDataList = new List<FolderFastJumpData>
            {
                new FolderFastJumpData("DF", "Assets/Duo1JFramework"),
                new FolderFastJumpData("Res", "Assets/Res")
            };
        }

        private void OnEnable()
        {
            ReadSoData();
        }

        private void OnDisable()
        {
            if (CheckChange())
            {
                if (EditorUtility.DisplayDialog("提示", "文件夹快速选中工具数据发现更改，是否保存?", "是", "否"))
                {
                    SaveSoData();
                }
            }
        }
    }
}