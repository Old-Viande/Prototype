using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�������һ��ö���࣬����������Ϸ��״̬
public enum States
{/*
    Unknown,//δ֪
    Starte,//������
    DrawPile,//����
    Configuration,//���õ�λ
    BattleRound,//ս���غϿ�ʼ
    Weather,//����
    //DrawCard,//�鿨
    //Withdraw,//���˵�λ
    RangedAttack,//Զ�̹���
    UnitMove,//��λ�ƶ�
    MeleeAttack,//��ս����
    //reinforce,//��Ԯ
    EndRound,//�غϽ���
    Exit,*/

    Unknown,//δ֪
    RoundStart,//�غϿ�ʼ
    DefenceDrawPile,//���������ֳ���
    DefenceConfiguration,//���������õ�λ
    DefencePlacement,//���������õ�λ
    AttackDrawPile,//���������ֳ���
    AttackConfiguration,//���������õ�λ
    AttackPlacement,//���������õ�λ
    BattleRound,//ս���غ�
    WeatherRound,//������ȡ
    DefenceDrawRound,//�������غ��ڳ���
    DefenceWithdrawn,//����������
    AttackDrawRound,//�������غ��ڳ���
    AttackWithdrawn,//����������
    RangeAttack,//Զ�̹���
    PawnMove,//ǰ�����غϽ���������λ�ƶ�
    MeleeAttack,//��ս����
    DefenceReinforce,//��������Ԯ
    AttackReinforce,//��������Ԯ
    EndRound,//�غϽ���,�ж���ɺ������һ�غ�
    Exit,

}
