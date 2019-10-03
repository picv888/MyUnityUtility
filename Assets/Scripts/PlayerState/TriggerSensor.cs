using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表示触发传感器
/// </summary>
public class TriggerSensor : MonoBehaviour {
    public List<Collider2D> listCollider = new List<Collider2D>();
    public bool IsTrigger {
        get {
            return listCollider.Count > 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.isTrigger) {
            listCollider.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        listCollider.Remove(collision);
    }
}
