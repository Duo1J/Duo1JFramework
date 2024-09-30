using UnityEditor;
using UnityEditor.UI;

namespace Duo1JFramework.UI
{
    [CustomEditor(typeof(ContentSizeFitterExt), true)]
    [CanEditMultipleObjects]
    public class ContentSizeFitterExtEditor : ContentSizeFitterEditor
    {
        private SerializedProperty forceRebuild;
        private SerializedProperty delay;
        private SerializedProperty delayFrame;

        protected override void OnEnable()
        {
            base.OnEnable();
            forceRebuild = serializedObject.FindProperty("forceRebuild");
            delay = serializedObject.FindProperty("delay");
            delayFrame = serializedObject.FindProperty("delayFrame");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(forceRebuild);
            EditorGUILayout.PropertyField(delay);
            EditorGUILayout.PropertyField(delayFrame);

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
