using System.Collections.Generic;

/// <summary>
/// 扩展List
/// </summary>
public static class ListExtension {

    /// <summary>
    /// 删除中List中重复的元素，只保留一个，返回一个新的List
    /// </summary>
    /// <param name="comparison">比较两个元素是否一样的委托.返回0表示两个元素一样</param>
    public static List<T> RemoveRepetition<T>(this List<T> list, System.Comparison<T> comparison) {
        if (comparison == null) {
            throw new System.Exception("参数不能为空");
        }
        List<T> newList = new List<T>();
        foreach (var item in list) {
            bool isContain = false;
            foreach (var itemInNewList in newList) {
                if (comparison(item, itemInNewList) == 0) {
                    isContain = true;
                }
            }
            if (!isContain) {
                newList.Add(item);
            }
        }
        return newList;
    }
}
