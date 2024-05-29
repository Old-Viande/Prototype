using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个类是一个枚举类，用来定义游戏的状态
public enum States
{/*
    Unknown,//未知
    Starte,//玩家身份
    DrawPile,//抽牌
    Configuration,//配置单位
    BattleRound,//战斗回合开始
    Weather,//天气
    //DrawCard,//抽卡
    //Withdraw,//撤退单位
    RangedAttack,//远程攻击
    UnitMove,//单位移动
    MeleeAttack,//近战攻击
    //reinforce,//增援
    EndRound,//回合结束
    Exit,*/

    Unknown,//未知
    RoundStart,//回合开始
    DefenceDrawPile,//防御方开局抽牌
    DefenceConfiguration,//防御方配置单位
    DefencePlacement,//防御方放置单位
    AttackDrawPile,//进攻方开局抽牌
    AttackConfiguration,//进攻方配置单位
    AttackPlacement,//进攻方放置单位
    BattleRound,//战斗回合
    WeatherRound,//天气抽取
    DefenceDrawRound,//防御方回合内抽牌
    DefenceWithdrawn,//防御方撤退
    AttackDrawRound,//进攻方回合内抽牌
    AttackWithdrawn,//进攻方撤退
    RangeAttack,//远程攻击
    PawnMove,//前三个回合仅攻击方单位移动
    MeleeAttack,//近战攻击
    DefenceReinforce,//防御方增援
    AttackReinforce,//进攻方增援
    EndRound,//回合结束,判定完成后进入下一回合
    Exit,

}
