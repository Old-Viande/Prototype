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
        // ȷ����Update�е��ã��Ա�����������³���
        FaceAway();
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
}
