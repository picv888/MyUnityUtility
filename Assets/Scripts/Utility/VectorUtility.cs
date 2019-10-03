using UnityEngine;

public static class VectorUtility {
    // Use this for initialization
    public static string ToDescription(this Vector2 vector2) {
        return string.Format("(" + vector2.x + ", " + vector2.y + ")");
    }

    public static string ToDescription(this Vector3 vector3) {
        return string.Format("(" + vector3.x + ", " + vector3.y + ", " + vector3.z + ")");
    }

    public static string ToDescription(this Quaternion quaternion) {
        return string.Format("(" + quaternion.x + ", " + quaternion.y + ", " + quaternion.z + ", " + quaternion.w + ")");
    }

    /// <summary>
    /// 获取射线和平面的交点
    /// </summary>
    /// <returns>The interac point ray2 plane.</returns>
    /// <param name="rayStartPoint">Ray start point.</param>
    /// <param name="rayDirection">Ray direction.</param>
    /// <param name="pointOnPlane">Point on plane.</param>
    /// <param name="normalForPlane">Normal for plane.</param>
    public static Vector3 GetInteracPointRay2Plane(Vector3 rayStartPoint, Vector3 rayDirection, Vector3 pointOnPlane, Vector3 normalForPlane) {
        Vector3 p;
        float t;
        t = (Vector3.Dot(normalForPlane, pointOnPlane) - Vector3.Dot(normalForPlane, rayStartPoint)) / Vector3.Dot(normalForPlane, rayDirection);
        p = rayStartPoint + t * rayDirection;
        return p;
    }
}