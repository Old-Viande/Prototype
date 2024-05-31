using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sword : BaseAction
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
            if(GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z) == null)
            {
                return;//ֱ�ӷ���
            }
            if (GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z).GetComponent<BaseAction>().isAttacker)//�ж�Ŀ���Ƿ��ǹ�����
            {
                return;//����ǹ������򷵻�
            }
            else
            {               
                target = GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z);//����Ŀ����Ϊ����Ŀ��
                GameManager.Instance.AttackSettlement(this.gameObject, target);
                AnimaSet(target);
            }
        }
        else//�Ƿ��ط�
        {
            if (GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z) == null)
            {
                return;//ֱ�ӷ���
            }
            if (GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z).GetComponent<BaseAction>().isAttacker)//�ж�Ŀ���Ƿ��ǹ�����
            {               
                target = GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z);//����ǹ�������Ŀ����Ϊ����Ŀ��
                GameManager.Instance.AttackSettlement(this.gameObject, target);
                AnimaSet(target);
            }          
            else
            {
                return;//����Ƿ��ط��򷵻�
            }
        }
        
    }
    private void AnimaSet(GameObject target)
    {
        Spine2DSkinList spineA = this.GetComponent<Spine2DSkinList>();
        Spine2DSkinList spineB= target.GetComponent<Spine2DSkinList>();
        string[] atktracks = new string[] { "Attacks/Squire_Attack" };
        string[] targettracks = new string[] { "Hit/Hit" };
        spineA.SetAnimation(atktracks,false);
        spineB.SetAnimation(targettracks,false);
        
    }
    protected override void OnDisable()
    {
        EventManager.MeleeAttack -= Attack;
    }

}
