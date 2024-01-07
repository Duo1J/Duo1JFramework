using Duo1JFramework.Actor;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    [CustomEditor(typeof(ActorController), true)]
    public class ActorControllerEditor : Editor
    {
        private ActorController actorController;

        private SerializedProperty model;
        private SerializedProperty animator;
        private SerializedProperty rigidbody;

        private void OnEnable()
        {
            actorController = (ActorController)target;

            model = serializedObject.FindProperty("model");
            animator = serializedObject.FindProperty("animator");
            rigidbody = serializedObject.FindProperty("rigidBody");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //组件列表
            LU.Vertical(() =>
            {
                GUILayout.Label("组件列表");

                LU.Vertical(() =>
                {
                    LU.SurrondSpace(3, () =>
                    {
                        EditorGUILayout.ObjectField(model, new GUIContent("模型"));
                        EditorGUILayout.ObjectField(animator, new GUIContent("动画控制器"));
                        EditorGUILayout.ObjectField(rigidbody, new GUIContent("刚体"));
                    });
                }, "box");

                if (GUILayout.Button("一键收集组件"))
                {
                    actorController.CollectComponent();
                    EditorUtility.SetDirty(actorController);
                }
            });

            GUILayout.Space(5);

            //状态
            LU.Vertical(() =>
            {
                GUILayout.Label("当前状态信息");
                GUILayout.Space(3);
                string info = actorController.GetHierarchyInfo();
                GUILayout.TextField(info);
            }, "box");

            serializedObject.ApplyModifiedProperties();
        }
    }
}