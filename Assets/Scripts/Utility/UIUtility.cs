using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUtility {
    /// <summary>
    /// 判断是否在UI组件上点击，点击阶段为Began，使用的前提是摄像机不能添加Physics Raycaster和Physics 2D Raycaster组件
    /// </summary>static
    /// <returns><c>true</c>, if pointer over user interface was ised, <c>false</c> otherwise.</returns>
    public static bool IsClickOnUI() {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
            if (IsPointerOnUI()) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断鼠标是否在UI组件上，使用的前提是摄像机不能添加Physics Raycaster和Physics 2D Raycaster组件
    /// </summary>
    /// <returns><c>true</c>, if pointer over user interface was ised, <c>false</c> otherwise.</returns>
    public static bool IsPointerOnUI() {
#if IPHONE || ANDROID
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
        if (EventSystem.current.IsPointerOverGameObject())
#endif
            return true;
        else
            return false;
    }
}
