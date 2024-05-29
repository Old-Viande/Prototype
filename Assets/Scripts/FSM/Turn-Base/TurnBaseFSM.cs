using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurnBaseFSM : Singleton<TurnBaseFSM>
{
    public Dictionary<States, IState> state = new Dictionary<States, IState>();
    //����һ���ֵ䣬key��States���ͣ�value��IState����
    //����ֵ�������Ǵ洢���е�״̬��key��״̬�����ͣ�value��״̬��ʵ��
    //�������ǾͿ���ͨ��״̬����������ȡ״̬��ʵ��
    public IState currentState;
    public States currentStateType;
    public int RoundCount = 0;

    protected virtual void Start()
    {
        /* state.Add(States.RoundStart, new StartState(this));
         state.Add(States.DrawPile, new AttackStartDrawPileState(this));
         state.Add(States.Configuration, new AttackConfigurationState(this));
         state.Add(States.BattleRound, new BattleRoundState(this));
         state.Add(States.Weather, new WeatherState(this));
         state.Add(States.RangedAttack, new RangeAttackState(this));
         state.Add(States.UnitMove, new UnitMoveState(this));
         state.Add(States.MeleeAttack, new MeleeAttackState(this));
         state.Add(States.EndRound, new EndRoundState(this));
         state.Add(States.Exit, new ExitState(this));*/
        state.Add(States.RoundStart, new StartRoundState(this));
        state.Add(States.DefenceDrawPile, new DefenceStartDrawPileState(this));
        state.Add(States.DefenceConfiguration, new DefenceConfigurationState(this));
        state.Add(States.DefencePlacement, new DefencePlacementState(this));
        state.Add(States.AttackDrawPile, new AttackStartDrawPileState(this));
        state.Add(States.AttackConfiguration, new AttackConfigurationState(this));
        state.Add(States.AttackPlacement, new AttackPlacementState(this));
        state.Add(States.BattleRound, new BattleRoundState(this));
        state.Add(States.WeatherRound, new WeatherState(this));
        state.Add(States.DefenceDrawRound, new DefenceDrawRoundState(this));
        state.Add(States.DefenceWithdrawn, new DefenceWithdrawnState(this));
        state.Add(States.AttackDrawRound, new AttackDrawRoundState(this));
        state.Add(States.AttackWithdrawn, new AttackWithdrawnState(this));
        state.Add(States.RangeAttack, new RangeAttackState(this));
        state.Add(States.PawnMove, new PawnMoveState(this));
        state.Add(States.MeleeAttack, new MeleeAttackState(this));
        state.Add(States.DefenceReinforce, new DefenceReinforceState(this));
        state.Add(States.AttackReinforce, new AttackReinforceState(this));
        state.Add(States.EndRound, new EndRoundState(this));
        
        ChangeState(States.RoundStart);
        // ChangeState(States.BattleRound);
    }


    private void Update()
    {
        //����ǰ״̬�����з������������
        currentState.OnUpdate();
    }

    public void ChangeState(States type)
    {
        if (currentStateType == type || type == States.Unknown)
        {
            return;
        }
        //��������������ı䵱ǰ״̬��
        if (currentStateType != States.Unknown)
            currentState.OnExit();

        currentStateType = type;
        currentState = state[type];
        currentState.OnEnter();
        //��������Ĵ��� currentState.OnExit();�ǵ��õ�ǰ״̬���˳�����
        //currentState = state[newState];�ǽ���ǰ״̬�ı�Ϊ�µ�״̬
        //currentState.OnEnter();�ǵ����µ�״̬�Ľ��뷽��
        //currentStateType = newState;�ǽ���ǰ״̬�����͸ı�Ϊ�µ�״̬������
    }

    public void Delay(float time, States type)
    {
        StartCoroutine(DelayCoroutine(time, type));
    }

    private IEnumerator DelayCoroutine(float time, States type)
    {
        Debug.Log("��ʼ����ʱ");
        yield return new WaitForSeconds(time);
        Debug.Log("����ʱ����");
        ChangeState(type);
    }
}