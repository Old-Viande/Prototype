using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private Dictionary<UItype, GameObject> UIsave;
    public UIManager()
    {
        //��ʼ��UI
        UIsave = new Dictionary<UItype, GameObject>();
    }
    public GameObject GetUI(UItype type)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {//����Ѿ��ҵ���UI�ͷ����Ǹ�UI
            if (UIsave.ContainsKey(type))
            {
                return UIsave[type];
            }
            else
            {
                //���û���ҵ�UI�ͼ���UI,���ұ��浽�ֵ���
                //�����Resources.Load�Ǽ���Resources�ļ����µ���Դ path��UI��·�� ���ھ���UI������ݽṹ��
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
         // ����Ƿ��Ѿ�����UI��ʵ��
         if (UIsave.TryGetValue(type, out ui))
         {
             // ȷ��UI����Ǽ����
             if (!ui.activeSelf)
             {
                 ui.SetActive(true);
             }
             return ui;
         }
         else
         {
             // ���û���ҵ�UI�ͼ���UI,���ұ��浽�ֵ���
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
