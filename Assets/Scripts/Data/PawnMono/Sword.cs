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

    private void Attack()
    {
        GameManager.Instance.Attacklis.Add(UniteSave.Speed, this.gameObject);
        
    }
     public override void AttackJudg()
    {
        GameObject target;
        GameManager.Instance.unitesGridMap.GetGridXZ(this.gameObject.transform.position, out int x, out int z);
        //�ж������Ƿ��ǹ�����
        if (isAttacker)//�ǹ�����
        {
            
            if (GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z).GetComponent<BaseAction>().isAttacker)//�ж�Ŀ���Ƿ��ǹ�����
            {
                return;//����ǹ������򷵻�
            }
            else
            {               
                target = GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z);//����Ŀ����Ϊ����Ŀ��
                GameManager.Instance.AttackSettlement(this.gameObject, target);
            }
        }
        else//�Ƿ��ط�
        {
            if (GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z).GetComponent<BaseAction>().isAttacker)//�ж�Ŀ���Ƿ��ǹ�����
            {               
                target = GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z);//����ǹ�������Ŀ����Ϊ����Ŀ��
                GameManager.Instance.AttackSettlement(this.gameObject, target);
            }          
            else
            {
                return;//����Ƿ��ط��򷵻�
            }
        }
       
    }

    protected override void OnDisable()
    {
        EventManager.MeleeAttack -= Attack;
    }

}
