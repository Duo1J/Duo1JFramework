using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能编辑器窗口
    /// </summary>
    public class SkillEditorWindow : BaseEditorWindow<SkillEditorWindow>
    {
        /// <summary>
        /// 当前编辑的技能
        /// </summary>
        [SerializeField]
        private SkillConfig target;

        /// <summary>
        /// 预览GameObject
        /// </summary>
        [SerializeField]
        private GameObject previewObject;

        /// <summary>
        /// 每秒像素
        /// </summary>
        [SerializeField]
        private float pixelPerSec = 200f;

        /// <summary>
        /// 视图起始时间
        /// </summary>
        [SerializeField]
        private float viewStart = 0f;

        /// <summary>
        /// 当前预览时间
        /// </summary>
        [SerializeField]
        private float currentTime;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        [SerializeField]
        private bool playing;

        /// <summary>
        /// 上次编辑器更新时间
        /// </summary>
        private double lastUpdateTime;

        /// <summary>
        /// 是否吸附
        /// </summary>
        [SerializeField]
        private bool snapEnable = true;

        /// <summary>
        /// 吸附值
        /// </summary>
        [SerializeField]
        private float snapValue = 0.05f;

        /// <summary>
        /// 当前选中片段
        /// </summary>
        [NonSerialized]
        private SequenceSegment selectedSeg;

        [NonSerialized]
        private SequenceTrack selectedTrack;

        /// <summary>
        /// 拖拽状态
        /// </summary>
        [NonSerialized]
        private DragState drag;

        /// <summary>
        /// 右侧属性面板滚动
        /// </summary>
        [NonSerialized]
        private Vector2 inspectorScroll;

        /// <summary>
        /// 轨道区滚动
        /// </summary>
        [NonSerialized]
        private Vector2 trackScroll;

        /// <summary>
        /// 预览用SkillContext
        /// </summary>
        [NonSerialized]
        private SkillContext previewCtx;

        /// <summary>
        /// 预览CombatUnitController缓存
        /// </summary>
        [NonSerialized]
        private CombatUnitController previewUnit;

        private const float TOOLBAR_HEIGHT = 26f;
        private const float RULER_HEIGHT = 24f;
        private const float TRACK_HEADER_WIDTH = 140f;
        private const float TRACK_HEIGHT = 40f;

        private float leftPanelWidth = 360f;
        private float rightPanelWidth = 400f;

        /// <summary>
        /// 打开并编辑指定技能
        /// </summary>
        public static void OpenWith(SkillConfig skillConfig)
        {
            SkillEditorWindow win = Open();
            win.target = skillConfig;
            win.minSize = new Vector2(900, 500);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            minSize = new Vector2(900, 500);
            EditorApplication.update += OnEditorUpdate;
            lastUpdateTime = EditorApplication.timeSinceStartup;
        }

        protected override void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
            StopPreview();
            base.OnDisable();
        }

        protected override void SaveData()
        {
            base.SaveData();
            if (target != null)
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }

        private void OnEditorUpdate()
        {
            double now = EditorApplication.timeSinceStartup;
            float dt = (float)(now - lastUpdateTime);
            lastUpdateTime = now;

            if (playing && target != null && target.Sequence != null)
            {
                currentTime += dt;
                if (currentTime >= target.Sequence.Duration)
                {
                    currentTime = target.Sequence.Loop ? 0f : target.Sequence.Duration;
                    if (!target.Sequence.Loop)
                    {
                        playing = false;
                    }
                }
                SamplePreview();
                Repaint();
            }
        }

        private void OnGUI()
        {
            DrawToolbar();
            DrawBody();
            HandleShortcut();
        }

        #region Layout

        private void DrawToolbar()
        {
            ED.Horizontal(() =>
            {
                SkillConfig newSkilConfig = (SkillConfig)EditorGUILayout.ObjectField(target, typeof(SkillConfig), false, GUILayout.Width(180));
                if (newSkilConfig != target)
                {
                    target = newSkilConfig;
                    currentTime = 0f;
                    playing = false;
                }

                if (GUILayout.Button("新建", EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    CreateSkillConfigAsset();
                }

                GUILayout.Space(10);
                GUILayout.Label("预览对象:", GUILayout.Width(58));
                GameObject newGo = (GameObject)EditorGUILayout.ObjectField(previewObject, typeof(GameObject), true, GUILayout.Width(160));
                if (newGo != previewObject)
                {
                    StopPreview();
                    previewObject = newGo;
                }

                GUILayout.Space(15);

                GUILayout.Label("Left");
                leftPanelWidth = GUILayout.HorizontalSlider(leftPanelWidth, 240f, 600f, GUILayout.Width(80));
                GUILayout.Label("Right");
                rightPanelWidth = GUILayout.HorizontalSlider(rightPanelWidth, 300f, 600f, GUILayout.Width(80));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(playing ? "暂停" : "播放", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    playing = !playing;
                    if (playing)
                    {
                        BeginPreview();
                    }
                }

                if (GUILayout.Button("重置", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    currentTime = 0f;
                    StopPreview();
                    Repaint();
                }

                snapEnable = GUILayout.Toggle(snapEnable, "吸附", EditorStyles.toolbarButton, GUILayout.Width(50));

                GUILayout.Label("缩放", GUILayout.Width(50));
                pixelPerSec = GUILayout.HorizontalSlider(pixelPerSec, 40f, 800f, GUILayout.Width(80));

                if (GUILayout.Button("保存", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    if (target != null)
                    {
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        ClearDirty();
                    }
                }
            }, EditorStyles.toolbar, GUILayout.Height(TOOLBAR_HEIGHT));
        }

        private void DrawBody()
        {
            if (target == null)
            {
                ED.HelpBox_Editor("请拖入一个SkillConfig或新建", MessageType.Info);
                return;
            }

            ED.Horizontal(() =>
            {
                DrawLeftPanel();
                DrawCenter();
                DrawRightPanel();
            });
        }

        private void DrawLeftPanel()
        {
            ED.Vertical(() =>
            {
                EditorGUILayout.LabelField("技能属性", EditorStyles.boldLabel);
                target.Id = EditorGUILayout.TextField("Id", target.Id);
                target.DisplayName = EditorGUILayout.TextField("名称", target.DisplayName);
                target.InputId = (EAbilityInputId)EditorGUILayout.EnumPopup("输入槽", target.InputId);
                target.Cooldown = EditorGUILayout.FloatField("冷却", target.Cooldown);
                target.CostMana = EditorGUILayout.FloatField("消耗MP", target.CostMana);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("序列设置", EditorStyles.boldLabel);
                target.Sequence.Duration = Mathf.Max(0.05f, EditorGUILayout.FloatField("总时长", target.Sequence.Duration));
                target.Sequence.Loop = EditorGUILayout.Toggle("循环", target.Sequence.Loop);

                EditorGUILayout.Space();
                if (GUILayout.Button("+ 添加轨道"))
                {
                    AddTrackMenu();
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("效果库", EditorStyles.boldLabel);
                EffectListInspector.Draw(target);

                GUILayout.FlexibleSpace();

            }, "box", GUILayout.Width(leftPanelWidth));
        }

        private void DrawRightPanel()
        {
            ED.Vertical(() =>
            {
                EditorGUILayout.LabelField("片段属性", EditorStyles.boldLabel);

                if (selectedSeg == null)
                {
                    ED.HelpBox_Editor("在时间轴上选择一个片段", MessageType.Info);
                    GUILayout.FlexibleSpace();
                    return;
                }

                ED.Scroll(ref inspectorScroll, () =>
                {
                    SegmentInspector.Draw(selectedSeg, target);
                }, "box");

                EditorGUILayout.Space();

                if (GUILayout.Button("删除片段"))
                {
                    if (selectedTrack != null)
                    {
                        selectedTrack.Segments.Remove(selectedSeg);
                        selectedSeg = null;
                        selectedTrack = null;
                        EditorUtility.SetDirty(target);
                    }
                }

                GUILayout.FlexibleSpace();

            }, "box", GUILayout.Width(rightPanelWidth));
        }

        private void DrawCenter()
        {
            ED.Vertical(() =>
            {
                Rect rulerRect = GUILayoutUtility.GetRect(0, RULER_HEIGHT, GUILayout.ExpandWidth(true));
                DrawRuler(rulerRect);

                ED.Scroll(ref trackScroll, () =>
                {
                    for (int i = 0; i < target.Sequence.Tracks.Count; i++)
                    {
                        SequenceTrack tk = target.Sequence.Tracks[i];
                        Rect r = GUILayoutUtility.GetRect(0, TRACK_HEIGHT, GUILayout.ExpandWidth(true));
                        DrawTrackRow(r, tk, i);
                    }
                });
            });
        }

        #endregion

        #region Draw Ruler / Track

        private void DrawRuler(Rect rect)
        {
            EditorGUI.DrawRect(rect, new Color(0.18f, 0.18f, 0.18f));

            Rect timelineArea = new Rect(rect.x + TRACK_HEADER_WIDTH, rect.y, rect.width - TRACK_HEADER_WIDTH, rect.height);
            float dur = Mathf.Max(0.05f, target.Sequence.Duration);
            float step = ChooseTimeStep();
            ED.HandlesGUI(() =>
            {
                Handles.color = new Color(0.5f, 0.5f, 0.5f);

                for (float t = 0; t <= dur + 1e-3f; t += step)
                {
                    float x = TimeToX(t, timelineArea);
                    Handles.DrawLine(new Vector3(x, timelineArea.y + 4), new Vector3(x, timelineArea.yMax));
                    GUI.Label(new Rect(x + 2, timelineArea.y, 60, 14), $"{t:0.00}", EditorStyles.miniLabel);
                }

                Handles.color = Color.red;
                float cx = TimeToX(currentTime, timelineArea);
                Handles.DrawLine(new Vector3(cx, timelineArea.y), new Vector3(cx, timelineArea.yMax));
            });

            UnityEngine.Event e = UnityEngine.Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && timelineArea.Contains(e.mousePosition))
            {
                currentTime = Mathf.Clamp(XToTime(e.mousePosition.x, timelineArea), 0, target.Sequence.Duration);
                if (playing)
                {
                    BeginPreview();
                }
                SamplePreview();
                e.Use();
                Repaint();
            }
        }

        private void DrawTrackRow(Rect rect, SequenceTrack tk, int idx)
        {
            Rect header = new Rect(rect.x, rect.y, TRACK_HEADER_WIDTH, rect.height);
            Rect content = new Rect(rect.x + TRACK_HEADER_WIDTH, rect.y, rect.width - TRACK_HEADER_WIDTH, rect.height);

            EditorGUI.DrawRect(header, new Color(0.22f, 0.22f, 0.22f));
            EditorGUI.DrawRect(content, idx % 2 == 0 ? new Color(0.27f, 0.27f, 0.27f) : new Color(0.24f, 0.24f, 0.24f));

            Rect nameRect = new Rect(header.x + 4, header.y + 4, header.width - 60, 18);
            tk.Name = EditorGUI.TextField(nameRect, tk.Name);

            Rect typeRect = new Rect(header.x + 4, header.y + 22, header.width - 60, 14);
            EditorGUI.LabelField(typeRect, tk.Type.ToString(), EditorStyles.miniLabel);

            Rect addRect = new Rect(header.xMax - 54, header.y + 6, 24, 20);
            if (GUI.Button(addRect, "+"))
            {
                AddSegmentMenu(tk);
            }
            Rect delRect = new Rect(header.xMax - 28, header.y + 6, 24, 20);
            if (GUI.Button(delRect, "-"))
            {
                target.Sequence.Tracks.Remove(tk);
                if (selectedTrack == tk)
                {
                    selectedSeg = null; selectedTrack = null;
                }
                EditorUtility.SetDirty(target);
                return;
            }

            ED.HandlesGUI(() =>
            {
                Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                float dur = Mathf.Max(0.05f, target.Sequence.Duration);
                float step = ChooseTimeStep();
                for (float t = 0; t <= dur + 1e-3f; t += step)
                {
                    float x = TimeToX(t, content);
                    Handles.DrawLine(new Vector3(x, content.y), new Vector3(x, content.yMax));
                }
            });

            for (int i = 0; i < tk.Segments.Count; i++)
            {
                DrawSegment(content, tk, tk.Segments[i]);
            }

            HandleTrackClick(content, tk);
        }

        private void DrawSegment(Rect trackContent, SequenceTrack tk, SequenceSegment seg)
        {
            float xs = TimeToX(seg.StartTime, trackContent);
            float xe = seg.IsInstant ? xs + 10 : TimeToX(seg.EndTime, trackContent);
            Rect r = new Rect(xs, trackContent.y + 4, Mathf.Max(10, xe - xs), trackContent.height - 8);

            bool sel = selectedSeg == seg;
            Color c = SegmentColor(tk.Type);
            EditorGUI.DrawRect(r, sel ? Color.Lerp(c, Color.white, 0.35f) : c);

            ED.HandlesGUI(() =>
            {
                Handles.color = Color.black;
                Handles.DrawLine(new Vector3(r.xMin, r.yMin), new Vector3(r.xMax, r.yMin));
                Handles.DrawLine(new Vector3(r.xMin, r.yMin), new Vector3(r.xMin, r.yMax));
                Handles.DrawLine(new Vector3(r.xMax, r.yMin), new Vector3(r.xMax, r.yMax));
                Handles.DrawLine(new Vector3(r.xMin, r.yMax), new Vector3(r.xMax, r.yMax));
            });

            GUI.Label(new Rect(r.x + 4, r.y + 2, r.width - 8, 16), seg.Name ?? seg.GetType().Name, EditorStyles.whiteLabel);

            HandleSegmentDrag(r, trackContent, tk, seg);
        }

        private void HandleTrackClick(Rect content, SequenceTrack tk)
        {
            UnityEngine.Event e = UnityEngine.Event.current;
            if (e.type == EventType.ContextClick && content.Contains(e.mousePosition))
            {
                float t = XToTime(e.mousePosition.x, content);
                GenericMenu menu = new GenericMenu();
                IReadOnlyList<Type> types = SegmentTypeRegistry.Get(tk.Type);
                for (int i = 0; i < types.Count; i++)
                {
                    Type ty = types[i];
                    menu.AddItem(new GUIContent("添加/" + SegmentTypeRegistry.GetDisplay(ty)), false, () =>
                    {
                        SequenceSegment s = (SequenceSegment)Activator.CreateInstance(ty);
                        s.Name = SegmentTypeRegistry.GetDisplay(ty);
                        s.StartTime = Snap(t);
                        s.EndTime = Mathf.Min(target.Sequence.Duration, s.StartTime + 0.2f);
                        tk.Segments.Add(s);
                        EditorUtility.SetDirty(target);
                        Repaint();
                    });
                }
                menu.ShowAsContext();
                e.Use();
            }
        }

        private void HandleSegmentDrag(Rect r, Rect content, SequenceTrack tk, SequenceSegment seg)
        {
            UnityEngine.Event e = UnityEngine.Event.current;

            const float EDGE = 6f;
            Rect leftE = new Rect(r.x, r.y, EDGE, r.height);
            Rect rightE = new Rect(r.xMax - EDGE, r.y, EDGE, r.height);
            EditorGUIUtility.AddCursorRect(r, MouseCursor.Pan);
            if (!seg.IsInstant)
            {
                EditorGUIUtility.AddCursorRect(leftE, MouseCursor.ResizeHorizontal);
                EditorGUIUtility.AddCursorRect(rightE, MouseCursor.ResizeHorizontal);
            }

            if (e.type == EventType.MouseDown && e.button == 0 && r.Contains(e.mousePosition))
            {
                selectedSeg = seg;
                selectedTrack = tk;
                drag = new DragState
                {
                    Seg = seg,
                    Track = tk,
                    ContentRect = content,
                    MouseStart = e.mousePosition,
                    StartTime = seg.StartTime,
                    EndTime = seg.EndTime,
                };
                if (!seg.IsInstant && leftE.Contains(e.mousePosition))
                {
                    drag.Mode = DragMode.Left;
                }
                else if (!seg.IsInstant && rightE.Contains(e.mousePosition))
                {
                    drag.Mode = DragMode.Right;
                }
                else
                {
                    drag.Mode = DragMode.Move;
                }
                e.Use();
                Repaint();
            }
            else if (e.type == EventType.MouseDrag && drag.Seg == seg)
            {
                float delta = (e.mousePosition.x - drag.MouseStart.x) / pixelPerSec;
                float dur = target.Sequence.Duration;
                switch (drag.Mode)
                {
                    case DragMode.Move:
                        float span = drag.EndTime - drag.StartTime;
                        float ns = Mathf.Clamp(Snap(drag.StartTime + delta), 0, Mathf.Max(0, dur - span));
                        seg.StartTime = ns;
                        seg.EndTime = ns + span;
                        break;
                    case DragMode.Left:
                        seg.StartTime = Mathf.Clamp(Snap(drag.StartTime + delta), 0, seg.EndTime - 0.02f);
                        break;
                    case DragMode.Right:
                        seg.EndTime = Mathf.Clamp(Snap(drag.EndTime + delta), seg.StartTime + 0.02f, dur);
                        break;
                }
                EditorUtility.SetDirty(target);
                e.Use();
                Repaint();
            }
            else if (e.type == EventType.MouseUp && drag.Seg == seg)
            {
                drag = default;
            }
        }

        #endregion

        #region Menu & Utils

        private void AddTrackMenu()
        {
            GenericMenu menu = new GenericMenu();
            foreach (ESequenceTrackType t in Enum.GetValues(typeof(ESequenceTrackType)))
            {
                ESequenceTrackType captured = t;
                menu.AddItem(new GUIContent(captured.ToString()), false, () =>
                {
                    target.Sequence.Tracks.Add(new SequenceTrack
                    {
                        Name = captured.ToString(),
                        Type = captured
                    });
                    EditorUtility.SetDirty(target);
                });
            }
            menu.ShowAsContext();
        }

        private void AddSegmentMenu(SequenceTrack tk)
        {
            GenericMenu menu = new GenericMenu();
            IReadOnlyList<Type> types = SegmentTypeRegistry.Get(tk.Type);
            for (int i = 0; i < types.Count; i++)
            {
                Type ty = types[i];
                menu.AddItem(new GUIContent(SegmentTypeRegistry.GetDisplay(ty)), false, () =>
                {
                    SequenceSegment s = (SequenceSegment)Activator.CreateInstance(ty);
                    s.Name = SegmentTypeRegistry.GetDisplay(ty);
                    s.StartTime = 0;
                    s.EndTime = Mathf.Min(target.Sequence.Duration, 0.2f);
                    tk.Segments.Add(s);
                    EditorUtility.SetDirty(target);
                });
            }
            menu.ShowAsContext();
        }

        private void HandleShortcut()
        {
            UnityEngine.Event e = UnityEngine.Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && selectedSeg != null && selectedTrack != null)
            {
                selectedTrack.Segments.Remove(selectedSeg);
                selectedSeg = null;
                selectedTrack = null;
                EditorUtility.SetDirty(target);
                Repaint();
                e.Use();
            }
        }

        private float TimeToX(float t, Rect area)
        {
            return area.x + (t - viewStart) * pixelPerSec;
        }

        private float XToTime(float x, Rect area)
        {
            return (x - area.x) / pixelPerSec + viewStart;
        }

        private float Snap(float v)
        {
            if (!snapEnable || snapValue <= 0f)
            {
                return v;
            }
            return Mathf.Round(v / snapValue) * snapValue;
        }

        private float ChooseTimeStep()
        {
            if (pixelPerSec >= 400)
            {
                return 0.1f;
            }
            if (pixelPerSec >= 200)
            {
                return 0.25f;
            }
            if (pixelPerSec >= 100)
            {
                return 0.5f;
            }
            return 1f;
        }

        private static Color SegmentColor(ESequenceTrackType t)
        {
            switch (t)
            {
                case ESequenceTrackType.Animation: return new Color(0.35f, 0.6f, 0.85f);
                case ESequenceTrackType.HitBox: return new Color(0.85f, 0.35f, 0.35f);
                case ESequenceTrackType.EffectApply: return new Color(0.85f, 0.55f, 0.25f);
                case ESequenceTrackType.Vfx: return new Color(0.6f, 0.35f, 0.85f);
                case ESequenceTrackType.Sfx: return new Color(0.3f, 0.7f, 0.4f);
                case ESequenceTrackType.Movement: return new Color(0.4f, 0.75f, 0.75f);
                case ESequenceTrackType.CameraShake: return new Color(0.9f, 0.8f, 0.35f);
                case ESequenceTrackType.Event: return new Color(0.55f, 0.55f, 0.55f);
            }
            return Color.gray;
        }

        private void CreateSkillConfigAsset()
        {
            string path = EditorUtility.SaveFilePanelInProject("新建SkillConfig", "NewSkill", "asset", "选择保存位置");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            SkillConfig skillConfig = ScriptableObject.CreateInstance<SkillConfig>();
            skillConfig.Id = System.IO.Path.GetFileNameWithoutExtension(path);
            skillConfig.DisplayName = skillConfig.Id;
            AssetDatabase.CreateAsset(skillConfig, path);
            AssetDatabase.SaveAssets();
            target = skillConfig;
        }

        #endregion

        #region Preview

        private void BeginPreview()
        {
            if (target == null || previewObject == null)
            {
                previewUnit = null;
                return;
            }
            previewUnit = previewObject.GetComponent<CombatUnitController>();
            previewCtx = new SkillContext
            {
                Owner = previewUnit,
                Ability = null,
                Config = target
            };
            target.Sequence.Reset();
        }

        private void StopPreview()
        {
            if (target != null && target.Sequence != null && previewCtx != null)
            {
                target.Sequence.Interrupt(previewCtx);
            }
            previewCtx = null;
            playing = false;
        }

        private void SamplePreview()
        {
            if (target == null || previewObject == null)
            {
                return;
            }
            if (previewCtx == null)
            {
                BeginPreview();
            }
            target.Sequence.Sample(previewCtx, currentTime);
        }

        #endregion

        private enum DragMode { Move, Left, Right }

        private struct DragState
        {
            public SequenceSegment Seg;
            public SequenceTrack Track;
            public Rect ContentRect;
            public Vector2 MouseStart;
            public float StartTime;
            public float EndTime;
            public DragMode Mode;
        }
    }
}
