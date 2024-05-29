using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHitPoint : Singleton<MouseHitPoint>
{
    
    public Vector3 point;
    public GameObject hitObject;
    //дһ�����Դ����������һ�����߼��������ڵ���Ļλ�õķ������������������ײ���򷵻���ײ��������Լ���ײ������
    public bool GetMousePoint(out Vector3 point, out GameObject hitObject)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            point = hit.point;
            hitObject = hit.collider.gameObject;
            return true;
        }
        else
        {
            point = Vector3.zero;
            hitObject = null;
            return false;
        }
    }
    
    
  
    //����������������е��ʱ���ã��������������壬����ڿ���̨�����ײ��������Լ���ײ������
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point;
            GameObject hitObject;
            if (GetMousePoint(out point, out hitObject))
            {
                this.hitObject = hitObject;
                this.point = point;
                GameManager.Instance.floorGridMap.GetGridXZ(point, out int x, out int z);
                Debug.Log("Hit Object: " + hitObject.name);
                Debug.Log("Hit Point: " + point);
                Debug.Log("Grid X: " + x + " Grid Z: " + z);
                /*GameManager.Instance.floorGridMap.GetGridXZ(CharacterMove.Instance.character.transform.position, out int x1, out int z1);
                CharacterMove.Instance.SetMoveData(GameManager.Instance.pathFinder.FindPath(x1,z1,x, z));*/
               
            }
        }
    }

   
}
