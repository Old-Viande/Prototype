using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>(); // ������ֵ�

    // �����󷵻ص��������
    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }
        obj.SetActive(false); // ����Ϊ�Ǽ���״̬
        poolDictionary[key].Enqueue(obj); // ���������¼������
    }

    // �Ӷ�����л�ȡ����
    public GameObject GetObject(string key)
    {
        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
            obj.SetActive(true); // �������
            return obj;
        }
        else
        {
            Debug.LogWarning($"�������û�п��õĶ���: {key}");
            return null;
        }
    }

    // ��ȡ������е�ǰ���������
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

    // ��ն����
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