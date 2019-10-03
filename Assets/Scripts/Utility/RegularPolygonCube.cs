using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 表示正多边形管道
/// </summary>
public class RegularPolygonCube : MonoBehaviour {
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public static GameObject CreateRegularPolygonCube(List<Vector3> listCenter, float raduis, int numberOfSide) {
        if (listCenter.Count <= 1) {
            Debug.Log("至少要有两个正多边形的中心");
            return null;
        }
        if (raduis <= 0f) {
            Debug.Log("外接圆半径要大于0");
            return null;
        }

        if (numberOfSide < 3) {
            Debug.Log("正多边形的边数不能小于3");
            return null;
        }
        //创建游戏物体
        GameObject go = new GameObject();
        go.name = "RegularPolygonCube";
        RegularPolygonCube cube = go.AddComponent<RegularPolygonCube>();
        cube.meshFilter = go.AddComponent<MeshFilter>();
        cube.meshRenderer = go.AddComponent<MeshRenderer>();
        //使用unity自带的shader创建材质
        Shader shader = Shader.Find("Standard");
        Material mat = new Material(shader);
        cube.meshRenderer.sharedMaterial = mat;
        //获取所有顶点
        List<Vector3> allVertexs = new List<Vector3>();

        //获取第一个正多边形的顶点
        List<Vector3> previousVertexs = null;
        List<Vector3> currentVertexs = GetVertexsForRegularPolygon(Vector3.positiveInfinity, Vector3.positiveInfinity, listCenter[0], listCenter[1], raduis, numberOfSide);
        allVertexs.AddRange(currentVertexs);
        previousVertexs = currentVertexs;

        //获取中间的正多边形的顶点
        for (int i = 1; i < listCenter.Count - 1; i++) {
            currentVertexs = GetVertexsForRegularPolygon(listCenter[i - 1], previousVertexs[0], listCenter[i], listCenter[i + 1], raduis, numberOfSide);
            allVertexs.AddRange(currentVertexs);
            previousVertexs = currentVertexs;
        }

        //获取最后一个正多边形的顶点
        currentVertexs = GetVertexsForRegularPolygon(listCenter[listCenter.Count - 2], previousVertexs[0], listCenter[listCenter.Count - 1], Vector3.positiveInfinity, raduis, numberOfSide);
        allVertexs.AddRange(currentVertexs);

        //初始化网格，连接顶点
        cube.InitMesh(allVertexs, numberOfSide);
        //加网格碰撞体
        go.AddComponent<MeshCollider>();
        return go;
    }

    /// <summary>
    /// Gets the vertexs for regular polygon.
    /// </summary>
    /// <returns>The vertexs for regular polygon.</returns>
    /// <param name="previousCenter">Previous regular polygon center.</param>
    /// <param name="previousFirstVertex">Previous regular polygon first vertex.</param>
    /// <param name="currentCenter">Current regular polygon center.</param>
    /// <param name="nextCenter">Next regular polygon center.</param>
    /// <param name="radius">Radius.</param>
    /// <param name="numberOfSide">Number of side.</param>
    static List<Vector3> GetVertexsForRegularPolygon(Vector3 previousCenter, Vector3 previousFirstVertex, Vector3 currentCenter, Vector3 nextCenter, float radius, int numberOfSide) {
        List<Vector3> listCurrentVertex = new List<Vector3>();

        float angleDelta = -360f / numberOfSide;
        Vector3 currentNormal = Vector3.one;//当前正多边形的法向量
        Vector3 firstVertexDir = Vector3.one;//当前正多边形的中心点指向第一个顶点的方向

        //当previousCenter是Vector3.positiveInfinity时，表示currentCenter是第一个正多边形的中心点
        //当nextCenter是Vector3.positiveInfinity时，表示currentCenter是最后一个正多边形的中心点
        //每个正多边形的法向量为自己的中心点指向下一个正多边形的中心点
        //最后一个正多边形的法向量和倒数第二个一样
        if (nextCenter.Equals(Vector3.positiveInfinity)) {
            currentNormal = currentCenter - previousCenter;
        }
        else {
            currentNormal = nextCenter - currentCenter;
        }

        if (previousCenter.Equals(Vector3.positiveInfinity)) {
            /*
             当前正多边形是第一个时，
             用世界坐标的上方和右方旋转分别对应currentNormal和firstVertexDir，求得firstVertexDir
             */
            Quaternion fromToRotation = Quaternion.FromToRotation(Vector3.up, currentNormal);
            firstVertexDir = fromToRotation * Vector3.right;
        }
        else {
            /*
             当前正多边形不是第一个正多边形时，
             由上一个正多边形的第一个顶点发出射线（方向和上一个正多边形的法向量一样）
             和当前正多边形相交，得到的点减去当前正多边形的中心点,
             就得到当前正多边形中心点指向第一个顶点的方向向量
            */
            Vector3 interacPoint = VectorUtility.GetInteracPointRay2Plane(previousFirstVertex, currentCenter - previousCenter, currentCenter, currentNormal);
            firstVertexDir = (interacPoint - currentCenter).normalized;
        }

        for (int i = 0; i < numberOfSide; i++) {
            Vector3 vertex = currentCenter + Quaternion.AngleAxis(angleDelta * i, currentNormal) * (firstVertexDir * radius);
            listCurrentVertex.Add(vertex);
        }
        listCurrentVertex.Add(listCurrentVertex[0]);//最后一个顶点和第一个顶点重合
        return listCurrentVertex;
    }

    /// <summary>
    /// 初始化网格，连接所有多边形的顶点
    /// </summary>
    /// <param name="vertexs">所有多边形的顶点组成的列表</param>
    /// <param name="numberOfSide">多边形的边数</param>
    private void InitMesh(List<Vector3> vertexs, int numberOfSide) {

        int numberOfVertex = numberOfSide + 1;//每个正多边形的顶点数
        int numberOfPolygon = vertexs.Count / numberOfVertex;//正多边形个数

        List<int> triangles = new List<int>();
        List<Vector2> listUV = new List<Vector2>();
        //设置正多边形管道侧面的三角面和UV
        for (int i = 0; i < numberOfPolygon; i++) {
            for (int j = 0; j < numberOfVertex; j++) {
                //设置uv
                listUV.Add(new Vector2(((float)j) / numberOfSide, ((float)i) / numberOfPolygon));

                //设置三角形
                if (i < numberOfPolygon - 1 && j < numberOfVertex - 1) {
                    triangles.Add(numberOfVertex * i + j);
                    triangles.Add(numberOfVertex * (i + 1) + j);
                    triangles.Add(numberOfVertex * i + (j + 1));

                    triangles.Add(numberOfVertex * (i + 1) + j);
                    triangles.Add(numberOfVertex * (i + 1) + (j + 1));
                    triangles.Add(numberOfVertex * i + (j + 1));
                }
            }
        }

        meshFilter.mesh.vertices = vertexs.ToArray();
        meshFilter.mesh.triangles = triangles.ToArray();
        meshFilter.mesh.uv = listUV.ToArray();
        meshFilter.mesh.RecalculateBounds();//自动计算边界
        meshFilter.mesh.RecalculateNormals();//自动计算法向量，调用这个方法后shader才能正常计算漫反射、高光，否则计算出来的都是黑色
        meshFilter.mesh.RecalculateTangents();//自动计算切线
    }
}
