using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurnBaseFSM : Singleton<TurnBaseFSM>
{
    public Dictionary<States, IState> state = new Dictionary<States, IState>();
    //这是一个字典，key是States类型，value是IState类型
    //这个字典的作用是存储所有的状态，key是状态的类型，value是状态的实例
    //这样我们就可以通过状态的类型来获取状态的实例
    public IState currentState;
    public States currentStateType;
    public int RoundCount = 1;
    public int DeTime = 2;
    public bool  isAtkWin = false;
    protected virtual void Start()
    {     
        state.Add(States.RoundStart, new StartRoundState(this));
        state.Add(States.DefenceDrawPile, new DefenceStartDrawPileState(this));
        state.Add(States.DefenceConfiguration, new DefenceConfigurationState(this));
        state.Add(States.DefencePlacement, new DefencePlacementState(this));
        state.Add(States.AttackDrawPile, new AttackStartDrawPileState(this));
        state.Add(States.AttackConfiguration, new AttackConfigurationState(this));
        state.Add(States.AttackPlacement, new AttackPlacementState(this));
        state.Add(States.BattleRound, new BattleRoundState(this));
        state.Add(States.PreRound, new WeatherState(this));
        state.Add(States.DefenceDrawRound, new DefenceDrawRoundState(this));
       // state.Add(States.DefenceWithdrawn, new DefenceWithdrawnState(this));
        state.Add(States.AttackDrawRound, new AttackDrawRoundState(this));
      //  state.Add(States.AttackWithdrawn, new AttackWithdrawnState(this));
        state.Add(States.RangeAttack, new RangeAttackState(this));
        state.Add(States.PawnMove, new PawnMoveState(this));
        state.Add(States.MeleeAttack, new MeleeAttackState(this));
        state.Add(States.DefenceReinforce, new DefenceReinforceState(this));
        state.Add(States.AttackReinforce, new AttackReinforceState(this));
        state.Add(States.EndRound, new EndRoundState(this));
        state.Add(States.AttackConfigurationRound, new AttackConfigurationRoundState(this));
        state.Add(States.DefenceConfigurationRound, new DefenceConfigurationRoundState(this));
        state.Add(States.Exit, new ExitState(this));
        ChangeState(States.RoundStart);

    }


    private void Update()
    {
        //将当前状态的运行方法在这里调用
        currentState.OnUpdate();
    }

    public void ChangeState(States type)
    {
        if (currentStateType == type || type == States.Unknown)
        {
            return;
        }
        //这个方法是用来改变当前状态的
        if (currentStateType != States.Unknown)
            currentState.OnExit();

        currentStateType = type;
        currentState = state[type];
        if (MainPanelFun.Instance != null) 
        {
           MainPanelFun.Instance.PanelText();
           MainPanelFun.Instance.SidInfor();
        }
        currentState.OnEnter();
        //解释这里的代码 currentState.OnExit();是调用当前状态的退出方法
        //currentState = state[newState];是将当前状态改变为新的状态
        //currentState.OnEnter();是调用新的状态的进入方法
        //currentStateType = newState;是将当前状态的类型改变为新的状态的类型
    }

    public void Delay(States type)
    {
        StartCoroutine(DelayCoroutine(DeTime, type));
    }

    private IEnumerator DelayCoroutine(float time, States type)
    {
        Debug.Log("开始倒计时");
        yield return new WaitForSeconds(time);
        Debug.Log("倒计时结束");
        ChangeState(type);
    }
}
