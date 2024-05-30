using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Halberd : BaseAction
{

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.MeleeAttack += Attack;
    }
    // Start is called before the first frame update
    void Start()
    {
        UniteSave = this.GetComponent<PawnData>().Unites;
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    private void Attack()
    {
        GameManager.Instance.Attacklis.Add(UniteSave.Speed, this.gameObject);
        AttackJudg();
    }

    public override void AttackJudg()
    {
        GameObject target;
        GameManager.Instance.unitesGridMap.GetGridXZ(this.gameObject.transform.position, out int x, out int z);
        //�ж������Ƿ��ǹ�����
        if (isAttacker)//�ǹ�����
        {
            GameObject[] targets = new GameObject[UniteSave.Range];//����һ�����������洢���Ŀ��
            for (var i = UniteSave.Range; i> 0; i--)//ѭ������������Χ�����е�λ�����վ����Զ������˳�����Ŀ������
            {
                if (GameManager.Instance.unitesGridMap.GetValue(x + i, z).GetComponent<BaseAction>().isAttacker!)
                {
                    target = GameManager.Instance.unitesGridMap.GetValue(x + i, z);//����Ƿ�������Ŀ����Ϊ����Ŀ��
                    if (target != null)
                    {
                        targets[i - 1] = target;//��Ŀ���������,����������һ��λ�ÿ�ʼ�洢
                    }
                }
            }//�����������������Ŀ��
            for (var i= targets.Length-1;i>0;i--)//����Ŀ�����飬����Զ��Ŀ�꿪ʼ����
            {
                if (targets[i] != null)//��Զ��Ŀ�겻Ϊ�գ�����й���
                {
                    GameManager.Instance.AttackSettlement(this.gameObject, targets[i]);//���й���
                    break;//����ѭ��
                }
            }

        }
        else//�Ƿ��ط�
        {
            GameObject[] targets = new GameObject[UniteSave.Range];//����һ�����������洢���Ŀ��
            for (var i = UniteSave.Range; i > 0; i--)//ѭ������������Χ�����е�λ�����վ����Զ������˳�����Ŀ������
            {
                if (GameManager.Instance.unitesGridMap.GetValue(x - i, z).GetComponent<BaseAction>().isAttacker)
                {
                    target = GameManager.Instance.unitesGridMap.GetValue(x - i, z);//����Ƿ�������Ŀ����Ϊ����Ŀ��
                    if (target != null)
                    {
                        targets[i - 1] = target;//��Ŀ���������,����������һ��λ�ÿ�ʼ�洢
                    }
                }
            }//�����������������Ŀ��
            for (var i = targets.Length - 1; i > 0; i--)//����Ŀ�����飬�������Ŀ�꿪ʼ����
            {
                if (targets[i] != null)//�����Ŀ�겻Ϊ�գ�����й���
                {
                    GameManager.Instance.AttackSettlement(this.gameObject, targets[i]);//���й���
                    break;//����ѭ��
                }
            }
        }
    }

    protected override void OnDisable()
    {
        EventManager.MeleeAttack -= Attack;
    }

}
