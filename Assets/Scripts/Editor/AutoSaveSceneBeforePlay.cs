using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// 开始游戏前自动保存场景
/// </summary>
// ensure class initializer is called whenever scripts recompile
[InitializeOnLoad]
public static class AutoSaveSceneBeforePlay {
    // register an event handler when the class is initialized
    static AutoSaveSceneBeforePlay() {
        EditorApplication.playModeStateChanged += ApplicationPlayModeStateChanged;
    }

    private static void ApplicationPlayModeStateChanged(PlayModeStateChange state) {
        //退出编辑模式时(开始游戏前)自动保存场景
        if (state == PlayModeStateChange.ExitingEditMode) {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
    }
}