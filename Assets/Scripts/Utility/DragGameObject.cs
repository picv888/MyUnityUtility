using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 使3D、2D游戏物体可以被拖动
/// </summary>
public class DragGameObject : MonoBehaviour, IDragHandler {
    public Camera cam;//拍摄这个物体的相机
    private void Awake() {
        //获取拍摄这个物体的摄像机
        if (cam == null) {
            cam = Camera.main;
        }
        //如果没有摄像机，输出错误
        if (cam == null) {
            Debug.Log("找不到摄像机: " + gameObject.name);
        }

        //给摄像机添加物理射线投射组件
        if (cam.gameObject.GetComponent<PhysicsRaycaster>() == null) {
            cam.gameObject.AddComponent<PhysicsRaycaster>();
        }
        if (cam.gameObject.GetComponent<Physics2DRaycaster>() == null) {
            cam.gameObject.AddComponent<Physics2DRaycaster>();
        }
        //如果没有EventSystem组件，输出错误
        if (EventSystem.current == null) {
            Debug.Log("场景中没有EventSystem，无法拖动物体");
        }
        //如果物体没有碰撞体，输出错误
        if (gameObject.GetComponent<Collider>() == null && gameObject.GetComponent<Collider2D>() == null) {
            Debug.Log("该物体没有碰撞体: " + gameObject.name);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 screenP = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 worldFrom = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, screenP.z));
        Vector3 worldTo = Camera.main.ScreenToWorldPoint(new Vector3(eventData.delta.x, eventData.delta.y, screenP.z));
        transform.position += (worldTo - worldFrom);
    }
}