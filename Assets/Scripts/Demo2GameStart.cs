using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2GameStart : MonoBehaviour {
    public static MapInfo currentMapInfo;
    public static CharacterInfo playerInfo;

    // Use this for initialization
    void Awake() {
        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<AudioSourceManager>();
        currentMapInfo = Resources.Load<MapInfo>("ScriptableObject/MapInfo/SheShouCunMapInfo");
        playerInfo = Resources.Load<CharacterInfo>("ScriptableObject/CharacterInfo/PlayerCharacterInfo");

        AudioSourceManager.Instance.PlayBGM(currentMapInfo.Bgm);
     }

    private void Start() {
        Camera.main.GetComponent<SmoothFollowCamera2D>().getMapBounds += CameraGetMapBounds;
    }

    Rect CameraGetMapBounds() {
        Transform minTrans = currentMapInfo.MapPrefab.transform.Find("MapMin");
        Transform maxTrans = currentMapInfo.MapPrefab.transform.Find("MapMax");
        return new Rect(minTrans.position, maxTrans.position - minTrans.position);
    }
}
