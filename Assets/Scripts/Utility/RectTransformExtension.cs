using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RectTransformExtension {
    /// <summary>
    /// RectTransform的扩展方法，设置insets
    /// </summary>
    public static void SetInsets(this RectTransform rectTrf, float top, float left, float bottom, float right) {
        if (rectTrf.parent == null) {
            Debug.Log("has no parent");
            return;
        }
        Vector2 parentSize = (rectTrf.parent as RectTransform).rect.size;
        if (top + bottom > parentSize.y) {
            Debug.Log("top + bottom > width of parent!");
            return;
        }
        if (left + right > parentSize.x) {
            Debug.Log("left + right > height of parent!");
            return;
        }
        float width = parentSize.x - left - right;
        float height = parentSize.y - top - bottom;
        rectTrf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, right, width);
        rectTrf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, top, height);
    }

    /// <summary>
    /// 获取物体自己的中心点相对父物体的坐标，不需要知道自己的轴心点
    /// </summary>
    /// <returns>The loacl center position.</returns>
    /// <param name="rectTrans">Rect trans.</param>
    public static Vector2 GetLoaclCenterPosition(this RectTransform rectTrans) {
        if (rectTrans.pivot == new Vector2(0.5f, 0.5f)) {
            return rectTrans.localPosition;
        }
        return (Vector2)rectTrans.localPosition + Vector2.Scale((new Vector2(0.5f, 0.5f) - rectTrans.pivot), rectTrans.rect.size);
    }

    /// <summary>
    /// 把物体自己的中心点设置到相对父物体的指定坐标，不需要知道自己的轴心点
    /// </summary>
    /// <param name="rectTrans">Rect trans.</param>
    /// <param name="v2">指定坐标</param>
    public static void SetLocalCenterPosition(this RectTransform rectTrans, Vector2 v2) {
        //计算物体的localPosition
        rectTrans.localPosition = v2 + Vector2.Scale(rectTrans.pivot - new Vector2(0.5f, 0.5f), rectTrans.rect.size);
    }
}

