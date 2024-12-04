using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.World
{
    [CustomEditor(typeof(Environment), true)]
    public class EnvironmentEditor : BaseCustomEditor<Environment>
    {
        private SerializedProperty lightBakedData;
        private SerializedProperty rendererLMParam;

        protected override void OnEnable()
        {
            base.OnEnable();

            lightBakedData = serializedObject.FindProperty("lightBakedData");
            rendererLMParam = serializedObject.FindProperty("rendererLMParam");
        }

        protected override void DrawInspector()
        {
            ED.Vertical(() =>
            {
                EditorGUILayout.PropertyField(lightBakedData, new GUIContent("光照烘焙数据"));
                EditorGUILayout.PropertyField(rendererLMParam, new GUIContent("Renderer光照参数"));

                GUILayout.Space(5);

                if (GUILayout.Button("填充数据"))
                {
                    instance.FillDataBySetting();
                }
            });
        }
    }
}
