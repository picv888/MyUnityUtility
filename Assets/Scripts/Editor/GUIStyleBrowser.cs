using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// GUIStyle浏览器，查看所有的GUIStyle.
/// </summary>
public class GUIStyleBrowser : EditorWindow {
    static List<GUIStyle> styles = new List<GUIStyle>();
    static List<PropertyInfo> properties;

    [MenuItem("MyUtility/GUIStyleBrowser")]
    public static void Test() {
        EditorWindow.GetWindow<GUIStyleBrowser>(true, "GUIStyle浏览器");
        properties = new List<PropertyInfo>(typeof(EditorStyles).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
        properties.Sort((proA, proB) => string.Compare(proA.Name, proB.Name));

        List<PropertyInfo> newProperties = new List<PropertyInfo>();
        for (int i = 0; i < properties.Count; i++) {
            object obj = properties[i].GetValue(null, null);
            if (obj is GUIStyle) {
                styles.Add(obj as GUIStyle);
                newProperties.Add(properties[i]);
            }
        }
        properties = newProperties;
    }

    public Vector2 scrollPosition = Vector2.zero;
    void OnGUI() {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < styles.Count; i++) {
            GUILayout.BeginHorizontal();
            GUILayout.TextField(i + ". EditorStyles." + properties[i].Name);
            GUILayout.Label("abcd1234", styles[i]);
            GUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }
        GUILayout.EndScrollView();
    }
}