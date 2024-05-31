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
        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement)
        {
            isAttacker = true;
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement)
        {
            isAttacker = false;
        }
        //��ȡ�Լ���λ�ã����Լ���λ����ӵ������б���
        UniteSave = this.GetComponent<PawnData>().Unites;
        GameManager.Instance.unitesGridMap.GetGridXZ(this.transform.position, out int x, out int z);
        if (isAttacker)
        { //ֻ�й�����λ�Ż���ӵ������б���
            GameManager.Instance.AttackMovelis.Add(new Vector2(x, z), this.gameObject);
        }
        else
        {//������λ��ӵ������б���
            GameManager.Instance.DefenceMovelis.Add(new Vector2(x, z), this.gameObject);
        }
        EventManager.Move += Move;
        EventManager.RoundEnd += RoundEnd;
       // EventManager.Idel += SetIdle;
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

    //public void SetIdle()
    //{
    //    string[] idel = { "Idel/Idel" };
    //    this.GetComponent<Spine2DSkinList>().SetAnimation(idel, true);
    //}
    public void Attack()
    {
        GameManager.Instance.Attacklist.Add(new KeyValuePair<int, GameObject>(UniteSave.Speed, this.gameObject));
    }

    public virtual void AttackJudg()
    {

    }
    private void RoundEnd()
    {
        this.GetComponent<PawnData>().Defence = this.GetComponent<PawnData>().Unites.Defence;
    }

    protected virtual void OnDisable()
    {
        EventManager.Move -= Move;
        EventManager.RoundEnd -= RoundEnd;
        GameManager.Instance.unitesGridMap.GetGridXZ(this.transform.position, out int x, out int z);
        if (isAttacker)
        {
        GameManager.Instance.AttackMovelis.Remove(new Vector2(x, z));
        }
        else
        {
         GameManager.Instance.DefenceMovelis.Remove(new Vector2(x, z));
        }
    }
}
