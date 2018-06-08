using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 打包AssetBundle ////以后希望加上自动区分文件打包（区分texture、材质球、预制体、Lua、JSON、音乐、动画等等）
/// </summary>
public class CreateAssetBundle  {

    [MenuItem("MyUtility/Create AssetBundle")]
    static void CreateAB()
    {
        
        //第一参数： AB包的输出路径
        //第二个参数： 打包的参数设置， 我们设置的是强制性的重新打包
        //第三个参数： AB包的适用平台， 不同平台使用AB包是不一样的
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetBundle/",
            BuildAssetBundleOptions.ForceRebuildAssetBundle,
            BuildTarget.StandaloneWindows64);
    }

}
