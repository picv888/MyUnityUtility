using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 把UI游戏物体的锚框设置到自己的边框上，可以递归设置子物体
/// </summary>
public class ModifyAnchorAtCorner : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    /// <summary>
    /// 设置自己的锚框到自己的边框上
    /// </summary>
    [MenuItem("MyUtility/SetAnchor")]
    private static void SetAnchor() {
        Transform rectTrans = Selection.activeTransform;
        if (!(rectTrans.parent is RectTransform)) {
            return;
        }
        SetAnchorAtCorner(rectTrans as RectTransform, false);
    }

    /// <summary>
    /// 递归遍历所有子物体，包括自己，将锚框设置到边框上
    /// </summary>
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
        Vector2 v = childV[0] - parentV[0];
        rectTrans.anchorMin = new Vector2(v.x / parent.rect.size.x, v.y / parent.rect.size.y);
        rectTrans.anchorMax = new Vector2((childV[3] - parentV[0]).x / parent.rect.size.x, (childV[1] - parentV[0]).y / parent.rect.size.y);
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);
        if (setChild) {
            for (int i = 0; i < rectTrans.childCount; i++) {
                RectTransform rectTf = rectTrans.GetChild(i) as RectTransform;
                SetAnchorAtCorner(rectTf, true);
            }
        }
    }
}
