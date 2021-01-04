using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 打包AssetBundle
/// </summary>
public class CreateAssetBundle {
    [MenuItem("MyUtility/Create AssetBundle")]
    static void CreateAB() {
        string directoryPath = Application.streamingAssetsPath + "/AssetBundle/";
        if (!Directory.Exists(directoryPath))//如果路径不存在
        {
            Directory.CreateDirectory(directoryPath);//创建路径上的所有文件夹
        }
        //第一参数： AB包的输出路径
        //第二个参数： 打包的参数设置， 我设置的是强制性的重新打包
        //第三个参数： AB包的适用平台， 不同平台使用AB包是不一样的
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(directoryPath,
                                                                       BuildAssetBundleOptions.ForceRebuildAssetBundle,
                                                                       BuildTarget.StandaloneOSX);
        if (manifest != null) {
            Debug.Log("资源打包成功");
        }
        else {
            Debug.Log("资源打包失败");
        }
        AssetDatabase.Refresh();
    }
}
