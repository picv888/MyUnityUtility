using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机平滑跟随，只可用于2D游戏的正交摄像机，在自由活动区域内不跟随，在平滑跟随区域内平滑跟随，在平滑跟随区域外面紧贴跟随，并且限制摄像范围在地图内
/// </summary>
public class SmoothFollowCamera2D : MonoBehaviour {
    Camera m_camera;
    public Transform followTargetTransform;
    Vector3 targetPosition;
    Vector2 followTargetViewPortPoint;//跟随目标相对屏幕的坐标（百分比）
    public CameraFollowInfo followInfo;
    public event System.Func<Rect> getMapBounds;//获取地图范围的委托
    Rect worldSmoothRect;
    Rect worldScreenArea;

    void Awake() {
        followInfo = Resources.Load<CameraFollowInfo>("ScriptableObject/CameraFollowInfo");
        m_camera = GetComponent<Camera>();
        worldSmoothRect = ViewPortToWorldRect(followInfo.SmoothArea);
        worldScreenArea = ViewPortToWorldRect(new Rect(0, 0, 1, 1));
    }

    void LateUpdate() {
        if (followTargetTransform == null) {
            return;
        }

        targetPosition = followTargetTransform.position;
        targetPosition.z -= followInfo.Distance;
        AdjustDistance();
        SmoothFollow();
        CloseFollow();
        ClampInMap();
    }

    Rect ViewPortToWorldRect(Rect viewPortRect) {
        Rect worldRect = new Rect(m_camera.ViewportToWorldPoint(viewPortRect.min), m_camera.ViewportToWorldPoint(viewPortRect.max) - m_camera.ViewportToWorldPoint(viewPortRect.min));
        return worldRect;
    }

    //调整摄像机和跟随目标的Z轴距离
    void AdjustDistance() {
        Vector3 pos = transform.position;
        pos.z = followTargetTransform.position.z - followInfo.Distance;
        transform.position = pos;
    }

    //目标在自由活动区域外面则自动跟随
    void SmoothFollow() {
        followTargetViewPortPoint = m_camera.WorldToViewportPoint(followTargetTransform.position);
        if (followTargetViewPortPoint.x < followInfo.FreeArea.xMin ||
            followTargetViewPortPoint.x > followInfo.FreeArea.xMax ||
            followTargetViewPortPoint.y < followInfo.FreeArea.yMin ||
            followTargetViewPortPoint.y > followInfo.FreeArea.yMax) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followInfo.SmoothFollowSpeed * Time.deltaTime);
        }
    }

    //目标在平滑跟随区域外面则紧贴跟随
    void CloseFollow() {
        followTargetViewPortPoint = m_camera.WorldToViewportPoint(followTargetTransform.position);
        Vector3 position = transform.position;
        if (followTargetViewPortPoint.x < followInfo.SmoothArea.xMin) {
            position.x = targetPosition.x + worldSmoothRect.width / 2f;
        }
        else if (followTargetViewPortPoint.x > followInfo.SmoothArea.xMax) {
            position.x = targetPosition.x - worldSmoothRect.width / 2f;
        }

        if (followTargetViewPortPoint.y < followInfo.SmoothArea.yMin) {
            position.y = targetPosition.y + worldSmoothRect.height / 2f;
        }
        else if (followTargetViewPortPoint.y > followInfo.SmoothArea.yMax) {
            position.y = targetPosition.y - worldSmoothRect.height / 2f;
        }
        transform.position = position;
    }

    //限制摄像范围在地图内
    void ClampInMap() {
        if (getMapBounds != null) {
            Rect mapRect = getMapBounds();
            Vector3 position = transform.position;
            if (mapRect.width < worldScreenArea.width) {
                position.x = mapRect.center.x;                
            }else{
                position.x = position.x < mapRect.xMin + worldScreenArea.width / 2f ? mapRect.xMin + worldScreenArea.width / 2f : position.x > mapRect.xMax - worldScreenArea.width / 2f ? mapRect.xMax - worldScreenArea.width / 2f : position.x;
            }

            if (mapRect.height < worldScreenArea.height) {
                position.y = mapRect.center.y;
            }
            else { 
                position.y = position.y < mapRect.yMin + worldScreenArea.height / 2f ? mapRect.yMin + worldScreenArea.height / 2f : position.y > mapRect.yMax - worldScreenArea.height / 2f ? mapRect.yMax - worldScreenArea.height / 2f : position.y;
            }
            transform.position = position;
        }
    }

#if UNITY_EDITOR
    //显示自由活动区域、平滑跟随区域的边界
    bool isShowBorder;
    void OnGUI() {
        if (GUILayout.Button(isShowBorder ? "隐藏相机跟随边界" : "显示相机跟随边界")) {
            isShowBorder = !isShowBorder;
        }
        if (isShowBorder) {
            GUI.Button(new Rect(m_camera.ViewportToScreenPoint(followInfo.FreeArea.min), m_camera.ViewportToScreenPoint(followInfo.FreeArea.max) - m_camera.ViewportToScreenPoint(followInfo.FreeArea.min)), "");
            GUI.Button(new Rect(m_camera.ViewportToScreenPoint(followInfo.SmoothArea.min), m_camera.ViewportToScreenPoint(followInfo.SmoothArea.max) - m_camera.ViewportToScreenPoint(followInfo.SmoothArea.min)), "");
        }
    }
#endif
}