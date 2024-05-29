using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>(); // 对象池字典

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
    }
}