using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAction : MonoBehaviour
{
    public Vector2 coordinate;
    public Unites UniteSave;
    public bool isAttacker;
    public bool canAttack;
    protected virtual void OnEnable()
    {
        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement|| TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)
        {
            isAttacker = true;
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement|| TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            isAttacker = false;
        }
        //��ȡ�Լ���λ�ã����Լ���λ����ӵ������б���
        UniteSave = this.GetComponent<PawnData>().Unites;
        EventManager.MoveReady += MoveReady;
        EventManager.Move += Move;
        EventManager.RoundEnd += RoundEnd;
        EventManager.PlayAnimation += AnimaPlay;
    }
    public void MoveReady()
    {
        GameManager.Instance.unitesGridMap.GetGridXZ(this.transform.position, out int x, out int z);
        if (isAttacker)
        { //ֻ�й�����λ�Ż���ӵ������б���
            GameManager.Instance.AttackMovelis.Add(new Vector2(x, z), this.gameObject);
        }
        else
        {//������λ��ӵ������б���
            GameManager.Instance.DefenceMovelis.Add(new Vector2(x, z), this.gameObject);
        }
    }
    public void Move()
    {
        //��ȡ��ǰ����
        GameManager.Instance.unitesGridMap.GetGridXZ(this.gameObject.transform.position, out int x, out int z);
        Vector2 point = new Vector2(x, z);
        //�����ǰ���겻����Ӧ���ƶ��������
        if (coordinate != point)
        {
            //�����ƶ�
            Vector3 target = GameManager.Instance.unitesGridMap.GetGridCenter((int)coordinate.x, (int)coordinate.y);
            this.transform.DOMove(target, 2);
        }
    }
    public void Attack()
    {
        GameManager.Instance.Attacklist.Add(new KeyValuePair<int, GameObject>(UniteSave.Speed, this.gameObject));
    }

    public virtual void AttackJudg()
    {

    }
    public void AnimaPlay()
    {
        this.GetComponent<Spine2DSkinList>().PlayAnimation();
    }
    private void RoundEnd()
    {
        this.GetComponent<PawnData>().Defence = this.GetComponent<PawnData>().Unites.Defence;
    }

    protected virtual void OnDisable()
    {
        Debug.Log(this.UniteSave.Name+"stop stop stop on disable"+" isattack:?"+ isAttacker);
        EventManager.MoveReady -= MoveReady;
        EventManager.Move -= Move;
        EventManager.RoundEnd -= RoundEnd;
        EventManager.PlayAnimation -= AnimaPlay;
      /*  GameManager.Instance.unitesGridMap.GetGridXZ(this.transform.position, out int x, out int z);
        if (isAttacker)
        {
        GameManager.Instance.AttackMovelis.Remove(new Vector2(x, z));
        }
        else if(!isAttacker)
        {
         GameManager.Instance.DefenceMovelis.Remove(new Vector2(x, z));
        }*/
    }
}
