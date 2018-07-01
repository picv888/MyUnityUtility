using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理AssetBundle，加载Asset的工具类，////还能继续优化加上异步加载功能
/// </summary>
public class AssetBundleManager
{
    #region 单例
    private static AssetBundleManager instance;
    public static AssetBundleManager Intance
    {
        get {
            if (instance == null)
            {
                instance = new AssetBundleManager();
            }
            return instance;
        }
    }
    private AssetBundleManager() {
        abDic = new Dictionary<string, AssetBundle>();
        abPath = Application.streamingAssetsPath + "/AssetBundle/";
        singleABName = "AssetBundle";
    }

    #endregion

    /// <summary>
    /// 用来存储已经加载了的AB包，键是AB包的名字，值就是AB包。
    /// </summary>
    private Dictionary<string, AssetBundle> abDic;

    //用来存储单一的ab包。
    private AssetBundle singleAB;

    //单一的构建清单，所有的ab包的依赖全部从这获取。
    private AssetBundleManifest singleManifest;

    //存储ab包的路径
    private string abPath;

    //单一的ab包的名字
    private string singleABName;

    /// <summary>
    /// 加载单一的ab包和单一的构建清单
    /// </summary>
    void LoadSingleAssetBundle()
    {
        //每次加载单一的ab包需要判断是否加载过，
        //singleAB为null没加载过，不为null就是加载过
        if (null == singleAB)
        {
            singleAB = AssetBundle.LoadFromFile(abPath + singleABName);
        }

        //从单一的ab包中加载单一的构建清单
        if (singleManifest == null)
        {
            singleManifest = singleAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    /// <summary>
    /// 加载指定ab包的所有的依赖项
    /// </summary>
    /// <param name="abName"></param>
    void LoadAllDependencies(string abName)
    {
        LoadSingleAssetBundle();
        //首先先获取指定的这个ab包的所有的依赖项
        //从单一的构建清单中获取
        string[] deps = singleManifest.GetAllDependencies(abName);

        //遍历去加载依赖项
        for (int i = 0; i < deps.Length; i++)
        {
            //加载该依赖项前，先判断之前加载没加载过该依赖项
            //就是判断存储ab包的字典里有没有这个ab包
            if (!abDic.ContainsKey(deps[i]))
            {
                //如果未加载过，需要加载.
                AssetBundle ab = AssetBundle.LoadFromFile(abPath + deps[i]);

                //ab包加载完之后， 把加载来的ab包存储在字典里
                abDic[deps[i]] = ab;
            }
          
        }
    }

    /// <summary>
    /// 加载指定的ab包，并且返回该ab包
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public AssetBundle LoadAssetBundle(string abName)
    {
        //先加载该ab包的所有的依赖项
        LoadAllDependencies(abName);

        //加载指定的ab包。
        //加载前先判是否已经加载过，如果加载过，把加载过的ab包给你
        //如果未加载过，就加载该ab包。
        AssetBundle ab = null;
        if (!abDic.TryGetValue(abName, out ab))//证明该ab包已经加载过
        {
            //如果进入到这，证明该键没有指定的值，那么证明该ab包未加载，需要加载
            ab = AssetBundle.LoadFromFile(abPath + abName);
            //把加载进来的ab包添加的字典中
            abDic[abName] = ab;
        }

        return ab;
    }

    /// <summary>
    /// 加载指定的ab包中的指定名字的指定类型的资源
    /// </summary>
    /// <typeparam name="T">指定资源的类型</typeparam>
    /// <param name="abName">ab包的名字</param>
    /// <param name="assetName">资源的名字</param>
    /// <returns></returns>
    public T LoadAssetByAB<T>(string abName, string assetName) where T : Object
    {
        //先获取指定的ab包
        AssetBundle ab = LoadAssetBundle(abName);
        if (ab != null)
        {
            //从ab包中加载指定的资源
            return ab.LoadAsset<T>(assetName);
        }
        else
        {
            Debug.LogError("指定的ab包的名字有误！");
        }
        return null;
    }

    /// <summary>
    /// 卸载指定的ab包
    /// </summary>
    /// <param name="abName"></param>
    public void UnloadAssetBundle(string abName, bool unloadAllLoadedObjects)
    {
        //先判断有没有这个ab包
        AssetBundle ab = null;
        if (abDic.TryGetValue(abName, out ab))
        {
            //卸载ab包
            ab.Unload(unloadAllLoadedObjects);
            //从容器中删除该ab包
            abDic.Remove(abName);
        }
    }

    /// <summary>
    /// 卸载全部的ab包
    /// </summary>
    /// <param name="unloadAllLoadedObjects"></param>
    public void UnloadAllAssetBundle(bool unloadAllLoadedObjects)
    {
        //遍历每一个ab包，调用ab包的卸载的方法
        //遍历键，通过键去获取值进行卸载
        /*
        foreach (var item in abDic.Keys)
        {
            //卸载ab包
            abDic[item].Unload(unloadAllLoadedObjects);
        }
        */
        //直接遍历值去卸载
        foreach (var item in abDic.Values)
        {
            item.Unload(unloadAllLoadedObjects);
        }

        //把字典所有内容清空
        abDic.Clear();
        
    }


}
