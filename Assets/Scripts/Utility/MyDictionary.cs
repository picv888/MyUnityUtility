// Serialization.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//对Dictionary的封装，使Dictionary可以序列化、反序列化
[Serializable]
public class MyDictionary<TKey, TValue> : ISerializationCallbackReceiver {
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> dict;
    public Dictionary<TKey, TValue> ToDictionary() { return dict; }

    public MyDictionary(Dictionary<TKey, TValue> dict) {
        this.dict = dict;
    }

    public void OnBeforeSerialize() {
        keys = new List<TKey>(dict.Keys);
        values = new List<TValue>(dict.Values);
    }

    public void OnAfterDeserialize() {
        var count = Math.Min(keys.Count, values.Count);
        dict = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i) {
            dict.Add(keys[i], values[i]);
        }
    }
}