using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spear : BaseAction
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
 
    public override void AttackJudg()
    {
        GameObject target;
        GameManager.Instance.unitesGridMap.GetGridXZ(this.gameObject.transform.position, out int x, out int z);
        //�ж������Ƿ��ǹ�����
        if (isAttacker)//�ǹ�����
        {
            GameObject[] targets = new GameObject[UniteSave.Range];//����һ�����������洢���Ŀ��
            for (var i = 1; i <= UniteSave.Range; i++)//ѭ������������Χ�����е�λ�����վ���ӽ���Զ��˳�����Ŀ������
            {
                if (GameManager.Instance.unitesGridMap.GetValue(x + i, z) == null)
                {
                    continue;//���Ŀ��Ϊ�գ��������˴�ѭ��
                }
                if (!GameManager.Instance.unitesGridMap.GetValue(x + i, z).GetComponent<BaseAction>().isAttacker)
                {
                    target = GameManager.Instance.unitesGridMap.GetValue(x + i, z);//����Ƿ�������Ŀ����Ϊ����Ŀ��
                    if (target != null)
                    {
                        targets[i - 1] = target;//��Ŀ���������
                    }
                }
            }//�����������������Ŀ��
            for (var i = 0; i < targets.Length; i++)//����Ŀ�����飬�������Ŀ�꿪ʼ����
            {
                if (targets[i] != null)//�����Ŀ�겻Ϊ�գ�����й���
                {
                    GameManager.Instance.AttackSettlement(this.gameObject, targets[i]);//���й���
                    AnimaSet(targets[i]);
                    break;//����ѭ��
                }
            }

        }
        else//�Ƿ��ط�
        {
            GameObject[] targets = new GameObject[UniteSave.Range];//����һ�����������洢���Ŀ��
            for (var i = 1; i <= UniteSave.Range; i++)//ѭ������������Χ�����е�λ�����վ���ӽ���Զ��˳�����Ŀ������
            {
                if (GameManager.Instance.unitesGridMap.GetValue(x - i, z) == null)
                {
                    continue;//���Ŀ��Ϊ�գ��������˴�ѭ��
                }
                if (GameManager.Instance.unitesGridMap.GetValue(x - i, z).GetComponent<BaseAction>().isAttacker)
                {
                    target = GameManager.Instance.unitesGridMap.GetValue(x - i, z);//����Ƿ�������Ŀ����Ϊ����Ŀ��
                    if (target != null)
                    {
                        targets[i - 1] = target;//��Ŀ���������
                    }
                }
            }//�����������������Ŀ��
            for (var i = 0; i < targets.Length; i++)//����Ŀ�����飬�������Ŀ�꿪ʼ����
            {
                if (targets[i] != null)//�����Ŀ�겻Ϊ�գ�����й���
                {
                    GameManager.Instance.AttackSettlement(this.gameObject, targets[i]);//���й���
                    AnimaSet(targets[i]);
                    break;//����ѭ��
                }
            }
        }
    }
    private void AnimaSet(GameObject target)
    {
        Spine2DSkinList spineA = this.GetComponent<Spine2DSkinList>();
        Spine2DSkinList spineB = target.GetComponent<Spine2DSkinList>();
        string[] atktracks = new string[] { "Attacks/Knight_Attack" };
        string[] targettracks = new string[] { "Hit/Hit" };
        spineA.SetAnimation(atktracks, false);
        spineB.SetAnimation(targettracks, false);
    }
    protected override void OnDisable()
    {
        EventManager.MeleeAttack -= Attack;
    }
}
