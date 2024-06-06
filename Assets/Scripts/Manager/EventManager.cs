using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    //静态类 任何地方都可以调用
    public static event Action MoveReady;
    public static event Action Move;
    public static event Action Withdraw;
    public static event Action Rollover;
    public static event Action RangAttack;
    public static event Action MeleeAttack;
    public static event Action RemoveUnits;
    public static event Action RoundEnd;
    public static event Action BloodBarChange;
    public static event Action PlayAnimation;
    //单位在哪个地方移动，攻击，被攻击，都会调用这个事件
    //但单位的具体攻击倾向，攻击方式，攻击目标，攻击范围，攻击伤害，攻击效果，都是在单位的脚本中实现的
    //在单位被抽取并生成的时候，它们就会把自身注册到这个事件中
    public static void OnMove()
    {//这里使用了?.操作符，如果Move不为空，就调用Move.Invoke()，否则不调用
        Move?.Invoke();
    }
    public static void OnWithdraw()
    {
        Withdraw?.Invoke();
    }
    public static void OnRollover()
    {
        Rollover?.Invoke();
    }
    public static void OnRangAttack()
    {
        RangAttack?.Invoke();
    }
    public static void OnMeleeAttack()
    {
        MeleeAttack?.Invoke();
    }
    public static void OnRemoveUnits()
    {
        RemoveUnits?.Invoke();
    }
    public static void OnRoundEnd()
    {
        RoundEnd?.Invoke();
    }
    public static void OnBloodBarChange()
    {
        BloodBarChange?.Invoke();
    }
    public static void OnMoveReady()
    {
        MoveReady?.Invoke();
    }
    public static void OnPlayAnimation()
    {
        PlayAnimation?.Invoke();        
    }
}
