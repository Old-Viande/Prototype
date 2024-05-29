using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHitPoint : Singleton<MouseHitPoint>
{
    
    public Vector3 point;
    public GameObject hitObject;
    //写一个可以从摄像机发射一条射线检测鼠标所在的屏幕位置的方法如果射线与物体碰撞，则返回碰撞点的坐标以及碰撞的物体
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
    
    
  
    //这个方法会在鼠标进行点击时调用，如果点击到了物体，则会在控制台输出碰撞点的坐标以及碰撞的物体
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
