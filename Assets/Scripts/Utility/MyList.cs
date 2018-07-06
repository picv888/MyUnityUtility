using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//对List的封装，使List可以序列化、反序列化
[Serializable]
public class MyList<T> {
    [SerializeField]
    List<T> list;
    public List<T> ToList() { return list; }

    public MyList(List<T> list) {
        this.list = list;
    }
}
