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
        //判断自身是否是攻击方
        if (isAttacker)//是攻击方
        {
            GameObject[] targets = new GameObject[UniteSave.Range];//创建一个数组用来存储多个目标
            for (var i = 1; i <= UniteSave.Range; i++)//循环遍历攻击范围的所有单位，按照距离从近到远的顺序进行目标的添加
            {
                if (GameManager.Instance.unitesGridMap.GetValue(x + i, z) == null)
                {
                    continue;//如果目标为空，则跳过此次循环
                }
                if (!GameManager.Instance.unitesGridMap.GetValue(x + i, z).GetComponent<BaseAction>().isAttacker)
                {
                    target = GameManager.Instance.unitesGridMap.GetValue(x + i, z);//如果是防御方则将目标设为攻击目标
                    if (target != null)
                    {
                        targets[i - 1] = target;//将目标存入数组
                    }
                }
            }//遍历结束，获得所有目标
            for (var i = 0; i < targets.Length; i++)//遍历目标数组，从最近的目标开始攻击
            {
                if (targets[i] != null)//最近的目标不为空，则进行攻击
                {
                    GameManager.Instance.AttackSettlement(this.gameObject, targets[i]);//进行攻击
                    AnimaSet(targets[i]);
                    break;//跳出循环
                }
            }

        }
        else//是防守方
        {
            GameObject[] targets = new GameObject[UniteSave.Range];//创建一个数组用来存储多个目标
            for (var i = 1; i <= UniteSave.Range; i++)//循环遍历攻击范围的所有单位，按照距离从近到远的顺序进行目标的添加
            {
                if (GameManager.Instance.unitesGridMap.GetValue(x - i, z) == null)
                {
                    continue;//如果目标为空，则跳过此次循环
                }
                if (GameManager.Instance.unitesGridMap.GetValue(x - i, z).GetComponent<BaseAction>().isAttacker)
                {
                    target = GameManager.Instance.unitesGridMap.GetValue(x - i, z);//如果是防御方则将目标设为攻击目标
                    if (target != null)
                    {
                        targets[i - 1] = target;//将目标存入数组
                    }
                }
            }//遍历结束，获得所有目标
            for (var i = 0; i < targets.Length; i++)//遍历目标数组，从最近的目标开始攻击
            {
                if (targets[i] != null)//最近的目标不为空，则进行攻击
                {
                    GameManager.Instance.AttackSettlement(this.gameObject, targets[i]);//进行攻击
                    AnimaSet(targets[i]);
                    break;//跳出循环
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
