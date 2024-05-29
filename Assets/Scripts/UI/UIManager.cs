using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private Dictionary<UItype, GameObject> UIsave;
    public UIManager()
    {
        //初始化UI
        UIsave = new Dictionary<UItype, GameObject>();
    }
    public GameObject GetUI(UItype type)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {//如果已经找到了UI就返回那个UI
            if (UIsave.ContainsKey(type))
            {
                return UIsave[type];
            }
            else
            {
                //如果没有找到UI就加载UI,并且保存到字典里
                //这里的Resources.Load是加载Resources文件夹下的资源 path是UI的路径 存在具体UI类的数据结构里
                GameObject ui = GameObject.Instantiate(Resources.Load<GameObject>(type.path), canvas.transform);
                ui.name = type.name;
                UIsave.Add(type, ui);
                return ui;

            }
        }
        return null;
    }

    /* public GameObject GetUI(UItype type)
     {
         GameObject canvas = GameObject.Find("Canvas");
         if (canvas == null)
         {
             Debug.LogError("Canvas not found in the scene!");
             return null;
         }

         GameObject ui;
         // 检查是否已经存在UI的实例
         if (UIsave.TryGetValue(type, out ui))
         {
             // 确保UI面板是激活的
             if (!ui.activeSelf)
             {
                 ui.SetActive(true);
             }
             return ui;
         }
         else
         {
             // 如果没有找到UI就加载UI,并且保存到字典里
             ui = GameObject.Instantiate(Resources.Load<GameObject>(type.path), canvas.transform);
             if (ui == null)
             {
                 Debug.LogError("Failed to load UI prefab from path: " + type.path);
                 return null;
             }
             ui.name = type.name;
             UIsave.Add(type, ui);
             return ui;
         }
     }*/
    public void DestroyUI(UItype type)
    {
        if (UIsave.ContainsKey(type))
        {
            GameObject.Destroy(UIsave[type]);
            UIsave.Remove(type);
        }
    }
}
