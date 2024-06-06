using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleRoundState : IState
{
    private TurnBaseFSM fsm;
    public BattleRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.SetInfPanel("Now the battle begins.");

        fsm.Delay(States.PreRound);
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}

public class WeatherState : IState
{
    private TurnBaseFSM fsm;
    public WeatherState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {

        GameManager.Instance.RandomWeather();
        //fsm.Delay(States.RangeAttack);
        fsm.Delay(States.AttackDrawRound);//等待完成后解除注释
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
public class RangeAttackState : IState
{
    private TurnBaseFSM fsm;
    public RangeAttackState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        Debug.Log("远程攻击开始");
        GameManager.Instance.Attacklist.Clear();
        EventManager.OnRangAttack();
        GameManager.Instance.AttackFollowOrder();
        EventManager.OnPlayAnimation();
        EventManager.OnBloodBarChange();
        //fsm.ChangeState(States.PawnMove);
        fsm.Delay(States.PawnMove);
    }

    public void OnUpdate()
    {


    }

    public void OnExit()
    {
        GameManager.Instance.UniteRoundEnd();
        Debug.Log("远程攻击结束");
    }
}
public class PawnMoveState : IState
{
    private TurnBaseFSM fsm;
    public PawnMoveState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {

        /* TextShow.Instance.AddText("UnitMoveState OnEnter");
         TextShow.Instance.AddText("Detection of units that can be moved");*/
        EventManager.OnMoveReady();
        GameManager.Instance.AttackPawnMoveOrder();
        GameManager.Instance.DefencePawnMoveOrder();
        EventManager.OnMove();
        fsm.Delay(States.MeleeAttack);
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        GameManager.Instance.ResetMoveList();
    }
}
public class MeleeAttackState : IState
{
    private TurnBaseFSM fsm;
    public MeleeAttackState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {

        GameManager.Instance.Attacklist.Clear();
        EventManager.OnMeleeAttack();
        GameManager.Instance.AttackFollowOrder();
        EventManager.OnPlayAnimation();
        EventManager.OnBloodBarChange();
        fsm.Delay(States.AttackReinforce);      
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        GameManager.Instance.UniteRoundEnd();
    }
}
public class EndRoundState : IState
{
    private TurnBaseFSM fsm;
    public EndRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        if (GameManager.Instance.atkPawnGrave.Count >= 8)
        {
            fsm.Delay(States.Exit);
            fsm.isAtkWin = false;
        }
        else if (GameManager.Instance.defPawnGrave.Count >= 8)
        {
            fsm.Delay(States.Exit);
            fsm.isAtkWin = true;
        }
        else
        {
            EventManager.OnRoundEnd();
            EventManager.OnBloodBarChange();
            fsm.Delay(States.PreRound);
        }

        /*if (GameManager.Instance.weatheres.Count >= 7)
        {
            fsm.Delay(States.Exit);
        }*/

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        fsm.RoundCount++;
    }
}
public class ExitState : IState
{
    private TurnBaseFSM fsm;
    public ExitState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        if (fsm.isAtkWin)
        {
            StartManager.Instance.OpenEndPanel();
            EndPanelFun.Instance.SetEndText(true);
        }
        else
        {
            StartManager.Instance.OpenEndPanel();
            EndPanelFun.Instance.SetEndText(false);
        }
        //Application.Quit();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}

public class DefenceConfigurationRoundState : IState
{
    private TurnBaseFSM fsm;
    public DefenceConfigurationRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenConfig();
        ConfigFun.Instance.UIStock();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenMainPanel();
    }
}
public class DefenceReinforceState : IState
{
    private TurnBaseFSM fsm;
    public DefenceReinforceState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        MainPanelFun.Instance.PawnPanelInit();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
public class AttackConfigurationRoundState : IState
{
    private TurnBaseFSM fsm;
    public AttackConfigurationRoundState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        StartManager.Instance.OpenConfig();
        ConfigFun.Instance.UIStock();

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
public class AttackReinforceState : IState
{
    private TurnBaseFSM fsm;
    public AttackReinforceState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        StartManager.Instance.ClosePanel();
        if (GameManager.Instance.atkPawnSave.Count == 0)
        {
            fsm.Delay(States.Exit);
            fsm.isAtkWin = false;
        }
        else if (GameManager.Instance.defPawnSave.Count == 0)
        {
            fsm.Delay(States.Exit);
            fsm.isAtkWin = true;
        }        
        StartManager.Instance.OpenMainPanel();
        MainPanelFun.Instance.PawnPanelInit();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
