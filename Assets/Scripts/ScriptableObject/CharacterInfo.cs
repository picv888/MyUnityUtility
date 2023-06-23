using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "New CharacterInfo", menuName = "ScriptableObject/CharacterInfo")]
public class CharacterInfo : ScriptableObject {
    [SerializeField]
    string characterName;
    [SerializeField]
    float maxRunSpeedX;//X轴最大移动速度，标量
    [SerializeField]
    float runAccelerationX;//X轴移动加速度，百分比
    [SerializeField]
    float jumpPower;//跳跃力量
    [SerializeField]
    AudioClip jumpClip;//跳跃声音

    #region 属性
    public float MaxRunSpeedX {
        get {
            return maxRunSpeedX;
        }
        set {
            maxRunSpeedX = value < 0f ? 0f : value;
        }
    }

    public float RunAccelerationX {
        get {
            return runAccelerationX;
        }
        set {
            runAccelerationX = value < 0f ? 0f : value;
        }
    }

    public float JumpPower {
        get {
            return jumpPower;
        }

        set {
            jumpPower = value < 0f ? 0f : value; ;
        }
    }

    public AudioClip JumpClip {
        get {
            return jumpClip;
        }

        set {
            jumpClip = value;
        }
    }

    public string CharacterName {
        get {
            return characterName;
        }

        set {
            characterName = value;
        }
    }
    #endregion
}
