using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    //��̬�� �κεط������Ե���
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
    //��λ���ĸ��ط��ƶ����������������������������¼�
    //����λ�ľ��幥�����򣬹�����ʽ������Ŀ�꣬������Χ�������˺�������Ч���������ڵ�λ�Ľű���ʵ�ֵ�
    //�ڵ�λ����ȡ�����ɵ�ʱ�����Ǿͻ������ע�ᵽ����¼���
    public static void OnMove()
    {//����ʹ����?.�����������Move��Ϊ�գ��͵���Move.Invoke()�����򲻵���
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
