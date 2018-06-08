// Serialization.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//对List的封装，使List可以序列化、反序列化
[Serializable]
public class Serialization<T> {
    [SerializeField]
    List<T> items;
    public List<T> ToList() { return items; }

    public Serialization(List<T> items) {
        this.items = items;
    }
}


//对Dictionary的封装，使Dictionary可以序列化、反序列化
[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver {
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target) {
        this.target = target;
    }

    public void OnBeforeSerialize() {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize() {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i) {
            target.Add(keys[i], values[i]);
        }
    }
}