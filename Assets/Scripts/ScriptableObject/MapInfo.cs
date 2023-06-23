using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "New MapInfo", menuName = "ScriptableObject/MapInfo")]
public class MapInfo : ScriptableObject {
    [SerializeField]
    string mapName;//地图名字
    [SerializeField]
    Vector2 size;//地图大小
    [SerializeField]
    List<MonsterBornPos> monsterBorns;//怪物生成点
    [SerializeField]
    AudioClip bgm;//地图BGM
    [SerializeField]
    GameObject mapPrefab;//地图预制体

    #region 属性
    public string MapName {
        get {
            return mapName;
        }

        set {
            mapName = value;
        }
    }

    public Vector2 Size {
        get {
            return size;
        }

        set {
            size = value;
        }
    }

    public AudioClip Bgm {
        get {
            return bgm;
        }

        set {
            bgm = value;
        }
    }

    public List<MonsterBornPos> MonsterBorns {
        get {
            return monsterBorns;
        }

        set {
            monsterBorns = value;
        }
    }

    public GameObject MapPrefab {
        get {
            return mapPrefab;
        }

        set {
            mapPrefab = value;
        }
    }
    #endregion
}

/// <summary>
/// 传送门
/// </summary>
[System.Serializable]
public class DoorPos {
    [SerializeField]
    Vector2 position;
    [SerializeField]
    MapInfo nextMapInfo;

    public Vector2 Position {
        get {
            return position;
        }

        set {
            position = value;
        }
    }

    public MapInfo NextMapInfo {
        get {
            return nextMapInfo;
        }

        set {
            nextMapInfo = value;
        }
    }
}

/// <summary>
/// 怪物生成点
/// </summary>
[System.Serializable]
public class MonsterBornPos {
    [SerializeField]
    Vector2 position;
    [SerializeField]
    CharacterInfo monsterInfo;

    public Vector2 Position {
        get {
            return position;
        }

        set {
            position = value;
        }
    }

    public CharacterInfo MonsterInfo {
        get {
            return monsterInfo;
        }

        set {
            monsterInfo = value;
        }
    }
}
