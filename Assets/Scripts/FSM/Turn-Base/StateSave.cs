using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class StartRoundState : IState
{

    private TurnBaseFSM fsm;
    public StartRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.OpenStartPanel();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现游戏开始的功能
public class AttackStartDrawPileState : IState
{
    //在这个状态类中，我们将实现初始抽牌的功能，玩家可以从两个牌库中一共抽取八张牌
    private TurnBaseFSM fsm;

    public AttackStartDrawPileState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenDraw();
        StartManager.Instance.SetInfPanel("Welcome to the start of the game, this is the card draw phase, please click on the two middle card piles on the left side to get unit cards " +
            "and armor cards, you can draw eight in total.\r\nAt the end click on the Finish button.");
        Debug.Log("AttackStartDrawPileState OnEnter");
        CardDrawFun.Instance.DrawTimes = 8;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现进攻方开局抽牌的功能

public class AttackConfigurationState : IState
{
    private TurnBaseFSM fsm;

    public AttackConfigurationState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenConfig();
        StartManager.Instance.SetInfPanel("Here is the configuration panel, drag and drop the card you drew earlier into the panel at the bottom center " +
            "and click Finish Configuring Units.\r\nClick the Done button at the bottom when all the configurations are done.");
        Debug.Log("AttackConfigurationState OnEnter");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现进攻方配置单位的功能

public class AttackPlacementState : IState
{
    private TurnBaseFSM fsm;

    public AttackPlacementState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenMainPanel();
        MainPanelFun.Instance.PawnPanelInit();
        StartManager.Instance.SetInfPanel("This is the placement phase, drag and drop the unit card you configured earlier into the grid on the right side of the screen.\r\n" +
                       "Click the Finish button at the bottom when all the units are placed.");
        Debug.Log("AttackPlacementState OnEnter");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}   //这个类是一个状态类，用来实现进攻方放置单位的功能

public class AttackDrawRoundState : IState
{
    private TurnBaseFSM fsm;

    public AttackDrawRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.SetInfPanel("This is the draw card phase, click on the card pile on the left side to get unit cards and armor cards.\r\n" +
                                  "Click the Finish button at the bottom when all the cards are drawn.");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现进攻方回合内抽牌的功能
public class AttackWithdrawnState : IState
{
    private TurnBaseFSM fsm;

    public AttackWithdrawnState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.SetInfPanel("This is the withdrawal phase, drag and drop the unit card you want to withdraw into the panel at the bottom center.\r\n" +
                       "Click the Finish button at the bottom when all the units are withdrawn.");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现进攻方撤退的功能

public class DefenceStartDrawPileState : IState
{
    //在这个状态类中，我们将实现初始抽牌的功能，玩家可以从两个牌库中一共抽取八张牌
    private TurnBaseFSM fsm;

    public DefenceStartDrawPileState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenDraw();
        StartManager.Instance.SetInfPanel("Welcome to the start of the game, this is the card draw phase, please click on the two middle card piles on the left side to get unit cards " +
            "and armor cards, you can draw eight in total.\r\nAt the end click on the Finish button.");
        CardDrawFun.Instance.DrawTimes = 8;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现防御方开局抽牌的功能

public class DefenceConfigurationState : IState
{
    private TurnBaseFSM fsm;

    public DefenceConfigurationState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenConfig();
        StartManager.Instance.SetInfPanel("Here is the configuration panel, drag and drop the card you drew earlier into the panel at the bottom center " +
            "and click Finish Configuring Units.\r\nClick the Done button at the bottom when all the configurations are done.");
        ConfigFun.Instance.UIStock();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现防御方配置单位的功能

public class DefencePlacementState : IState
{
    private TurnBaseFSM fsm;

    public DefencePlacementState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenMainPanel();
        MainPanelFun.Instance.PawnPanelInit();
        StartManager.Instance.SetInfPanel("This is the placement phase, drag and drop the unit card you configured earlier into the grid on the right side of the screen.\r\n" +
                                  "Click the Finish button at the bottom when all the units are placed.");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}// 这个类是一个状态类，用来实现防御方放置单位的功能

public class DefenceDrawRoundState : IState
{
    private TurnBaseFSM fsm;

    public DefenceDrawRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.SetInfPanel("This is the draw card phase, click on the card pile on the left side to get unit cards and armor cards.\r\n" +
                                             "Click the Finish button at the bottom when all the cards are drawn.");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现防御方回合内抽牌的功能

public class DefenceWithdrawnState : IState
{
    private TurnBaseFSM fsm;

    public DefenceWithdrawnState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.SetInfPanel("This is the withdrawal phase, drag and drop the unit card you want to withdraw into the panel at the bottom center.\r\n" +
                                  "Click the Finish button at the bottom when all the units are withdrawn.");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}//这个类是一个状态类，用来实现防御方撤退的功能

