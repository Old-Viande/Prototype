using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class CanvasTop : MonoBehaviour
{
    public Slider bloodbar;
    public Slider bloodFloor;
    private bool FloorStart = false;
    private float FloorTime = 0;
    public float FloorSpeed = 1;
    public PawnData pawn;


    private void Start()
    {
        pawn = this.transform.parent.GetComponent<PawnData>();
        EventManager.BloodBarChange += OnHpChange;
    }
    public void OnHpChange()
    {
        bloodbar.value = (float)pawn.Defence / (float)pawn.Unites.Defence;
        /*FloorStart = true;
        FloorTime = 0;*/
    }
    private void Update()
    {
        /*  if (FloorStart && FloorTime < 1)
          {
              FloorTime += Time.deltaTime * FloorSpeed;
              bloodFloor.value = Mathf.Lerp(bloodFloor.value, bloodbar.value, FloorTime);
          }
          if (FloorTime >= 1)
          {
              FloorTime = 0;
              FloorStart = false;
          }*/

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

    private void LateUpdate()
    {
        FaceAway();       
    }


}

