using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainPanelFun : Singleton<MainPanelFun>
{
    public GameObject RoundText;
    public GameObject CurentState;
    public GameObject NextState;
    public RectTransform atkPanelRect;
    public RectTransform defPanelRect;
    public TextMeshProUGUI atkInforRect;
    public TextMeshProUGUI defInforRect;
    public GameObject atkpanel;
    public GameObject defPanel;
    public GameObject atkContent;
    public GameObject defContent;
    public Button atkPanelClose;
    public Button defPanelClose;
    // public Dictionary<string, GameObject> pawnDic = new Dictionary<string, GameObject>();
    public Vector2 atkhiddenPosition;
    public Vector2 defhiddenPosition;
    public Vector2 atkvisiblePosition;
    public Vector2 defvisiblePosition;
    public float animationDuration = 0.5f;

    public bool atkPanelIsOpen;
    public bool defPanelIsOpen;

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
            if (atkPanelIsOpen)
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
            if (defPanelIsOpen)
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
    public void SidInfor()
    {
        atkInforRect.text = "Number of Pawn deployed on the Attacker :" + GameManager.Instance.atkPawnDic.Count + "\n" +
        "Number of Pawn defeated : <color=red>" + GameManager.Instance.atkPawnGrave.Count + "</color>\n" +
        "Number of Pawn from defeat :<color=red>" + (8 - GameManager.Instance.atkPawnGrave.Count);
        defInforRect.text = "Number of Pawn deployed on the Defender :" + GameManager.Instance.defPawnDic.Count + "\n" +
        "Number of Pawn defeated : <color=red>" + GameManager.Instance.defPawnGrave.Count + "</color>\n" +
        "Number of Pawn from defeat :<color=red>" + (8 - GameManager.Instance.defPawnGrave.Count);
    }
    public void PanelText()
    {
        if (TurnBaseFSM.Instance != null)
        {
            RoundText.GetComponent<TextMeshProUGUI>().text = "Round " + TurnBaseFSM.Instance.RoundCount;
            CurentState.GetComponent<TextMeshProUGUI>().text = "CurentState: " + TurnBaseFSM.Instance.currentStateType;
            NextState.GetComponent<TextMeshProUGUI>().text = "NextState: " + CheckNextState(TurnBaseFSM.Instance.currentStateType);
        }
    }
    public string CheckNextState(States currentStateType)
    {

        switch (currentStateType)
        {
            case States.PreRound:
                return "DrawPileRound";
            case States.AttackDrawRound:
                return "AttackConfiguration";
            case States.DefenceDrawRound:
                return "DefenceConfiguration";
            case States.RangeAttack:
                return "PawnMove";
            case States.PawnMove:
                return "MeleeAttack";
            case States.MeleeAttack:
                return "ReinforceRound";

        }
        return "Round waiting.";
    }
    public void PlacementFinish()
    {
        // 根据当前状态类型选择合适的保存方法
        //var savePawn = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
        //    ? (Action<string, GameObject>)GameManager.Instance.AttackPawnPoolSave
        //    : GameManager.Instance.DefencePawnPoolSave;
        Action<string, GameObject> savePawn;

        switch (TurnBaseFSM.Instance.currentStateType)
        {
            case States.AttackPlacement:
                savePawn = GameManager.Instance.AttackPawnPoolSave;
                break;
            case States.AttackReinforce:
                savePawn = GameManager.Instance.AttackPawnPoolSave;
                break;
            case States.DefencePlacement:
                savePawn = GameManager.Instance.DefencePawnPoolSave;
                break;
            case States.DefenceReinforce:
                savePawn = GameManager.Instance.DefencePawnPoolSave;
                break;
            default:
                savePawn = null; // 或者其他默认处理方式
                break;
        }
        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement || TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)
        {
            foreach (Transform item in atkContent.transform)
            {
                savePawn(item.gameObject.name, item.gameObject);
            }
            PawnPaneljump();//这里的代码应该是调用将面板弹回侧边的方法
            if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement)
            {

                TurnBaseFSM.Instance.ChangeState(States.DefenceDrawPile);
            }
            else
            {
                TurnBaseFSM.Instance.ChangeState(States.DefenceReinforce);
            }
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement || TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            foreach (Transform item in defContent.transform)
            {
                savePawn(item.gameObject.name, item.gameObject);
            }
            PawnPaneljump();//这里的代码应该是调用将面板弹回侧边的方法
            if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement)
            {
                TurnBaseFSM.Instance.ChangeState(States.BattleRound);
            }
            else
            {
                TurnBaseFSM.Instance.ChangeState(States.EndRound);
            }
        }
    }

}
