using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneUtility : MonoBehaviour{
    private static SceneUtility instance;

    public static SceneUtility Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    { 
        instance = this;
    }

    public delegate bool DelReturnWouldSwitch(float LoadProgress);
    private DelReturnWouldSwitch onLoading;//参数为加载进度,返回值为是否允许跳转

    public void LoadScene(string sceneName, DelReturnWouldSwitch loadingBackcall)
    {
        onLoading = loadingBackcall;
        StartCoroutine(SyncLoadScene(sceneName));
    }
    private IEnumerator SyncLoadScene(string sceneName)
    {
        AsyncOperation waitForLoad = SceneManager.LoadSceneAsync(sceneName);
        waitForLoad.allowSceneActivation = false;

        do
        {
            bool canSwitch = false;//是否允许切换场景
            if (onLoading != null)
            {
                //有委托则从委托获取能否跳转
                canSwitch = onLoading(waitForLoad.progress);
            }
            else
            {
                //没有委托则直接跳转
                canSwitch = true;
            }
            if (canSwitch)
            {
                waitForLoad.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        } while (true);
    }
}
