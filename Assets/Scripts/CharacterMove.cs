using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : Singleton<CharacterMove>
{
    public GameObject character;
    //private List<SPathNode> paths;
    private List<PathNode> paths;
    private int i;
    private int pathIndex;
    private Vector3 targetPoint;
    /* public void SetMoveData(List<SPathNode> paths)
     {

             this.paths = paths;
             i = 0;           
             pathIndex = paths.ToArray().Length;


     }*/
    public void SetMoveData(List<PathNode> paths)
    {

        this.paths = paths;
        i = 0;
        pathIndex = paths.ToArray().Length;


    }

    // Update is called once per frame
    void Update()
    {
        //在这里实现一个利用寻路算法找到的路径点进行移动的方法
        if (paths != null)
        {
            if (i < pathIndex)
            {
                targetPoint = GameManager.Instance.pathFinder.GetGrid().GetGridCenter(paths[i].x, paths[i].z);
                if (Vector3.Distance(character.transform.position, targetPoint) < 0.1f)
                {
                    i++;
                }
                else
                {
                    character.transform.position = Vector3.MoveTowards(character.transform.position, targetPoint, 0.1f);
                }
            }else
            {
                paths.Clear();
            }
        }

    }

}
