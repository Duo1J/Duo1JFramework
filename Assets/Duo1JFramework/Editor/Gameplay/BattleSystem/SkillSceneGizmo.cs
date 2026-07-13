using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 在Scene视图绘制当前编辑技能中HitBox形状预览
    /// </summary>
    [InitializeOnLoad]
    public static class SkillSceneGizmo
    {
        static SkillSceneGizmo()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView view)
        {
            if (!EditorWindow.HasOpenInstances<SkillEditorWindow>())
            {
                return;
            }

            SkillEditorWindow[] wins = Resources.FindObjectsOfTypeAll<SkillEditorWindow>();
            if (wins == null || wins.Length == 0)
            {
                return;
            }
            SkillEditorWindow win = wins[0];

            SkillConfig skillConfig = GetCurrentTarget(win);
            GameObject preview = GetPreview(win);
            if (skillConfig == null || preview == null)
            {
                return;
            }

            Transform tf = preview.transform;
            foreach (SequenceTrack tk in skillConfig.Sequence.Tracks)
            {
                if (tk.Type != ESequenceTrackType.HitBox) continue;
                foreach (SequenceSegment seg in tk.Segments)
                {
                    if (seg is HitBoxSegment hb)
                    {
                        DrawHitBox(tf, hb);
                    }
                }
            }
        }

        private static void DrawHitBox(Transform tf, HitBoxSegment hb)
        {
            Vector3 world = tf.TransformPoint(hb.Offset);
            Handles.color = new Color(1f, 0.35f, 0.35f, 0.4f);
            switch (hb.Shape)
            {
                case EHitBoxShape.Sphere:
                    Handles.DrawWireDisc(world, Vector3.up, hb.Size.x);
                    Handles.DrawWireDisc(world, tf.forward, hb.Size.x);
                    Handles.DrawWireDisc(world, tf.right, hb.Size.x);
                    break;
                case EHitBoxShape.Box:
                    ED.HandlesMatrix(Matrix4x4.TRS(world, tf.rotation, Vector3.one), () =>
                    {
                        Handles.DrawWireCube(Vector3.zero, hb.Size);
                    });
                    break;
                case EHitBoxShape.Sector:
                    float half = hb.Size.y * 0.5f;
                    Vector3 from = Quaternion.Euler(0, -half, 0) * tf.forward;
                    Handles.DrawSolidArc(world, Vector3.up, from, hb.Size.y, hb.Size.x);
                    break;
            }
        }

        private static SkillConfig GetCurrentTarget(SkillEditorWindow win)
        {
            System.Reflection.FieldInfo f = typeof(SkillEditorWindow).GetField("target",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return f?.GetValue(win) as SkillConfig;
        }

        private static GameObject GetPreview(SkillEditorWindow win)
        {
            System.Reflection.FieldInfo f = typeof(SkillEditorWindow).GetField("previewObject",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return f?.GetValue(win) as GameObject;
        }
    }
}
