using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void EventMove()
    {
        GameManager.Instance.AttackPawnMoveOrder();
        GameManager.Instance.DefencePawnMoveOrder();
        EventManager.OnMove();
    }
    public void EventBlood()
    {
        EventManager.OnBloodBarChange();
    }
    public void EventWithdraw()
    {
        EventManager.OnWithdraw();
    }
    public void drewcard()
    {
        //GameManager.Instance.CardPealDrew(GameManager.Instance.uniteTypeSO);
    }
    public void ClosePanel()
    {
        StartManager.Instance.ClosePanel();
    }
}
