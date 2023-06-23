using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "CameraFollowInfo", menuName = "ScriptableObject/CameraFollowInfo")]
public class CameraFollowInfo : ScriptableObject {
    [SerializeField, Range(1f, 50f)]
    float distance;//和跟随的目标的Z轴方向距离
    [SerializeField, Range(0f, 1f)]
    float smoothFollowSpeed;//平滑跟随的速度
    [SerializeField]
    Rect freeArea;//自由活动区域，值为相对屏幕的百分比
    [SerializeField]
    Rect smoothArea;//平滑跟随区域，值为相对屏幕的百分比

    public float Distance {
        get {
            return distance;
        }

        set {
            distance = value;
        }
    }

    public float SmoothFollowSpeed {
        get {
            return smoothFollowSpeed;
        }

        set {
            smoothFollowSpeed = value;
        }
    }

    public Rect FreeArea {
        get {
            return freeArea;
        }

        set {
            freeArea = value;
        }
    }

    public Rect SmoothArea {
        get {
            return smoothArea;
        }

        set {
            smoothArea = value;
        }
    }
}
