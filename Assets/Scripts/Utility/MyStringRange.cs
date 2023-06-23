using UnityEngine;
using System.Collections;

/// <summary>
/// 该类使用起始坐标和长度表示子字符串的范围
/// </summary>
public class MyStringRange {
    public int index;
    public int length;
    public MyStringRange() { }
    public MyStringRange(int index, int length) {
        this.index = index;
        this.length = length;
    }
    public override string ToString() {
        return string.Format("[Range: index={0}, length={1}]", index, length);
    }
}
