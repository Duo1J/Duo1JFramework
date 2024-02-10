using System;
using System.Collections.Generic;
using UnityEngine;

public class FolderFastJumpDataSo : ScriptableObject
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