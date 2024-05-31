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
        //判断自身是否是攻击方
        if (isAttacker)//是攻击方
        {
            if(GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z) == null)
            {
                return;//直接返回
            }
            if (GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z).GetComponent<BaseAction>().isAttacker)//判断目标是否是攻击方
            {
                return;//如果是攻击方则返回
            }
            else
            {               
                target = GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z);//否则将目标设为攻击目标
                GameManager.Instance.AttackSettlement(this.gameObject, target);
                AnimaSet(target);
            }
        }
        else//是防守方
        {
            if (GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z) == null)
            {
                return;//直接返回
            }
            if (GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z).GetComponent<BaseAction>().isAttacker)//判断目标是否是攻击方
            {               
                target = GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z);//如果是攻击方则将目标设为攻击目标
                GameManager.Instance.AttackSettlement(this.gameObject, target);
                AnimaSet(target);
            }          
            else
            {
                return;//如果是防守方则返回
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
