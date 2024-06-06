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

    public override void AttackJudg()
    {
        GameObject target;
        GameObject[] targets = new GameObject[3];//�󽣵Ĺ�����λ���Ϊ3��
        GameManager.Instance.unitesGridMap.GetGridXZ(this.gameObject.transform.position, out int x, out int z);
        if (isAttacker)//�ǹ�����
        {
            for (var i = 0; i < 3; i++)
            {
                target = GameManager.Instance.unitesGridMap.GetValue(x + UniteSave.Range, z + i);
                if (target != null)
                {
                    if (!target.GetComponent<BaseAction>().isAttacker)
                    {
                        targets[i] = target;
                    }
                }
            }
        }
        else//�Ƿ��ط�
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
        /* var sortedList = targets.OrderByDescending(kv => kv.GetComponent<BaseAction>().UniteSave.Defence) // ����key�Ĵ�С��������                                
                                 .ToArray();
         targets = sortedList;
         List<GameObject> targetList = new List<GameObject>(targets);
         targetList.RemoveAll(target => target == null);
         if (targetList.Count != 0)
         {
           GameManager.Instance.AttackSettlement(this.gameObject, targets);
           AnimaSet(targets);
         }*/

        List<GameObject> targetList = new List<GameObject>(targets);
        targetList.RemoveAll(target => target == null);

        if (targetList.Count != 0)
        {
            var sortedList = targetList
                .OrderByDescending(kv => kv.GetComponent<BaseAction>().UniteSave.Defence) // ����key�Ĵ�С��������                                
                .ToArray();

            targetList = new List<GameObject>(sortedList); // ����targetList��ȷ��ʹ���������б�

            GameManager.Instance.AttackSettlement(this.gameObject, targetList.ToArray());
            AnimaSet(targetList.ToArray());
        }
    }
    private void AnimaSet(GameObject[] target)
    {
        Spine2DSkinList spineA = this.GetComponent<Spine2DSkinList>();
        string atktracks = "Attacks/Queen_Attack";
        string targettracks = "Hit/Hit";
        spineA.SetAnimaList(atktracks);
        for (var i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Spine2DSkinList>().SetAnimaList(targettracks);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.MeleeAttack -= Attack;
    }

}
