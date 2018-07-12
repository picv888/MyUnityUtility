using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏开始的脚本，在这里用代码控制加载单例的组件，不需要在unity的setting里设置脚本加载顺序了
/// </summary>
public class GameStart : MonoBehaviour {

    private static GameStart instance;
    public static GameStart Instance{
        get {
            return instance;
        }
    }

    private void Awake() {
        instance = this;
        Init();
    }

    GameObject m_gameObject;
    /// <summary>
    /// 加载单例的组件
    /// </summary>
    private void Init(){
        m_gameObject = gameObject;
        m_gameObject.AddComponent<AudioSourceManager>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)){
            AudioSourceManager.Instance.PlayOneShot("music/hit4.wav");
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            AudioSourceManager.Instance.PlayOneShot("music/hit3");
        }

    }
}
