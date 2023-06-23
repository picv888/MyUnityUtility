using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

/// <summary>
/// 对动画控制器文件（.controller文件）的Inspector扩展，可以批量修改动画过渡，过渡条件
/// </summary>
[CustomEditor(typeof(UnityEditor.Animations.AnimatorController))]
public class AnimatorControllerEditor : Editor {
    UnityEditor.Animations.AnimatorController animatorController;
    List<AnimatorControllerParameter> listParameter;
    List<string> listStrParameter;
    enum ManagerType {
        TransitionSetting,
        Condition,
    }
    string[] toolBarTitles = new string[] { "动画过渡设置", "过渡条件设置" };
    ManagerType selectedType = ManagerType.TransitionSetting;

    Vector2 scrollPos;
    Dictionary<string, bool> fieldsFoldout = new Dictionary<string, bool>();

    class AnimatorStateTransitionSetting {
        public bool hasExitTime;
        public float exitTime = 1;
        public bool hasFixedDuration;
        public float duration;
        public float offset;
    }
    AnimatorStateTransitionSetting anyAnimatorTransition = new AnimatorStateTransitionSetting();//根据这个批量设置动画过渡
    bool isSetAnyTransition;//是否批量设置动画过渡
    bool isAllSelected;//是否全选
    Dictionary<string, bool> batchesToggleDict = new Dictionary<string, bool>();

    void OnEnable() {
        animatorController = (UnityEditor.Animations.AnimatorController)target;
        listParameter = new List<AnimatorControllerParameter>(animatorController.parameters);
        listStrParameter = new List<string>();
        for (int i = 0; i < listParameter.Count; i++) {
            listStrParameter.Add(listParameter[i].name);
        }
    }

