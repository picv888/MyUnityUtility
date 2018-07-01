using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUtility {

    private static UIUtility instance;
    public static UIUtility Instance {
        get {
            if (null == instance) {
                instance = new UIUtility();
            }
            return instance;
        }
    }

    private UIUtility() { }

    /// <summary>
    /// 判断是否在UI组件上点击,点击阶段为Began
    /// </summary>
    /// <returns><c>true</c>, if pointer over user interface was ised, <c>false</c> otherwise.</returns>
    public bool IsClickOnUI() {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
            if (IsPointerOnUI()) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断鼠标是否在UI组件上
    /// </summary>
    /// <returns><c>true</c>, if pointer over user interface was ised, <c>false</c> otherwise.</returns>
    public bool IsPointerOnUI() {
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
