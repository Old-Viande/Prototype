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
       
        fsm.Delay(5, States.WeatherRound);
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

        fsm.Delay(7, States.RangeAttack);
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
        EventManager.OnBloodBarChange();
        //fsm.ChangeState(States.PawnMove);
        fsm.Delay(3, States.PawnMove);
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
        GameManager.Instance.AttackPawnMoveOrder();
        GameManager.Instance.DefencePawnMoveOrder();
        EventManager.OnMove();
        fsm.Delay(3, States.MeleeAttack);
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
    
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
        EventManager.OnBloodBarChange();
        fsm.Delay(3, States.EndRound);
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


        if (GameManager.Instance.weatheres.Count >= 7)
        {
            fsm.Delay(4, States.Exit);

        }
        else
        {
            EventManager.OnRoundEnd();
            EventManager.OnBloodBarChange();
            fsm.Delay(4, States.WeatherRound);
        }

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
     
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
      
        Application.Quit();
    }

    public void OnUpdate()
    {
       
    }

    public void OnExit()
    {
        
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
       
        fsm.Delay(4, States.AttackReinforce);
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
        
        fsm.Delay(4, States.EndRound);
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
      
    }
}
