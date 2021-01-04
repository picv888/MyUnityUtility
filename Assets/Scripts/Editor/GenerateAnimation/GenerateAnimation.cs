using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

/// <summary>
/// 序列帧自动生成animation、animationController、预制体
/// </summary>
public class GenerateAnimation : EditorWindow {

    //生成出的Prefab的路径
    static DefaultAsset prefabDirectory;
    static string prefabPath;
    //生成出的AnimationController的路径
    static DefaultAsset animationControllerDirectory;
    static string animationControllerPath;
    //生成出的Animation的路径
    static DefaultAsset animationDirectory;
    static string animationPath;
    //美术给的原始图片路径
    static DefaultAsset imageDirectory;
    static string imagePath;

    /// <summary>
    /// 临时存储int[]
    /// </summary>
    int[] IntArray = new int[] { 0, 1 };
    //帧率
    static int FrameRate = 30;
    //是否循环
    static bool IsLoop = false;
    //要循环的动作名字
    static string AnimaName = " 需要循环的动作名用\",\"间隔.例如:xxx,xxx,xxx";
    //每秒播放图片的个数
    static int FrameNum = 10;
    //图片类型
    static int ImagesTypeInt = 0;
    static string[] ImagesTypeString = new string[] { "png", "jpg" };
    //是否显示帮助
    static bool IsHelp = false;
    /// <summary>
    /// 创建、显示窗体
    /// </summary>
    public static void Init() {
        GenerateAnimation window = (GenerateAnimation)EditorWindow.GetWindow(typeof(GenerateAnimation), true, "生成动画");
        window.Show();
    }

    /// <summary>
    /// 显示窗体里面的内容
    /// </summary>
    private void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("图片文件夹:");
        if (GUILayout.Button("图片文件夹是哪个？")) {
            IsHelp = !IsHelp;
        }
        if (IsHelp) {
            GUILayout.Label("（项目文件结构：.../图片文件夹/角色文件夹/动作文件夹/序列帧图片.png）");
        }
        GUILayout.EndHorizontal();
        imageDirectory = (DefaultAsset)EditorGUILayout.ObjectField(imageDirectory, typeof(DefaultAsset), false);
        imagePath = AssetDatabase.GetAssetPath(imageDirectory);
        GUILayout.Space(5f);


        GUILayout.Label("生成Prefab到文件夹:");
        prefabDirectory = (DefaultAsset)EditorGUILayout.ObjectField(prefabDirectory, typeof(DefaultAsset), false);
        prefabPath = AssetDatabase.GetAssetPath(prefabDirectory);
        GUILayout.Space(5f);

        GUILayout.Label("生成AnimationController到文件夹:");
        animationControllerDirectory = (DefaultAsset)EditorGUILayout.ObjectField(animationControllerDirectory, typeof(DefaultAsset), false);
        animationControllerPath = AssetDatabase.GetAssetPath(animationControllerDirectory);
        GUILayout.Space(5f);

        GUILayout.Label("生成Animation到文件夹:");
        animationDirectory = (DefaultAsset)EditorGUILayout.ObjectField(animationDirectory, typeof(DefaultAsset), false);
        animationPath = AssetDatabase.GetAssetPath(animationDirectory);
        GUILayout.Space(5f);

        //帧率
        GUILayout.BeginHorizontal();
        GUILayout.Label("动画帧率:");
        FrameRate = EditorGUILayout.IntSlider(FrameRate, 1, 60);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        //每秒播放图片的个数
        GUILayout.BeginHorizontal();
        GUILayout.Label("每秒播放图片的个数:");
        FrameNum = EditorGUILayout.IntSlider(FrameNum, 1, 60);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        //是否循环
        IsLoop = EditorGUILayout.Toggle("动画是否循环", IsLoop);
        if (IsLoop) {//生成出的Prefab的路径
            AnimaName = EditorGUILayout.TextField("循环的动画名字:", AnimaName);
        }

        //图片类型
        ImagesTypeInt = EditorGUILayout.IntPopup("Images Type", ImagesTypeInt, ImagesTypeString, IntArray);

