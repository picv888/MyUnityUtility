using UnityEngine;
using System.Collections.Generic;

[System.Serializable, CreateAssetMenu(fileName = "New Database", menuName = "ScriptableObject/Database")]
public class Database : ScriptableObject {
    [SerializeField]
    List<CharacterInfo> characterInfoTable;
    [SerializeField]
    List<MapInfo> mapInfoTable;

    public List<CharacterInfo> CharacterInfoTable {
        get {
            return characterInfoTable;
        }

        set {
            characterInfoTable = value;
        }
    }

    public List<MapInfo> MapInfoTable {
        get {
            return mapInfoTable;
        }

        set {
            mapInfoTable = value;
        }
    }
}
