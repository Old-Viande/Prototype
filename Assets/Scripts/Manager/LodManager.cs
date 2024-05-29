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
        /*assets = tables.Tbasstes.Get("SwordMan");  // ������������ռ�ͱ�����
        Debug.Log("�ҵ���ô��" + assets);*/
    }
    private JSONNode Loader(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, gameConfigDir, fileName + ".json");
        Debug.Log($"Loading file: {filePath}");
        string json = File.ReadAllText(filePath);
        return JSON.Parse(json);
    }
    /// <summary>
    /// ����Ԥ������Դ����
    /// </summary>
    /// <param name="Fillname"></param>
    /// <returns></returns>
    public GameObject LoadResource(string Fillname)
    {
        //�������·����Ϊ��
        if (Loadpath(Fillname) != null)
        {
            //������Դ
            return Resources.Load<GameObject>(Loadpath(Fillname));
        }

        return null;
    }
    /// <summary>
    /// UI�����Ԥ������Դ����
    /// </summary>
    /// <param name="Fillname"></param>
    /// <returns></returns>
    public GameObject LoadUIResource(string Fillname)
    {
        //�������·����Ϊ��
        if (Loadpath(Fillname) != null)
        {
            //������Դ
            return Resources.Load<GameObject>(LoadUIpath(Fillname));
        }

        return null;
    }
    #region
    private string Loadpath(string Fillname)
    {
        //�����ļ�����ȡ·��
        assets = tables.Tbasstes.Get(Fillname);
        return assets.ObjPath;

    }
    private string LoadUIpath(string Fillname)
    {
        //�����ļ�����ȡ·��
        assets = tables.Tbasstes.Get(Fillname);
        return assets.UIPath;

    }
    #endregion

    void Update()
    {

    }
}
