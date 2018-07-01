using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 文件工具类，可以从指定路径文件中读取所有字符串、把字符串保存到指定路径的文件
/// </summary>
public static class FileTools {

    /// <summary>
    /// 读取指定路径的文件的所有字符串
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadJson(string path) {
        if (!File.Exists(path)) {
            return "";
        }
        string str = "";

        StreamReader sr = new StreamReader(path, Encoding.UTF8);

        try {
            str = sr.ReadToEnd();
        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
        }

        sr.Close();

        return str;
    }

    /// <summary>
    /// 把字符创写入指定路径的文件
    /// </summary>
    /// <param name="path">指定的路径</param>
    public static void WriteJson(string path, string str) {
        if (!File.Exists(path)) {
            FileStream fs = File.Create(path);
            fs.Close();
        }

        StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
        try {
            sw.Write(str);
        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
        }
        sw.Close();
    }
}
