using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏开始时执行的脚本，将该脚本挂载在场景中的一个空物体上
/// </summary>
public class Demo1GameStart : MonoBehaviour {
    void Awake() {
        //创建正多边形管道
        List<Vector3> listCenter = GetPolygonCenters();
        GameObject cube = RegularPolygonCube.CreateRegularPolygonCube(listCenter, 0.5f, 10);
        //使物体可以被拖动
        if (null != cube) {
            cube.AddComponent<DragGameObject>();
        }
    }

    List<Vector3> GetPolygonCenters() {
        List<Vector3> listCenter = new List<Vector3>();
        listCenter.Add(transform.position);
        Vector3 currentForward = GetRandomForward();
        Vector3 targetForward = GetRandomForward();
        int turnIndex = 0;//第几次弯道
        int turnCount = 100;//弯道个数
        float radius = GetRandomRadius();//弯道半径
        float distancePer10Degree = radius * Mathf.PI / 18f;//每转10度的位移 = 弯道半径 * 2 * PI / 360 * 10 = 弯道半径 * Pi / 18

        while (turnIndex < turnCount) {
            if (Vector3.Angle(currentForward, targetForward) > 10f) {
                //角度大于10度，旋转10度
                currentForward = Vector3.Slerp(currentForward, targetForward, 10f / Vector3.Angle(currentForward, targetForward));
                listCenter.Add(listCenter[listCenter.Count - 1] + currentForward * distancePer10Degree);
            }
            else {
                //角度小于10度，直接完成转弯
                currentForward = targetForward;
                turnIndex++;
                listCenter.Add(listCenter[listCenter.Count - 1] + currentForward * distancePer10Degree);
                //随机直走一段距离再转弯
                listCenter.Add(listCenter[listCenter.Count - 1] + currentForward * Random.Range(3f, 10f));
                //获取下一次旋转
                targetForward = GetRandomForward();
                //随机弯道半径
                radius = GetRandomRadius();
                distancePer10Degree = radius * Mathf.PI / 18f;
            }
        }
        return listCenter;
    }

    /// <summary>
    /// //随机弯道半径
    /// </summary>
    float GetRandomRadius(){
        return Random.Range(0.5f, 5f);
    }

    /// <summary>
    /// 随机获取一个朝向
    /// </summary>
    Vector3 GetRandomForward() {
        Vector3 fw;
        float u = Random.Range(0f, 1f);
        float v = Random.Range(0f, 1f);
        float theta = 180f * u;
        float phi = Mathf.Acos(2 * v - 1) * Mathf.Rad2Deg;
        fw.x = Mathf.Sin(theta) * Mathf.Sin(phi);
        fw.y = Mathf.Cos(theta) * Mathf.Sin(phi);
        fw.z = Mathf.Cos(phi);
        return fw;
    }
}
