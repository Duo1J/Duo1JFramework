using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.AnimationAPI
{
    /// <summary>
    /// 足部IK曲线生成器
    /// </summary>
    public class FootIKCurveGenerator : BaseEditorWindow<FootIKCurveGenerator>
    {
        /// <summary>
        /// 工具模式
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// 使用Asset中的引用
            /// </summary>
            ByAsset,

            /// <summary>
            /// 使用Hierarchy的实例
            /// </summary>
            ByHierarchy
        }

        private Mode mode = Mode.ByAsset;

        /// <summary>
        /// 采样目标
        /// </summary>
        private GameObject sampleGo;

        /// <summary>
        /// 采样目标Animator
        /// </summary>
        private Animator animator;

        /// <summary>
        /// 采样帧率
        /// </summary>
        private int sampleRate = 30;

        /// <summary>
        /// 脚底距离地面小于此值时视为触地
        /// </summary>
        private float foot2GroundThreshold = 0.005f;

        /// <summary>
        /// 左脚曲线名
        /// </summary>
        private string leftFootIKCurveName = Def.Anim.LEFT_FOOT_IK_CURVE_PARAM;

        /// <summary>
        /// 右脚曲线名
        /// </summary>
        private string rightFootIKCurveName = Def.Anim.RIGHT_FOOT_IK_CURVE_PARAM;

        /// <summary>
        /// 左脚骨骼位置
        /// </summary>
        private Transform leftFootBoneTF;

        /// <summary>
        /// 右脚骨骼位置
        /// </summary>
        private Transform rightFootBoneTF;

        private float leftBottomHeight;
        private float rightBottomHeight;

        /// <summary>
        /// ByAsset模式的资产数
        /// </summary>
        private int assetCnt = 0;

        /// <summary>
        /// ByAsset模式的资产引用列表
        /// </summary>
        private GameObject[] assets;

        /// <summary>
        /// 权重差异阈值
        /// </summary>
        private const float WeightDiffThreshold = 0.01f;

        private Vector2 scrollPos;

        public void OnGUI()
        {
            RichText = true;
            DrawErrMsg();

            ED.Horizontal(() =>
            {
                GUILayout.Label("模式");
                mode = (Mode)EditorGUILayout.EnumPopup(mode);
            });

            GUILayout.Space(5);

            ED.Scroll(ref scrollPos, () =>
            {
                DrawCommonParam();
                GUILayout.Space(5);

                switch (mode)
                {
                    case Mode.ByAsset:
                        DrawMode_ByAsset();
                        break;
                    case Mode.ByHierarchy:
                        DrawMode_ByHierarchy();
                        break;
                }
            });
        }

        /// <summary>
        /// 绘制通用参数
        /// </summary>
        private void DrawCommonParam()
        {
            sampleRate = EditorGUILayout.IntSlider("采样率", sampleRate, 2, 60);
            foot2GroundThreshold = EditorGUILayout.FloatField("脚离地面阈值", foot2GroundThreshold);
            leftFootIKCurveName = EditorGUILayout.TextField("左脚IK曲线名", leftFootIKCurveName);
            rightFootIKCurveName = EditorGUILayout.TextField("右脚IK曲线名", rightFootIKCurveName);
        }

        private void DrawMode_ByAsset()
        {
            ED.Horizontal(() =>
            {
                assetCnt = EditorGUILayout.IntField("资产数目", assetCnt);
                if (GUILayout.Button(assets == null ? "创建" : "设置", GUILayout.Width(60)))
                {
                    if (assetCnt > 0)
                    {
                        assets = new GameObject[assetCnt];
                    }
                }
            });

            if (assets != null)
            {
                for (int i = 0; i < assets.Length; i++)
                {
                    assets[i] = (GameObject)EditorGUILayout.ObjectField(assets[i], typeof(GameObject), false);
                }
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("生成脚部IK动画曲线"))
            {
                if (assets == null || assets.Length == 0)
                {
                    SetErrMsg("资产采样列表为空, 请设置`资产数目`并拖入目标");
                    return;
                }

                if (CheckCommonParam())
                {
                    for (int i = 0; i < assets.Length; i++)
                    {
                        EditorUtil.CreateTempInstance(assets[i], (instance) =>
                        {
                            if (InitSample(instance, true))
                            {
                                BakeAnimationFootIKCurve();
                            }
                        });
                    }
                }
            }
        }

        private void DrawMode_ByHierarchy()
        {
            if (GUILayout.Button("设置采样目标为当前选中"))
            {
                SetErrMsg(null);
                if (EditorUtil.GetActiveGo(out GameObject _sampleGo, false))
                {
                    InitSample(_sampleGo, true);
                    Repaint();
                }
                else
                {
                    SetErrMsg("未在Hierarchy窗口里选中物体");
                }
            }

            if (sampleGo == null)
            {
                ED.HelpBox_Editor("请在Hierarchy窗口选择要生成的物体", MessageType.Info);
                ED.HelpBox_Editor("目标物体或子物体应拥有Animator组件\n组件拥有AnimatorController资产且具备人形骨骼", MessageType.Info);
                ED.HelpBox_Editor("将要自动计算曲线的动画Clip都放到AnimatorController中", MessageType.Info);
                return;
            }

            GUILayout.Label($"采样目标：{sampleGo.name}");

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("生成脚部IK动画曲线"))
            {
                if (CheckCommonParam())
                {
                    BakeAnimationFootIKCurve();
                }
            }
        }

        /// <summary>
        /// 初始化采样对象
        /// </summary>
        private bool InitSample(GameObject _sampleGo, bool searchChild = true)
        {
            if (searchChild)
            {
                animator = _sampleGo.GetComponentInChildren<Animator>();
            }
            else
            {
                animator = _sampleGo.GetComponent<Animator>();
            }

            if (animator == null)
            {
                SetErrMsg($"采样目标`{_sampleGo.name}`错误，没有持有Animator组件");
                ClearData();
                return false;
            }

            sampleGo = animator.gameObject;

            if (animator.runtimeAnimatorController == null)
            {
                SetErrMsg($"采样目标`{sampleGo.name}`错误，没有AnimatorController资产");
                ClearData();
                return false;
            }

            if (animator.avatar == null)
            {
                SetErrMsg($"采样目标`{sampleGo.name}`错误，没有avatar资产");
                ClearData();
                return false;
            }

            leftFootBoneTF = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFootBoneTF = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            leftBottomHeight = animator.leftFeetBottomHeight;
            rightBottomHeight = animator.rightFeetBottomHeight;

            Log.EditorInfo($"设置采样目标为`{sampleGo.name}`");

            return true;
        }

        private void ClearData()
        {
            sampleGo = null;
            animator = null;
            leftFootBoneTF = null;
            rightFootBoneTF = null;
            leftBottomHeight = 0;
            rightBottomHeight = 0;
        }

        /// <summary>
        /// 检查通用参数
        /// </summary>
        private bool CheckCommonParam()
        {
            if (string.IsNullOrEmpty(leftFootIKCurveName))
            {
                SetErrMsg("左脚IK曲线名为空");
                return false;
            }

            if (string.IsNullOrEmpty(rightFootIKCurveName))
            {
                SetErrMsg("右脚IK曲线名为空");
                return false;
            }

            SetErrMsg(null);
            return true;
        }

        private void BakeAnimationFootIKCurve()
        {
            EditorUtil.AnimMode(() =>
            {
                AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
                foreach (AnimationClip clip in clips)
                {
                    string assetPath = AssetDatabase.GetAssetPath(clip);
                    Log.EditorInfo($"生成`{clip.name}`的IK曲线");

                    ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                    if (importer == null)
                    {
                        SetErrMsg($"无法获取到`{clip.name}`的ModelImporter");
                        continue;
                    }

                    ModelImporterClipAnimation[] anims = importer.clipAnimations;
                    int animIndex = Enumerable.Range(0, anims.Length).FirstOrDefault(i => anims[i].name == clip.name);

                    anims[animIndex].curves = anims[animIndex].curves
                        .Where(c =>
                            !leftFootIKCurveName.Equals(c.name) &&
                            !rightFootIKCurveName.Equals(c.name))
                        .ToArray();

                    int frames = Mathf.CeilToInt(clip.length * sampleRate);
                    List<Keyframe> leftFootKeyList = new List<Keyframe>();
                    List<Keyframe> rightFootKeyList = new List<Keyframe>();
                    float lastLeftWeight = 0f;
                    float lastRightWeight = 0f;

                    EditorUtil.AnimSampling(() =>
                    {
                        for (int i = 0; i <= frames; i++)
                        {
                            float time = (float)i / sampleRate;
                            AnimationMode.SampleAnimationClip(sampleGo, clip, time);

                            bool leftGrounded = IsFootGrounded(sampleGo.transform, leftFootBoneTF, leftBottomHeight);
                            bool rightGrounded = IsFootGrounded(sampleGo.transform, rightFootBoneTF, rightBottomHeight);

                            float keyFrameTime = (float)i / frames;
                            float leftCurWeight = leftGrounded ? 1f : 0f;
                            float rightCurWeight = rightGrounded ? 1f : 0f;

                            if (i == 0 || i == frames)
                            {
                                leftFootKeyList.Add(new Keyframe(keyFrameTime, leftCurWeight));
                                rightFootKeyList.Add(new Keyframe(keyFrameTime, rightCurWeight));
                            }
                            else
                            {
                                if (Math.Abs(leftCurWeight - lastLeftWeight) > WeightDiffThreshold)
                                {
                                    leftFootKeyList.Add(new Keyframe((float)(i - 1) / frames, lastLeftWeight));
                                    leftFootKeyList.Add(new Keyframe(keyFrameTime, leftCurWeight));
                                }

                                if (Math.Abs(rightCurWeight - lastRightWeight) > WeightDiffThreshold)
                                {
                                    rightFootKeyList.Add(new Keyframe((float)(i - 1) / frames, lastRightWeight));
                                    rightFootKeyList.Add(new Keyframe(keyFrameTime, rightCurWeight));
                                }
                            }

                            lastLeftWeight = leftCurWeight;
                            lastRightWeight = rightCurWeight;
                        }
                    });

                    ClipAnimationInfoCurve leftInfoCurve = new ClipAnimationInfoCurve();
                    ClipAnimationInfoCurve rightInfoCurve = new ClipAnimationInfoCurve();
                    leftInfoCurve.name = leftFootIKCurveName;
                    rightInfoCurve.name = rightFootIKCurveName;
                    leftInfoCurve.curve = new AnimationCurve(leftFootKeyList.ToArray());
                    rightInfoCurve.curve = new AnimationCurve(rightFootKeyList.ToArray());

                    anims[animIndex].curves = anims[animIndex].curves.Concat(new[] { leftInfoCurve, rightInfoCurve }).ToArray();
                    importer.clipAnimations = anims;
                    importer.SaveAndReimport();
                }

                Log.EditorInfo($"`{sampleGo.name}`的IK动画曲线生成完毕");
            });
        }

        /// <summary>
        /// 脚是否着地
        /// </summary>
        private bool IsFootGrounded(Transform goTf, Transform boneTf, float bottomHeight)
        {
            return (boneTf.position.y - bottomHeight - goTf.position.y) <= foot2GroundThreshold;
        }
    }
}
