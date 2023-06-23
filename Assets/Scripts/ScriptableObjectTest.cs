using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScriptableObjectTest : MonoBehaviour {
    public Database db;
    public Dictionary<int, string> dict;
    void Awake() {

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            db = AssetBundleManager.Intance.LoadAssetByAB<Database>("data", "Database");
            MapInfo mapInfo = db.MapInfoTable[0];
            AudioSourceManager.Instance.PlayBGM(mapInfo.Bgm);
            CharacterInfo info = db.CharacterInfoTable[0];
            AudioSourceManager.Instance.PlayOneShot(info.JumpClip);
        }

        if (Input.GetMouseButtonDown(0)) {
            CharacterInfo info = Resources.Load<CharacterInfo>("ScriptableObject/CharacterInfo/PlayerCharacterInfo");
            AudioSourceManager.Instance.PlayOneShot(info.JumpClip);
        }

#if UNITY_EDITOR
        //编辑模式下才执行
        if (!UnityEditor.EditorApplication.isPlaying) {
            Debug.Log("Update " + transform.position.ToDescription());
        }
#endif
    }
}

