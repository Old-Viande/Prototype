using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GreatSword : BaseAction
{

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.MeleeAttack += Attack;
    }
    // Start is called before the first frame update
    void Start()
    {
       // UniteSave = this.GetComponent<PawnData>().Unites;
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
        GameObject[] targets = new GameObject[3];//大剑的攻击单位最多为3个
        GameManager.Instance.unitesGridMap.GetGridXZ(this.gameObject.transform.position, out int x, out int z);
        if (isAttacker)//是攻击方
        {
            for(var i = 0; i < 3; i++)
            {
               target = GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z + i);
                if(target != null)
                {
                    if (target.GetComponent<BaseAction>().isAttacker!)
                    {
                        targets[i] = target;
                    }
                }
            }
        }
        else//是防守方
        {
            for (var i = 0; i < 3; i++)
            {
                target = GameManager.Instance.unitesGridMap.GetValue(x - UniteSave.Range, z + i);
                if (target != null)
                {
                    if (target.GetComponent<BaseAction>().isAttacker)
                    {
                        targets[i] = target;
                    }
                }
            }
        }

        var sortedList = targets.OrderByDescending(kv => kv.GetComponent<BaseAction>().UniteSave.Defence) // 按照key的大小进行排序                                
                                  .ToArray();
        targets = sortedList;
        List<GameObject> targetList = new List<GameObject>(targets);
        targetList.RemoveAll(target => target == null);
        if (targetList.Count != 0)
        {
          GameManager.Instance.AttackSettlement(this.gameObject, targets);
          AnimaSet(targets);
        }
    }
    private void AnimaSet(GameObject[] target)
    {
        Spine2DSkinList spineA = this.GetComponent<Spine2DSkinList>();
        string[] atktracks = new string[] { "Attacks/Queen_Attack" };
        string[] targettracks = new string[] { "Hit/Hit" };
        spineA.SetAnimation(atktracks);
        for(var i = 0; i < target.Length; i++)
        {
          target[i].GetComponent<Spine2DSkinList>().SetAnimation(targettracks);           
        }
    }

    protected override void OnDisable()
    {
        EventManager.MeleeAttack -= Attack;
    }

}
