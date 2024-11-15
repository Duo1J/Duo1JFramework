using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 体素物体Editor
    /// </summary>
    [CustomEditor(typeof(VoxelItem), true)]
    public class VoxelItemEditor : BaseCustomEditor<VoxelItem>
    {
        private SerializedProperty voxelSize;
        private SerializedProperty mesh;

        protected override void OnEnable()
        {
            base.OnEnable();

            voxelSize = serializedObject.FindProperty("voxelSize");
            mesh = serializedObject.FindProperty("mesh");
        }

        protected override void DrawInspector()
        {
            voxelSize.floatValue = EditorGUILayout.FloatField(new GUIContent("体素大小"), voxelSize.floatValue);
            EditorGUILayout.ObjectField(mesh, new GUIContent("生成Mesh"));

            int voxelCount = 0;
            if (instance.Voxel != null)
            {
                voxelCount = instance.Voxel.VoxelList.Count;
            }

            GUILayout.Space(5);
            GUILayout.Label($"体素数量: {voxelCount}");

            GUILayout.Space(5);
            if (GUILayout.Button("生成体素"))
            {
                instance.GenerateVoxel();
                EditorUtility.SetDirty(instance);
                Log.EditorInfo($"体素生成成功");
            }

            if (GUILayout.Button("清理体素"))
            {
                instance.ClearVoxel();
            }
        }
    }
}
