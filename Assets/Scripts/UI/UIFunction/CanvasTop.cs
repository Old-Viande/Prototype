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

        // ����Ƿ���Ҫ����Y�ᷭת
        /* if (flipYAxis)
         {
             // ���������ת����Ϊ�����������ת��ƥ�䣬Ȼ����Y��������180����ת
             transform.rotation = cameraRotation * Quaternion.Euler(0, 180, 0);
         }
         else
         {
             // ���跭ת��ֱ��ƥ�����������ת
             transform.rotation = cameraRotation;
         }*/
        transform.rotation = cameraRotation;
    }

    private void LateUpdate()
    {
        FaceAway();       
    }


}

