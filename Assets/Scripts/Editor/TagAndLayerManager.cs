using UnityEditor;
using UnityEngine;

/// <summary>
/// 管理Unity中的Tag、SortingLayer、Layer，提供清除、写入功能
/// </summary>
public class TagManagerData {
    /// <summary>
    /// 自定义Tag数据
    /// </summary>
    public static string[] tags = new string[]{
        "Player",
        "Enemy"
    };

    /// <summary>
    /// 自定义SortingLayer数据
    /// </summary>
    public static string[] sortingLayers = new string[]{
        "Player",
        "Background",
        "Foreground",
    };

    /// <summary>
    /// 自定义Layer数据
    /// </summary>
    public static string[] layers = new string[]{
        "Player",
        "Ground",
    };
}


/// <summary>
/// 每次打开工程自动写入Tag数据、SortingLayer数据、Layer数据
/// </summary>
[InitializeOnLoad]
public class AutoWriteSettings {
    static AutoWriteSettings() {
        bool hasKey = PlayerPrefs.HasKey("WriteSettings");
        if (hasKey == false) {
            PlayerPrefs.SetInt("WriteSettings", 1);
            OnWrite();
        }
    }

    /// <summary>
    /// 写入Tag数据、SortingLayer数据、Layer数据
    /// </summary>
    [MenuItem("MyUtility/TagTools/Write")]
    static void OnWrite() {
        foreach (var tag in TagManagerData.tags) {
            AddTag(tag);
        }

        foreach (var sortingLayer in TagManagerData.sortingLayers) {
            AddSortingLayer(sortingLayer);
        }

        foreach (var layer in TagManagerData.layers) {
            AddLayer(layer);
        }
    }


    /// <summary>
    /// 清除Tag数据
    /// </summary>
    [MenuItem("MyUtility/TagTools/ClearTags")]
    static void ClearTags() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "tags") {
                it.ClearArray();
                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }


    /// <summary>
    /// 清除SortingLayer数据
    /// </summary>
    [MenuItem("MyUtility/TagTools/ClearSortingLayers")]
    static void ClearSortingLayers() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "m_SortingLayers") {
                it.ClearArray();
                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }

    /// <summary>
    /// 清除Layer数据
    /// </summary>
    [MenuItem("MyUtility/TagTools/ClearLayers")]
    static void ClearLayers() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "layers") {
                for (int i = 0; i < it.arraySize; i++) {
                    if (i == 3 || i == 6 || i == 7) continue;
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                    dataPoint.stringValue = string.Empty;
                }
                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }

    /// <summary>
    /// 清除Tag数据、SortingLayer数据、Layer数据
    /// </summary>
    [MenuItem("MyUtility/TagTools/ClearAll")]
    static void ClearAll() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            it.ClearArray();
            tagManager.ApplyModifiedProperties();
        }
    }

    static void ReadTag() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "tags") {
                var count = it.arraySize;

                for (int i = 0; i < count; i++) {
                    var dataPoint = it.GetArrayElementAtIndex(i);
                    //Debug.Log(dataPoint.stringValue);
                }
            }
        }
    }

    static void ReadSortingLayer() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "m_SortingLayers") {
                var count = it.arraySize;
                for (int i = 0; i < count; i++) {
                    var dataPoint = it.GetArrayElementAtIndex(i);
                    while (dataPoint.NextVisible(true)) {
                        if (dataPoint.name == "name") {
                            //Debug.Log(dataPoint.stringValue);
                        }
                    }
                }


            }
        }
    }

    static void ReadLayer() {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "layers") {
                for (int i = 0; i < it.arraySize; i++) {
                    if (i == 3 || i == 6 || i == 7) continue;
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                    //Debug.Log(dataPoint.stringValue);
                }
            }
        }
    }


    static void AddTag(string tag) {
        if (!isHasTag(tag)) {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true)) {
                if (it.name == "tags") {
                    it.InsertArrayElementAtIndex(it.arraySize);
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);
                    dataPoint.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                    return;
                }
            }
        }
    }

    static void AddSortingLayer(string sortingLayer) {
        if (!isHasSortingLayer(sortingLayer)) {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true)) {
                if (it.name == "m_SortingLayers") {
                    Debug.Log("SortingLayers" + it.arraySize);
                    it.InsertArrayElementAtIndex(it.arraySize);
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);
                    while (dataPoint.NextVisible(true)) {
                        if (dataPoint.name == "name") {
                            dataPoint.stringValue = sortingLayer;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    static void AddLayer(string layer) {
        if (!isHasLayer(layer)) {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true)) {
                if (it.name == "layers") {
                    for (int i = 0; i < it.arraySize; i++) {
                        if (i == 3 || i == 6 || i == 7) continue;
                        SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(dataPoint.stringValue)) {
                            dataPoint.stringValue = layer;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    static bool isHasTag(string tag) {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++) {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                return true;
        }
        return false;
    }

    static bool isHasSortingLayer(string sortingLayer) {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true)) {
            if (it.name == "m_SortingLayers") {
                for (int i = 0; i < it.arraySize; i++) {
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                    while (dataPoint.NextVisible(true)) {
                        if (dataPoint.name == "name") {
                            if (dataPoint.stringValue == sortingLayer) return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    static bool isHasLayer(string layer) {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++) {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                return true;
        }
        return false;
    }
}