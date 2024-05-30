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
        Debug.Log("BattleRoundState OnEnter");
        TextShow.Instance.AddText("WeatherState OnEnter");
        GameManager.Instance.RandomWeather();
        foreach (var item in GameManager.Instance.weatheres)
        {
            TextShow.Instance.AddText(item.ToString());
        }
        fsm.Delay(7, States.RangeAttack);
    }

    public void OnUpdate()
    {
        Debug.Log("BattleRoundState OnUpdate");
    }

    public void OnExit()
    {
        Debug.Log("BattleRoundState OnExit");
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
        EventManager.OnRangAttack();
        fsm.Delay(5, States.PawnMove);
    }

    public void OnUpdate()
    {
      
        Debug.Log("远程攻击开始");
    }

    public void OnExit()
    {
        Debug.Log("RangeAttackState OnExit");
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
        Debug.Log("UnitMoveState OnEnter");
        TextShow.Instance.AddText("UnitMoveState OnEnter");
        TextShow.Instance.AddText("Detection of units that can be moved");
        fsm.Delay(5, States.MeleeAttack);
    }

    public void OnUpdate()
    {
        Debug.Log("UnitMoveState OnUpdate");
       
        
    }

    public void OnExit()
    {
        Debug.Log("UnitMoveState OnExit");
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
    {   TextShow.Instance.AddText("end of movement");
        Debug.Log("MeleeAttackState OnEnter");
        TextShow.Instance.AddText("MeleeAttackState OnEnter");
        TextShow.Instance.AddText("Melee units that come into range start attacking");
        EventManager.OnMeleeAttack();
        fsm.Delay(7, States.EndRound);
    }

    public void OnUpdate()
    {
        Debug.Log("MeleeAttackState OnUpdate");
    }

    public void OnExit()
    {
        Debug.Log("MeleeAttackState OnExit");
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
        Debug.Log("EndRoundState OnEnter");
        TextShow.Instance.AddText("EndRoundState OnEnter");
        TextShow.Instance.AddText("Detection end judgment");
        if (GameManager.Instance.weatheres.Count >= 4)
        {
            TextShow.Instance.AddText("The weather is too bad, the battle is over");
            //延时四秒后游戏退出
            fsm.Delay(4, States.Exit);

        }
        else
        {
            fsm.Delay(4, States.WeatherRound);
        }
    }

    public void OnUpdate()
    {
        Debug.Log("EndRoundState OnUpdate");
        Debug.Log("检测场上是否还有单位");
        Debug.Log("检测是否有玩家胜利");
        Debug.Log("检测是否有AI胜利");
        Debug.Log("检测是否平局");
        Debug.Log("如果没有胜利者，进入下一回合");
        
    }

    public void OnExit()
    {
        Debug.Log("EndRoundState OnExit");
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
        Debug.Log("ExitState OnEnter");       
        TextShow.Instance.AddText("Game Over");        
        Application.Quit();
    }

    public void OnUpdate()
    {
        Debug.Log("ExitState OnUpdate");
    }

    public void OnExit()
    {
        Debug.Log("ExitState OnExit");
    }
}

public class DefenceReinforceState:IState
{
    private TurnBaseFSM fsm;
    public DefenceReinforceState(TurnBaseFSM fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        Debug.Log("DefenceReinforceState OnEnter");
        TextShow.Instance.AddText("DefenceReinforceState OnEnter");
        TextShow.Instance.AddText("DefenceReinforceState OnEnter");
        TextShow.Instance.AddText("DefenceReinforceState OnEnter");
        fsm.Delay(4, States.AttackReinforce);
    }

    public void OnUpdate()
    {
        Debug.Log("DefenceReinforceState OnUpdate");
    }

    public void OnExit()
    {
        Debug.Log("DefenceReinforceState OnExit");
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
        Debug.Log("AttackReinforceState OnEnter");
        TextShow.Instance.AddText("AttackReinforceState OnEnter");
        TextShow.Instance.AddText("AttackReinforceState OnEnter");
        TextShow.Instance.AddText("AttackReinforceState OnEnter");
        fsm.Delay(4, States.EndRound);
    }

    public void OnUpdate()
    {
        Debug.Log("AttackReinforceState OnUpdate");
    }

    public void OnExit()
    {
        Debug.Log("AttackReinforceState OnExit");
    }
}
