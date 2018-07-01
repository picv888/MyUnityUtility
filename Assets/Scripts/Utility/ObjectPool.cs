using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池，负责生成游戏物体、回收游戏物体到池子、从池子里复用游戏物体
/// </summary>
public class ObjectPool {

    public static ObjectPool _instance;
    public static ObjectPool Instance {
        get {
            if (_instance == null) {
                _instance = new ObjectPool();
            }
            return _instance;
        }
    }

    public Dictionary<string, List<GameObject>> poolDict = new Dictionary<string, List<GameObject>>();

    private ObjectPool() { }

    //生成游戏物体（或者从池子里复用游戏物体）
    public GameObject Instantiate(GameObject go, Vector3 position) {
        GameObject g = Instantiate(go, position, Quaternion.identity);
        return g;
    }
             
    public GameObject Instantiate(GameObject go, Vector3 position, Quaternion quaternion) {
        string key = "Pool-" + go.name;
        GameObject gameObj;
        bool isContainsKey = poolDict.ContainsKey(key);
        if (isContainsKey) {
            List<GameObject> list = poolDict[key];
            if (list.Count > 0) {
                int lastIndex = list.Count - 1;
                gameObj = list[lastIndex];
                gameObj.transform.position = position;
                gameObj.transform.rotation = quaternion;
                gameObj.SetActive(true);
                list.RemoveAt(lastIndex);
            }
            else {
                gameObj = Object.Instantiate<GameObject>(go, position, quaternion);
            }
        }
        else {
            gameObj = Object.Instantiate<GameObject>(go, position, quaternion);
            poolDict.Add(key, new List<GameObject>());
        }
        gameObj.name = key;
        return gameObj;
    }

    //放入池子
    public void Destroy(GameObject go) {
        string key = go.name;
        poolDict[key].Add(go);
        go.SetActive(false);
    }
}
