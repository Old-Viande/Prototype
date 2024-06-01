using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelFun : Singleton<MainPanelFun>
{
    public GameObject RoundPanel;
    public RectTransform atkPanelRect;
    public RectTransform defPanelRect;
    public GameObject atkpanel;
    public GameObject defPanel;
    public GameObject atkContent;
    public GameObject defContent;
    public Button atkPanelClose;
    public Button defPanelClose;
    public Dictionary<string, GameObject> pawnDic = new Dictionary<string, GameObject>();
    public Vector2 atkhiddenPosition;
    public Vector2 defhiddenPosition;
    public Vector2 atkvisiblePosition;
    public Vector2 defvisiblePosition;
    public float animationDuration = 0.5f;

    public bool atkPanelIsOpen;
    public bool defPanelIsOpen;

    private void OnEnable()
    {
       /* if (atkContent != null)
        {
            // 激活面板以及 pawnpanel 面板中的所有子物体
            atkContent.SetActive(true);
            foreach (Transform item in atkContent.transform)
            {
                item.gameObject.SetActive(true);
            }
        }
        if (defContent != null)
        {
            // 激活面板以及 pawnpanel 面板中的所有子物体
            defContent.SetActive(true);
            foreach (Transform item in defContent.transform)
            {
                item.gameObject.SetActive(true);

            }
        }*/
    }
    public void Start()
    {
        atkPanelIsOpen = true;
        defPanelIsOpen = true;
        atkPanelClose.onClick.AddListener(PlacementFinish);
        defPanelClose.onClick.AddListener(PlacementFinish);
        PanelInit();
        PawnPanelInit();
    }
    private void PanelInit()
    {
        atkhiddenPosition = new Vector2(atkPanelRect.anchoredPosition.x, atkPanelRect.anchoredPosition.y);
        defhiddenPosition = new Vector2(defPanelRect.anchoredPosition.x, defPanelRect.anchoredPosition.y);
        atkvisiblePosition = new Vector2(-atkPanelRect.anchoredPosition.x, atkPanelRect.anchoredPosition.y);
        defvisiblePosition = new Vector2(-defPanelRect.anchoredPosition.x, defPanelRect.anchoredPosition.y);

        //atkPanelRect.anchoredPosition = atkhiddenPosition;
        //defPanelRect.anchoredPosition = defhiddenPosition;
    }

    public void PawnPanelInit()
    {

        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement || TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)//如果当前状态是攻击方配置单位或者攻击方增援回合时，
        {
            foreach (var item in GameManager.Instance.AttackPawnPool.poolDictionary)
            {
                GameObject obj = GameManager.Instance.AttackPawnPoolGet(item.Key);
                obj.transform.SetParent(atkContent.transform);
            }
            //生成可拖拽的棋子后，清空对象池
            GameManager.Instance.AttackPawnPool.ClearPool();

        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement || TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            foreach (var item in GameManager.Instance.DefencePawnPool.poolDictionary)
            {
                GameObject obj = GameManager.Instance.DefencePawnPoolGet(item.Key);
                obj.transform.SetParent(defContent.transform);
            }
            //生成可拖拽的棋子后，清空对象池
            GameManager.Instance.DefencePawnPool.ClearPool();
        }
        PawnPaneljump();
    }
    public void PawnPaneljump()
    {
        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement || TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)
        {
            if(atkPanelIsOpen)
            {
                atkPanelRect.DOAnchorPos(atkvisiblePosition, animationDuration).SetEase(Ease.InOutBack);
                atkPanelIsOpen = false;
            }
            else
            {
                atkPanelRect.DOAnchorPos(atkhiddenPosition, animationDuration).SetEase(Ease.InOutBack);
                atkPanelIsOpen = true;
            }
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement || TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            if(defPanelIsOpen)
            {
                defPanelRect.DOAnchorPos(defvisiblePosition, animationDuration).SetEase(Ease.InOutBack);
                defPanelIsOpen = false;
            }
            else
            {
                defPanelRect.DOAnchorPos(defhiddenPosition, animationDuration).SetEase(Ease.InOutBack);
                defPanelIsOpen = true;
            }
        }
    }

    public void PlacementFinish()
    {
        // 根据当前状态类型选择合适的保存方法
        var savePawn = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
            ? (Action<string, GameObject>)GameManager.Instance.AttackPawnPoolSave
            : GameManager.Instance.DefencePawnPoolSave;
        // 遍历 pawnpanel 中的每个子物体，并调用保存方法


        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement || TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)
        {
            foreach (Transform item in atkContent.transform)
            {
                savePawn(item.gameObject.name, item.gameObject);
            }
            PawnPaneljump();//这里的代码应该是调用将面板弹回侧边的方法
            TurnBaseFSM.Instance.ChangeState(States.DefenceDrawPile);
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement || TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            foreach (Transform item in defContent.transform)
            {
                savePawn(item.gameObject.name, item.gameObject);
            }
            PawnPaneljump();//这里的代码应该是调用将面板弹回侧边的方法
            TurnBaseFSM.Instance.ChangeState(States.BattleRound);
        }
    }

}
