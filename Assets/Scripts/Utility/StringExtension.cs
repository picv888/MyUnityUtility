using System;
using System.Collections.Generic;

    /// <summary>
    /// 扩展String类的方法
    /// </summary>
    public static class StringExtension {

        /// <summary>
        /// 返回子字符串的Range，找不到则返回null
        /// </summary>
        public static MyStringRange RangeOfSubString(this string str, string subStr) {
            int strLen = str.Length;
            int subStrLen = subStr.Length;
            if (subStrLen > strLen) {
                return null;
            }
            for (int i = 0; i <= strLen - subStrLen; i++) {
                for (int j = 0; j < subStrLen; j++) {
                    if (str[i + j] != subStr[j]) {
                        break;
                    }
                    else if (j == subStrLen - 1) {
                        return new MyStringRange(i, subStrLen);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 搜索子字符串，并返回所有匹配的Range，找不到则返回null
        /// </summary>
        public static List<MyStringRange> AllRangeOfSubString(this string str, string subStr) {
            int strLen = str.Length;
            int subStrLen = subStr.Length;
            if (subStrLen > strLen) {
                return null;
            }

            List<MyStringRange> list = new List<MyStringRange>();
            for (int i = 0; i <= strLen - subStrLen; i++) {
                for (int j = 0; j < subStrLen; j++) {
                    if (str[i + j] != subStr[j]) {
                        break;
                    }
                    else if (j == subStrLen - 1) {
                        list.Add(new MyStringRange(i, subStrLen));
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }
    }
