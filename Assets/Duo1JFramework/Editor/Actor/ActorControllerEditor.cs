using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    [CustomEditor(typeof(ActorController), true)]
    public class ActorControllerEditor : Editor
    {
        private ActorController actorController;

        //组件
        private SerializedProperty model;
        private SerializedProperty animator;
        private SerializedProperty ikCon;
        private SerializedProperty rigidbody;
        private SerializedProperty cc;

        //参数
        private SerializedProperty gravityRate;

        private void OnEnable()
        {
            actorController = (ActorController)target;

            model = serializedObject.FindProperty("model");
            animator = serializedObject.FindProperty("animator");
            ikCon = serializedObject.FindProperty("ikCon");
            rigidbody = serializedObject.FindProperty("rigidBody");
            cc = serializedObject.FindProperty("cc");

            gravityRate = serializedObject.FindProperty("gravityRate");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ActorController actorController = target as ActorController;

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
                        EditorGUILayout.ObjectField(ikCon, new GUIContent("足部IK控制器"));
                        if (rigidbody != null)
                            EditorGUILayout.ObjectField(rigidbody, new GUIContent("刚体"));
                        if (cc != null)
                            EditorGUILayout.ObjectField(cc, new GUIContent("角色控制器"));
                    });
                }, "box");

                if (GUILayout.Button("一键收集组件"))
                {
                    actorController.CollectComponent();
                    EditorUtility.SetDirty(actorController);
                }

                GUILayout.Space(5);
                GUILayout.Label("参数列表");

                LU.Vertical(() =>
                {
                    LU.SurrondSpace(3, () =>
                    {
                        if (gravityRate != null)
                            gravityRate.floatValue = EditorGUILayout.FloatField("重力比率", gravityRate.floatValue);

                        actorController.CameraOffsetX = EditorGUILayout.FloatField("相机X轴偏移", actorController.CameraOffsetX);
                        actorController.CameraOffsetY = EditorGUILayout.FloatField("相机Y轴偏移", actorController.CameraOffsetY);
                        actorController.CameraOffsetZ = EditorGUILayout.FloatField("相机Z轴偏移", actorController.CameraOffsetZ);
                    });
                }, "box");
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