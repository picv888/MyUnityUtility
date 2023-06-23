using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用预制体的路径写在这个类，使用属性获取预制体，实现懒加载
/// </summary>
public class PrefabManager{
    private static PrefabManager instance;

    public static PrefabManager Instance {
        get {
            if (instance == null) {
                instance = new PrefabManager();
            }
            return instance;
        }
    }
    private PrefabManager() { }

    private GameObject shopGridUIPrefab;
    public GameObject ShopGridUIPrefab{
        get{
            if (shopGridUIPrefab == null) {
                shopGridUIPrefab = Resources.Load<GameObject>("Shop/ShopGrid");
                if (shopGridUIPrefab == null) {
                    Debug.Log("找不到预制体Shop/ShopGrid");
                }
            }
            return shopGridUIPrefab;
        }
    }

    private GameObject shopUIPrefab;
    public GameObject ShopUIPrefab {
        get {
            if (shopUIPrefab == null) {
                shopUIPrefab = Resources.Load<GameObject>("Shop/ShopUI");
                if (shopUIPrefab == null) {
                    Debug.Log("找不到预制体Shop/ShopUI");
                }
            }
            return shopUIPrefab;
        }
    }
}
