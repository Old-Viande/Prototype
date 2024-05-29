using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg;
using cfg.assets;
using System.IO;
using System;

public class LodManager : Singleton<LodManager>
{
    string gameConfigDir = "Resources/Cpnfig/";
    private Assets assets;
    private Tables tables;
    private void Start()
    {
        tables = new Tables(Loader);
        /*assets = tables.Tbasstes.Get("SwordMan");  // 修正后的命名空间和表引用
        Debug.Log("找到了么？" + assets);*/
    }
    private JSONNode Loader(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, gameConfigDir, fileName + ".json");
        Debug.Log($"Loading file: {filePath}");
        string json = File.ReadAllText(filePath);
        return JSON.Parse(json);
    }
    /// <summary>
    /// 本地预制体资源加载
    /// </summary>
    /// <param name="Fillname"></param>
    /// <returns></returns>
    public GameObject LoadResource(string Fillname)
    {
        //如果加载路径不为空
        if (Loadpath(Fillname) != null)
        {
            //加载资源
            return Resources.Load<GameObject>(Loadpath(Fillname));
        }

        return null;
    }
    /// <summary>
    /// UI层相关预制体资源加载
    /// </summary>
    /// <param name="Fillname"></param>
    /// <returns></returns>
    public GameObject LoadUIResource(string Fillname)
    {
        //如果加载路径不为空
        if (Loadpath(Fillname) != null)
        {
            //加载资源
            return Resources.Load<GameObject>(LoadUIpath(Fillname));
        }

        return null;
    }
    #region
    private string Loadpath(string Fillname)
    {
        //根据文件名获取路径
        assets = tables.Tbasstes.Get(Fillname);
        return assets.ObjPath;

    }
    private string LoadUIpath(string Fillname)
    {
        //根据文件名获取路径
        assets = tables.Tbasstes.Get(Fillname);
        return assets.UIPath;

    }
    #endregion

    void Update()
    {

    }
}
