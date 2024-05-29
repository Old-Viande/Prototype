using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private void Attack()
    {
        GameManager.Instance.Attacklis.Add(UniteSave.Speed, this.gameObject);
        AttackJudg();
    }

    private void AttackJudg()
    {
        throw new NotImplementedException();
    }

    protected override void OnDisable()
    {
        EventManager.MeleeAttack -= Attack;
    }
}