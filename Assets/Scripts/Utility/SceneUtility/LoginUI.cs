using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour {
    Button startGameBtn;
    Slider slider;
    GameObject progressView;
	// Use this for initialization
	void Start () {
        startGameBtn = transform.Find("StartScreen/Menu/Button1").GetComponent<Button>();
        slider = transform.Find("bg/Slider").GetComponent<Slider>();
        progressView = transform.Find("bg").gameObject;
        startGameBtn.onClick.AddListener(StartGameBtnClick);
    }
	
	public void StartGameBtnClick()
    {
        progressView.SetActive(true);
        SceneUtility.Instance.LoadScene("Forest_01", BoolCanTrans);
    }

    private float realProgress;
    public bool BoolCanTrans(float progress)
    {
        realProgress = progress;
        if (slider.value >= 1.0f)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (slider.value >= 0.89)
        {
            slider.value = Mathf.MoveTowards(slider.value, 1.0f, 0.1f/2*Time.deltaTime);
        }
        else
        {
            slider.value = Mathf.Lerp(slider.value, realProgress, 0.1f);
        }
        
    }
}
