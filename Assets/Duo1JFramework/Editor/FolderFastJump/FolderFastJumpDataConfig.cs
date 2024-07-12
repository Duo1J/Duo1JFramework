using System;
using System.Collections.Generic;

namespace Duo1JFramework
{
    /// <summary>
    /// 文件夹快速选中工具配置
    /// </summary>
    public class FolderFastJumpDataConfig : EditorConfigSO<FolderFastJumpDataConfig>
    {
        public List<FolderFastJumpData> list;
    }

    [Serializable]
    public class FolderFastJumpData
    {
        public string name = "";
        public string path = "";

        public FolderFastJumpData()
        {
        }

        public FolderFastJumpData(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public FolderFastJumpData Clone()
        {
            return new FolderFastJumpData(name, path);
        }
    }
}