    public override void OnInspectorGUI() {
        if (animatorController != null) {
            //toolBar
            selectedType = (ManagerType)GUILayout.Toolbar((int)selectedType, toolBarTitles);
            if (selectedType == ManagerType.TransitionSetting) {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Has Exit Time:");
                    anyAnimatorTransition.hasExitTime = GUILayout.Toggle(anyAnimatorTransition.hasExitTime, "", GUILayout.ExpandWidth(false));

                    GUILayout.Label("Exit Time:");
                    anyAnimatorTransition.exitTime = EditorGUILayout.FloatField(anyAnimatorTransition.exitTime, GUILayout.ExpandWidth(false));

                    GUILayout.Label("Fixed Duration:");
                    anyAnimatorTransition.hasFixedDuration = GUILayout.Toggle(anyAnimatorTransition.hasFixedDuration, "", GUILayout.ExpandWidth(false));

                    GUILayout.Label("Transition duration:");
                    anyAnimatorTransition.duration = EditorGUILayout.FloatField(anyAnimatorTransition.duration, GUILayout.ExpandWidth(false));

                    GUILayout.Label("Transition offset:");
                    anyAnimatorTransition.offset = EditorGUILayout.FloatField(anyAnimatorTransition.offset, GUILayout.ExpandWidth(false));
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(25f);
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("全选/取消全选")) {
                        isAllSelected = !isAllSelected;
                        List<string> keys = new List<string>();
                        foreach (var item in batchesToggleDict.Keys) {
                            keys.Add(item);
                        }
                        for (int i = 0; i < keys.Count; i++) {
                            string key = keys[i];
                            batchesToggleDict[key] = isAllSelected;
                        }
                    }
                    if (GUILayout.Button("批量设置")) {
                        isSetAnyTransition = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            ShowLayersGUI();
        }

        isSetAnyTransition = false;
    }

    public void ShowLayersGUI() {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            AnimatorControllerLayer[] layers = animatorController.layers;
            for (int i = 0; i < layers.Length; i++) {
                AnimatorControllerLayer layer = layers[i];
                AnimatorStateMachine machine = layer.stateMachine;
                ChildAnimatorState[] states = machine.states;
                AnimatorState[] animStates = new AnimatorState[states.Length];
                for (int j = 0; j < states.Length; j++) {
                    animStates[j] = states[j].state;
                }

                string key = "Layer " + i;
                if (!fieldsFoldout.ContainsKey(key)) {
                    fieldsFoldout[key] = true;
                }
                fieldsFoldout[key] = EditorGUI.Foldout(GUILayoutUtility.GetRect(100, 25), fieldsFoldout[key], "Layer " + i + " : " + layer.name);
                if (fieldsFoldout[key]) {
                    ShowStateGUI(animStates);
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }

    public void ShowStateGUI(AnimatorState[] states) {
        for (int i = 0; i < states.Length; i++) {
            AnimatorState state = states[i];
            AnimatorStateTransition[] transitions = state.transitions;
            string key = "State " + i;
            if (!fieldsFoldout.ContainsKey(key)) {
                fieldsFoldout[key] = true;
            }
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(25f);
                fieldsFoldout[key] = EditorGUILayout.Foldout(fieldsFoldout[key], "State " + i + " : " + state.name);
            }
            GUILayout.EndHorizontal();
            if (fieldsFoldout[key]) {
                switch (selectedType) {
                    case ManagerType.TransitionSetting:
                        ShowTransitionSettingGUI(state, transitions);
                        break;
                    case ManagerType.Condition:
                        ShowTransitionConditionGUI(state, transitions);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 显示过渡设置
    /// </summary>
    /// <param name="state">State.</param>
    /// <param name="transitions">Transitions.</param>
    public void ShowTransitionSettingGUI(AnimatorState state, AnimatorStateTransition[] transitions) {
        GUILayout.BeginVertical();
        {
            for (int i = 0; i < transitions.Length; i++) {
                AnimatorStateTransition transition = transitions[i];
                string batchesSetDictKey = state.name + transition.destinationState.name;
                if (!batchesToggleDict.ContainsKey(batchesSetDictKey)) {
                    batchesToggleDict[batchesSetDictKey] = isAllSelected;
                }
                bool toggle = batchesToggleDict[batchesSetDictKey];
                //批量设置
                if (isSetAnyTransition && toggle) {
                    transition.hasExitTime = anyAnimatorTransition.hasExitTime;
                    transition.exitTime = anyAnimatorTransition.exitTime;
                    transition.hasFixedDuration = anyAnimatorTransition.hasFixedDuration;
                    transition.duration = anyAnimatorTransition.duration;
                    transition.offset = anyAnimatorTransition.offset;
                }

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(25f);
                    //勾选按钮，用于批量设置
                    batchesToggleDict[batchesSetDictKey] = EditorGUILayout.Toggle(batchesToggleDict[batchesSetDictKey]);
                    GUILayout.Space(10f);
                    GUILayout.Label(state.name + "-> " + transition.destinationState.name);
                    GUILayout.Label("Has Exit Time:");
                    transition.hasExitTime = GUILayout.Toggle(transition.hasExitTime, "", GUILayout.ExpandWidth(false));

                    GUILayout.Label("Exit Time:");
                    transition.exitTime = EditorGUILayout.FloatField(transition.exitTime, GUILayout.ExpandWidth(false));

                    GUILayout.Label("Fixed Duration:");
                    transition.hasFixedDuration = GUILayout.Toggle(transition.hasFixedDuration, "", GUILayout.ExpandWidth(false));

                    GUILayout.Label("Transition duration:");
                    transition.duration = EditorGUILayout.FloatField(transition.duration, GUILayout.ExpandWidth(false));

                    GUILayout.Label("Transition offset:");
                    transition.offset = EditorGUILayout.FloatField(transition.offset, GUILayout.ExpandWidth(false));
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    /// <summary>
    /// 显示过渡条件
    /// </summary>
    /// <param name="state">State.</param>
    /// <param name="transitions">Transitions.</param>
    public void ShowTransitionConditionGUI(AnimatorState state, AnimatorStateTransition[] transitions) {

        GUILayout.BeginVertical();
        {
            for (int i = 0; i < transitions.Length; i++) {
                AnimatorStateTransition transition = transitions[i];
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(50f);
                    GUILayout.Label(state.name + "-> " + transition.destinationState.name);
                    AnimatorCondition[] conditions = transition.conditions;
                    for (int j = 0; j < conditions.Length; j++) {
                        AnimatorCondition condition = conditions[j];//结构体值拷贝
                        int selectedParameterIndex = listStrParameter.FindIndex(str => str == condition.parameter);
                        AnimatorControllerParameter parameter = listParameter[selectedParameterIndex];
                        //显示
                        int newSelectedParameterIndex = EditorGUILayout.Popup(selectedParameterIndex, listStrParameter.ToArray());
                        AnimatorConditionMode mode = condition.mode;
                        float threshold = condition.threshold;
                        ModeAndThresholdPopUp(ref mode, ref threshold, parameter.type);
                        if (j < conditions.Length-1) {
                            GUILayout.Label("&&");
                        }
                        GUILayout.FlexibleSpace();//使左对齐
                        //保存
                        if (newSelectedParameterIndex != selectedParameterIndex) {
                            condition.parameter = listStrParameter[newSelectedParameterIndex];
                            AnimatorControllerParameter selectedParameter = listParameter[newSelectedParameterIndex];
                            switch (selectedParameter.type) {
                                case AnimatorControllerParameterType.Bool:
                                    condition.mode = AnimatorConditionMode.If;
                                    condition.threshold = 0f;
                                    break;
                                case AnimatorControllerParameterType.Int:
                                    condition.mode = AnimatorConditionMode.Equals;
                                    condition.threshold = (float)selectedParameter.defaultInt;
                                    break;
                                case AnimatorControllerParameterType.Float:
                                    condition.mode = AnimatorConditionMode.Greater;
                                    condition.threshold = selectedParameter.defaultFloat;
                                    break;
                                case AnimatorControllerParameterType.Trigger:
                                    condition.mode = AnimatorConditionMode.If;
                                    condition.threshold = 0f;
                                    break;
                            }
                            conditions[j] = condition;
                            transition.conditions = conditions;
                        }
                        else if (mode != condition.mode || threshold > condition.threshold || threshold < condition.threshold) {
                            condition.mode = mode;
                            condition.threshold = threshold;
                            conditions[j] = condition;
                            transition.conditions = conditions;
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    void ModeAndThresholdPopUp(ref AnimatorConditionMode mode, ref float threshold,AnimatorControllerParameterType parameterType) {
        string[] displayedOptions = null;
        int[] optionValues = null;
        switch (parameterType) {
            case AnimatorControllerParameterType.Bool:
                displayedOptions = new string[] { "true", "false" };
                optionValues = new int[] { (int)AnimatorConditionMode.If, (int)AnimatorConditionMode.IfNot };
                bool boolMode = EditorGUILayout.Toggle(mode == AnimatorConditionMode.If? true : false, GUILayout.MaxWidth(10f));
                mode = boolMode ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot;
                break;
            case AnimatorControllerParameterType.Int:
                displayedOptions = new string[] { "<", "==", ">", "!=" };
                optionValues = new int[] { (int)AnimatorConditionMode.Less, (int)AnimatorConditionMode.Equals, (int)AnimatorConditionMode.Greater, (int)AnimatorConditionMode.NotEqual };
                int intMode = EditorGUILayout.IntPopup((int)mode, displayedOptions, optionValues);
                int intThreshold = EditorGUILayout.IntField((int)threshold);
                mode = (AnimatorConditionMode)intMode;
                threshold = (int)intThreshold;
                break;
            case AnimatorControllerParameterType.Float:
                displayedOptions = new string[] { "<", ">" };
                optionValues = new int[] { (int)AnimatorConditionMode.Less, (int)AnimatorConditionMode.Greater };
                int floatMode = EditorGUILayout.IntPopup((int)mode, displayedOptions, optionValues);
                mode = (AnimatorConditionMode)floatMode;
                threshold = EditorGUILayout.FloatField(threshold);
                break;
            case AnimatorControllerParameterType.Trigger:
                break;
        }

    }
}