        //开始生成
        if (GUILayout.Button("生成动画")) {
            BuildAniamtions();
        }
    }

    //生成所有动画
    public void BuildAniamtions() {
        if (imageDirectory == null ||
           prefabDirectory == null ||
           animationDirectory == null ||
           animationControllerDirectory == null) {
            Debug.LogError("没有设置文件夹路径");
            return;
        }

        DirectoryInfo raw = new DirectoryInfo(imagePath);
        foreach (DirectoryInfo dictorys in raw.GetDirectories()) {
            GenerateCompleteAnimation(dictorys);
        }

        /*
         //使用选中的文件夹获取序列帧
        Object[] defaultAssetArr = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets);
        foreach (DefaultAsset defaultAsset in defaultAssetArr) {
            if (!System.IO.Directory.Exists(imagePath + "/" + defaultAsset.name)) {
                Debug.LogWarning("选中的文件夹" + defaultAsset.name + "不在Assets/IGamesToolsRaw/BuildAnimation文件夹中,请按照规则选择");
                continue;
            }
            DirectoryInfo dictorys = new DirectoryInfo(imagePath + "/" + defaultAsset.name);
            BuildCompleteAnimation(dictorys);
        }
        */
        Debug.Log(imageDirectory.name + "动画生成完毕");
    }

    //生成角色的所有动画，生成animationController文件、animation文件、角色预制体
    static void GenerateCompleteAnimation(DirectoryInfo directory) {
        List<AnimationClip> clips = new List<AnimationClip>();
        foreach (DirectoryInfo dictoryAnimations in directory.GetDirectories()) {
            //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
            clips.Add(BuildAnimationClip(dictoryAnimations));
        }
        //把所有的动画文件生成在一个AnimationController里
        UnityEditor.Animations.AnimatorController controller = BuildAnimationController(clips, directory.Name);
        //最后生成程序用的Prefab文件
        BuildPrefab(directory, controller);
    }

    //把文件夹下的所有图片生成出一个animation文件
    static AnimationClip BuildAnimationClip(DirectoryInfo dictorys) {
        string animationName = dictorys.Name;
        //查找所有图片，图片后缀为面板选择类型
        FileInfo[] images = dictorys.GetFiles("*." + ImagesTypeString[ImagesTypeInt]);
        AnimationClip clip = new AnimationClip();
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[images.Length];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以在面板界面调节
        float frameTime = 1f / FrameNum;
        for (int i = 0; i < images.Length; i++) {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images[i].FullName));
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = sprite;
        }
        //动画帧率，可在面板界面调节
        clip.frameRate = FrameRate;

        //动画是否循环在面板界面调节
        if (IsLoop) {
            if (IsContainAnimaName(animationName)) {
                SerializedObject serializedClip = new SerializedObject(clip);
                AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
                clipSettings.loopTime = true;
                serializedClip.ApplyModifiedProperties();
            }
        }
        string parentName = System.IO.Directory.GetParent(dictorys.FullName).Name;
        System.IO.Directory.CreateDirectory(animationPath + "/" + parentName);
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, animationPath + "/" + parentName + "/" + animationName + ".anim");
        AssetDatabase.SaveAssets();
        return clip;
    }

    static UnityEditor.Animations.AnimatorController BuildAnimationController(List<AnimationClip> clips, string name) {
        System.IO.Directory.CreateDirectory(animationControllerPath);
        UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(animationControllerPath + "/" + name + ".controller");
        UnityEditor.Animations.AnimatorControllerLayer layer = animatorController.layers[0];
        UnityEditor.Animations.AnimatorStateMachine sm = layer.stateMachine;
        foreach (AnimationClip newClip in clips) {
            UnityEditor.Animations.AnimatorState state = sm.AddState(newClip.name);
            state.motion = newClip;

        }
        AssetDatabase.SaveAssets();
        return animatorController;
    }

    static void BuildPrefab(DirectoryInfo dictorys, UnityEditor.Animations.AnimatorController animatorCountorller) {
        //生成Prefab 添加一张预览用的Sprite
        FileInfo images = dictorys.GetDirectories()[0].GetFiles("*." + ImagesTypeString[ImagesTypeInt])[0];
        GameObject go = new GameObject();
        go.name = dictorys.Name;
        SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
        spriteRender.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images.FullName));
        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = animatorCountorller;
        System.IO.Directory.CreateDirectory(prefabPath);
        PrefabUtility.CreatePrefab(prefabPath + "/" + go.name + ".prefab", go);
        DestroyImmediate(go);
    }


    public static string DataPathToAssetPath(string path) {
        if (Application.platform == RuntimePlatform.WindowsEditor) {
            string pa = path.Substring(path.IndexOf("Assets\\"));
            return pa;
        }

        else {
            string pa = path.Substring(path.IndexOf("Assets/"));
            return pa;
        }

    }

    public static bool IsContainAnimaName(string name) {
        bool isContain = false;
        //未填写指定循环动画则全部设置为循环
        if (AnimaName[0] == ' ' || AnimaName == "") {
            isContain = true;
        }
        else {
            int offset = 0;
            for (int j = 0; j < AnimaName.Length;) {

                int end = AnimaName.IndexOf(",", offset);
                string DataId;
                if (end == -1) {
                    DataId = AnimaName.Substring(offset, AnimaName.Length - offset);
                }
                else {
                    DataId = AnimaName.Substring(offset, end - offset);
                }

                offset = end + 1;

                if (offset == 0) {
                    j = AnimaName.Length;
                }
                if (DataId == name) {
                    isContain = true;
                }
            }
        }

        return isContain;
    }

    class AnimationClipSettings {
        SerializedProperty m_Property;

        private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

        public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

        public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
        public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
        public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
        public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
        public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

        public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
        public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
        public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
        public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
        public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
        public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
        public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
        public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
        public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
        public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
    }

}
