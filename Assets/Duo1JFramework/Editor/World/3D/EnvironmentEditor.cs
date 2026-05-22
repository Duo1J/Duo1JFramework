using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.World
{
    [CustomEditor(typeof(Environment), true)]
    public class EnvironmentEditor : BaseCustomEditor<Environment>
    {
        private SerializedProperty lightBakedData;
        private SerializedProperty rendererLMParam;

        private SerializedProperty fog;
        private SerializedProperty fogColor;
        private SerializedProperty fogDensity;
        private SerializedProperty skybox;
        private SerializedProperty timeOfDay;

        protected override void OnEnable()
        {
            base.OnEnable();

            lightBakedData = serializedObject.FindProperty("lightBakedData");
            rendererLMParam = serializedObject.FindProperty("rendererLMParam");

            fog = serializedObject.FindProperty("fog");
            fogColor = serializedObject.FindProperty("fogColor");
            fogDensity = serializedObject.FindProperty("fogDensity");
            skybox = serializedObject.FindProperty("skybox");
            timeOfDay = serializedObject.FindProperty("timeOfDay");
        }

        protected override void DrawInspector()
        {
            ED.Vertical(() =>
            {
                EditorGUILayout.PropertyField(lightBakedData, new GUIContent("光照烘焙数据"));
                EditorGUILayout.PropertyField(rendererLMParam, new GUIContent("Renderer光照参数"));

                GUILayout.Space(5);

                GUILayout.Label("环境设置");
                EditorGUILayout.PropertyField(fog, new GUIContent("启用雾效"));
                EditorGUILayout.PropertyField(fogColor, new GUIContent("雾效颜色"));
                EditorGUILayout.PropertyField(fogDensity, new GUIContent("雾效浓度"));
                EditorGUILayout.PropertyField(skybox, new GUIContent("天空盒"));
                EditorGUILayout.PropertyField(timeOfDay, new GUIContent("TOD"));

                GUILayout.Space(5);

                if (GUILayout.Button("填充数据"))
                {
                    instance.FillDataBySetting();
                }
            });
        }
    }
}
