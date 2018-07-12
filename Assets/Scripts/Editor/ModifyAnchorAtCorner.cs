using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 把UI游戏物体的锚框设置到自己的边框上，并且递归设置子物体
/// </summary>
public class ModifyAnchorAtCorner : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    [MenuItem("MyUtility/SetAnchor")]
    private static void SetAnchor() {
        Transform rectTrans = Selection.activeTransform;
        if (!(rectTrans.parent is RectTransform)) {
            return;
        }
        SetAnchorAtCorner(rectTrans as RectTransform, false);
    }

    [MenuItem("MyUtility/SetAllChildAnchor")]
    private static void SetAllChildAnchor() {
        Transform rectTrans = Selection.activeTransform;
        if (!(rectTrans.parent is RectTransform)) {
            return;
        }
        SetAnchorAtCorner(rectTrans as RectTransform, true);
    }

    private static void SetAnchorAtCorner(RectTransform rectTrans, bool setChild) {

        RectTransform parent = rectTrans.parent as RectTransform;
        Vector3[] childV = new Vector3[4];
        rectTrans.GetWorldCorners(childV);
        Vector3[] parentV = new Vector3[4];
        parent.GetWorldCorners(parentV);
        float reciprocalParentHeight = 1f/(parentV[1].y - parentV[0].y);
        float reciprocalParentWidth = 1f/(parentV[3].x - parentV[0].x);
        rectTrans.anchorMin = Vector2.Scale(new Vector2(reciprocalParentWidth, reciprocalParentHeight),(childV[0]-parentV[0]));
        rectTrans.anchorMax = Vector2.Scale(new Vector2(reciprocalParentWidth, reciprocalParentHeight), (childV[2] - parentV[0]));
        rectTrans.anchoredPosition = new Vector2(0f, 0f);
        rectTrans.offsetMin = new Vector2(0f, 0f);
        rectTrans.offsetMax = new Vector2(0f, 0f);
        if (setChild) {
            for (int i = 0; i < rectTrans.childCount; i++) {
                RectTransform rectTf = rectTrans.GetChild(i) as RectTransform;
                SetAnchorAtCorner(rectTf, true);
            }
        }
    }

    private static Vector2 GetAnchor(RectTransform rectTransform){
        Vector2 anchor;
        anchor.x = Mathf.Lerp(rectTransform.anchorMin.x, rectTransform.anchorMax.x, rectTransform.pivot.x);
        anchor.y = Mathf.Lerp(rectTransform.anchorMin.y, rectTransform.anchorMax.y, rectTransform.pivot.y);
        return anchor;
    } 
}
