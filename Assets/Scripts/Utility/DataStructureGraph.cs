using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图的接口IGraph<T> 
/// </summary>
public interface IGraph<T> {
    //获取顶点数
    int GetNumOfVertex();
    //获取边或弧的数目
    int GetNumOfEdge();
    //在两个顶点之间添加权为v的边或弧，顶点v1指向顶点v2
    void SetEdge(Node<T> v1, Node<T> v2, int v);
    //删除两个顶点之间的边或弧，顶点v1指向顶点v2
    void DelEdge(Node<T> v1, Node<T> v2);
    //判断两个顶点之间是否有边或弧，顶点v1指向顶点v2
    bool IsEdge(Node<T> v1, Node<T> v2);
}

/// <summary>
/// 该类表示图的一个顶点
/// </summary>
public class Node<T> {
    //数据
    private T data;
    //构造器
    public Node(T v) {
        data = v;
    }

    //入度
    public int inDegree = 0;

    //出度
    public int outDegree = 0;

    //用邻接矩阵表示法时，顶点的下标
    public int indexOfAdjMatrix = -1;

    //数据域属性
    public T Data {
        get {
            //数据域
            return data;
        }
        set {
            data = value;
        }
    }
}

/// <summary>
/// 用邻接矩阵表示图(有向图、无向图、网 都适用)
/// </summary>
public class DirecNetAdjMatrix<T> : IGraph<T> {
    private Node<T>[] nodes;
    private int numArcs;
    private int[,] matrix;

    //构造器
    public DirecNetAdjMatrix(int n) {
        nodes = new Node<T>[n];
        matrix = new int[n, n];
        //用int.MaxValue表示下标x的顶点到下标y的顶点之间没有弧
        for (int x = 0; x < matrix.GetLength(0); x++) {
            for (int y = 0; y < matrix.GetLength(1); y++) {
                matrix[x, y] = int.MaxValue;
            }
        }
        numArcs = 0;
    }
    //获取索引为index的顶点的信息
    public Node<T> GetNode(int index) {
        return nodes[index];
    }

    //设置索引为index的顶点的信息
    public void SetNode(int index, Node<T> v) {
        nodes[index] = v;
        v.indexOfAdjMatrix = index;
    }

    //弧数目属性
    public int NumArcs {
        get {
            return numArcs;
        }
        set {
            numArcs = value;
        }
    }

    //获取matrix[index1, index2]的值
    public int GetMatrix(int index1, int index2) {
        return matrix[index1, index2];
    }

    //设置matrix[index1, index2]的值
    public void SetMatrix(int index1, int index2, int v) {
        matrix[index1, index2] = v;
    }

    //获取顶点数目
    public int GetNumOfVertex() {
        return nodes.Length;
    }

    //获取弧的数目
    public int GetNumOfEdge() {
        return numArcs;
    }

    //判断v是否是网的顶点
    public bool IsNode(Node<T> v) {
        //遍历顶点数组
        foreach (Node<T> nd in nodes) {
            //如果顶点nd与v相等，则v是图的顶点，返回true 
            if (v.Equals(nd)) {
                return true;
            }
        }
        return false;
    }

    //获取v在顶点数组中的索引
    public int GetIndex(Node<T> v) {
        /*
        int i = -1;
        //遍历顶点数组
        for (i = 0; i < nodes.Length; ++i) {
            //如果顶点nd与v相等，则v是图的顶点，返回索引值
            if (nodes[i].Equals(v)) {
                return i;
            }
        }
        */

        return v.indexOfAdjMatrix;
    }

    //在v1和v2之间添加权为v的弧
    public void SetEdge(Node<T> v1, Node<T> v2, int v) {
        //v1或v2不是网的顶点
        if (!IsNode(v1) || !IsNode(v2)) {
            Debug.Log("Node is not belong to Graph!");
            return;
        }
        matrix[GetIndex(v1), GetIndex(v2)] = v;
        v1.outDegree++;
        v2.inDegree++;
        ++numArcs;
    }

    //删除v1和v2之间的弧
    public void DelEdge(Node<T> v1, Node<T> v2) {
        //v1或v2不是网的顶点
        if (!IsNode(v1) || !IsNode(v2)) {
            Debug.Log("Node is not belong to Graph!");
            return;
        }
        //v1和v2之间存在弧
        if (matrix[GetIndex(v1), GetIndex(v2)] != int.MaxValue) {
            matrix[GetIndex(v1), GetIndex(v2)] = int.MaxValue;
            v1.outDegree--;
            v2.inDegree--;
            --numArcs;
        }
    }

    //判断v1和v2之间是否存在弧
    public bool IsEdge(Node<T> v1, Node<T> v2) {
        //v1或v2不是网的顶点
        if (!IsNode(v1) || !IsNode(v2)) {
            Debug.Log("Node is not belong to Graph!");
            return false;
        }

        //v1和v2之间存在弧
        if (matrix[GetIndex(v1), GetIndex(v2)] != int.MaxValue) {
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// 获取指定入度的值的顶点列表
    /// </summary>
    /// <returns>The node list by in degree.</returns>
    /// <param name="ID">Identifier.</param>
    public List<Node<T>> GetNodeListByInDegree(int ID) {
        List<Node<T>> list = new List<Node<T>>();
        foreach (var item in nodes) {
            if (item.inDegree == ID) {
                list.Add(item);
            }
        }
        return list;
    }
}

