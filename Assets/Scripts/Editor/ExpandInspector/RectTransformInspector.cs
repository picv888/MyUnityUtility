using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 扩展RectTransform的Inspector
/// </summary>
[CustomEditor(typeof(RectTransform))]
public class RectTransformInspector : DecoratorEditor{

    public RectTransformInspector() : base("RectTransformEditor") { }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("把锚框设置到物体边框上")) {
            RectTransform rectTrans = (RectTransform)target;
            SetAnchorAtCorner(rectTrans, false);
        }
        if (GUILayout.Button("把锚框设置到物体边框上（并且递归设置子物体的锚框）")) {
            RectTransform rectTrans = (RectTransform)target;
            SetAnchorAtCorner(rectTrans, true);
        }
    }

    static void SetAnchorAtCorner(RectTransform rectTrans, bool setChild) {
        if (null != rectTrans.parent && rectTrans.parent is RectTransform) {
            Undo.RecordObject(rectTrans, "设置锚框前");//记录Undo
            RectTransform parent = rectTrans.parent as RectTransform;
            Vector3[] childV = new Vector3[4];
            rectTrans.GetWorldCorners(childV);
            Vector3[] parentV = new Vector3[4];
            parent.GetWorldCorners(parentV);
            float reciprocalParentHeight = 1f / (parentV[1].y - parentV[0].y);
            float reciprocalParentWidth = 1f / (parentV[3].x - parentV[0].x);
            rectTrans.anchorMin = Vector2.Scale(new Vector2(reciprocalParentWidth, reciprocalParentHeight), (childV[0] - parentV[0]));
            rectTrans.anchorMax = Vector2.Scale(new Vector2(reciprocalParentWidth, reciprocalParentHeight), (childV[2] - parentV[0]));
            rectTrans.anchoredPosition = new Vector2(0f, 0f);
            rectTrans.offsetMin = new Vector2(0f, 0f);
            rectTrans.offsetMax = new Vector2(0f, 0f);
        }

        if (setChild) {
            for (int i = 0; i < rectTrans.childCount; i++) {
                RectTransform rectTf = rectTrans.GetChild(i) as RectTransform;
                SetAnchorAtCorner(rectTf, true);
            }
        }
    }
}
