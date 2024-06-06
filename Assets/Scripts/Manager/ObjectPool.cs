using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /* public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>(); // 对象池字典

     // 将对象返回到对象池中
     public void ReturnObject(string key, GameObject obj)
     {
         if (!poolDictionary.ContainsKey(key))
         {
             poolDictionary[key] = new Queue<GameObject>();
         }
         obj.SetActive(false); // 设置为非激活状态
         poolDictionary[key].Enqueue(obj); // 将对象重新加入队列
     }

     // 从对象池中获取对象
     public GameObject GetObject(string key)
     {
         if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
         {
             GameObject obj = poolDictionary[key].Dequeue();
             obj.SetActive(true); // 激活对象
             return obj;
         }
         else
         {
             Debug.LogWarning($"对象池中没有可用的对象: {key}");
             return null;
         }
     }

     // 获取对象池中当前对象的数量
     public int GetPoolCount(string key)
     {
         if (poolDictionary.ContainsKey(key))
         {
             return poolDictionary[key].Count;
         }
         else
         {
             return 0;
         }
     }

     // 清空对象池
     public void ClearPool()
     {
         foreach (var queue in poolDictionary.Values)
         {
             while (queue.Count > 0)
             {
                 GameObject obj = queue.Dequeue();
                 Destroy(obj);
             }
         }
         poolDictionary.Clear();
     }*/
    public string poolParentName = "ObjectPool"; // 对象池挂载物体的名称
    private Transform poolParent; // 对象池挂载的物体
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>(); // 对象池字典

    // 设置对象池的父物体
    public void SetPoolParent(Transform parent)
    {
        poolParent = parent;
        if (poolParent == null)
        {
            Debug.LogError("poolParent 为空，请确保传入的父物体不为空。");
        }
    }

    // 将对象返回到对象池中
    public void ReturnObject(string key, GameObject obj)
    {
        if (poolParent == null)
        {
            Debug.LogError("poolParent 为空，请确保已设置对象池的父物体。");
            return;
        }

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        obj.SetActive(false); // 设置为非激活状态
        obj.transform.SetParent(poolParent); // 将对象的父对象设置为对象池挂载物体
        poolDictionary[key].Enqueue(obj); // 将对象重新加入队列
    }

    // 从对象池中获取对象
    public GameObject GetObject(string key)
    {
        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
            obj.SetActive(true); // 激活对象
            obj.transform.SetParent(null); // 将对象的父对象设置为空（根级别）
            return obj;
        }
        else
        {
            Debug.LogWarning($"对象池中没有可用的对象: {key}");
            return null;
        }
    }

    // 获取对象池中当前对象的数量
    public int GetPoolCount(string key)
    {
        if (poolDictionary.ContainsKey(key))
        {
            return poolDictionary[key].Count;
        }
        else
        {
            return 0;
        }
    }

    // 清空对象池
    public void ClearPool()
    {
        foreach (var queue in poolDictionary.Values)
        {
            while (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                //Object.Destroy(obj);
            }
        }
        poolDictionary.Clear();
    }
}