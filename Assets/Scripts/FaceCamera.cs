using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class FaceCamera : MonoBehaviour
{
    public bool flipYAxis = false;

    public void OnEnable()
    {
        //flipYAxis=TurnBaseFSM.Instance.currentStateType==States.AttackPlacement?false:true;
    }
    void Update()
    {
        // 确保在Update中调用，以便物体持续更新朝向
        FaceAway();
    }

    private void FaceAway()
    {
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // 检查是否需要进行Y轴翻转
       /* if (flipYAxis)
        {
            // 将物体的旋转设置为与摄像机的旋转相匹配，然后在Y轴上增加180度旋转
            transform.rotation = cameraRotation * Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // 无需翻转，直接匹配摄像机的旋转
            transform.rotation = cameraRotation;
        }*/
        transform.rotation = cameraRotation;
    }
}
