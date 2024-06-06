using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITool:Singleton<UITool>
{
    public GameObject activepanel;
   public void SetActivePanel(GameObject panel)
    {
        activepanel = panel;
    }
    public T GetorAddComponent<T>() where T : Component
    {
        T t = activepanel.GetComponent<T>();
        if (t == null)
        {
            t = activepanel.AddComponent<T>();
        }
        return t;
    }
    public GameObject FindChildGameObject(string name)
    {
        Transform transform = activepanel.transform.Find(name);
        if (transform == null)
        {
            return null;
        }
        return transform.gameObject;
    }
    public T GetorAddComponentInChildren<T>() where T : Component
    {
        T t = activepanel.GetComponentInChildren<T>();
        if (t == null)
        {
            t = activepanel.AddComponent<T>();
        }
        return t;
    }
    // 需要输入父物体的递归查找子物体，找到返回，找不到返回null
    public GameObject FindDeepChild(GameObject parent, string childName)
    {
        if (parent.name == childName)
            return parent;

        foreach (Transform child in parent.transform)
        {
            GameObject result = FindDeepChild(child.gameObject, childName);
            if (result != null)
                return result;
        }

        return null;
    }
    // 直接在当前活动面板递归查找子物体，找到返回，找不到返回null
    public GameObject FindDeepChild(string childName)
    {
        if (activepanel == null)
        {
            Debug.LogError("Active panel is not set.");
            return null;
        }
        return FindDeepChild(activepanel, childName);
    }
}
