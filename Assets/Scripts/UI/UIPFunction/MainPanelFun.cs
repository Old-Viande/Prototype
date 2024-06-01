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
            // ��������Լ� pawnpanel ����е�����������
            atkContent.SetActive(true);
            foreach (Transform item in atkContent.transform)
            {
                item.gameObject.SetActive(true);
            }
        }
        if (defContent != null)
        {
            // ��������Լ� pawnpanel ����е�����������
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

        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement || TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)//�����ǰ״̬�ǹ��������õ�λ���߹�������Ԯ�غ�ʱ��
        {
            foreach (var item in GameManager.Instance.AttackPawnPool.poolDictionary)
            {
                GameObject obj = GameManager.Instance.AttackPawnPoolGet(item.Key);
                obj.transform.SetParent(atkContent.transform);
            }
            //���ɿ���ק�����Ӻ���ն����
            GameManager.Instance.AttackPawnPool.ClearPool();

        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement || TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            foreach (var item in GameManager.Instance.DefencePawnPool.poolDictionary)
            {
                GameObject obj = GameManager.Instance.DefencePawnPoolGet(item.Key);
                obj.transform.SetParent(defContent.transform);
            }
            //���ɿ���ק�����Ӻ���ն����
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
        // ���ݵ�ǰ״̬����ѡ����ʵı��淽��
        var savePawn = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
            ? (Action<string, GameObject>)GameManager.Instance.AttackPawnPoolSave
            : GameManager.Instance.DefencePawnPoolSave;
        // ���� pawnpanel �е�ÿ�������壬�����ñ��淽��


        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement || TurnBaseFSM.Instance.currentStateType == States.AttackReinforce)
        {
            foreach (Transform item in atkContent.transform)
            {
                savePawn(item.gameObject.name, item.gameObject);
            }
            PawnPaneljump();//����Ĵ���Ӧ���ǵ��ý���嵯�ز�ߵķ���
            TurnBaseFSM.Instance.ChangeState(States.DefenceDrawPile);
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement || TurnBaseFSM.Instance.currentStateType == States.DefenceReinforce)
        {
            foreach (Transform item in defContent.transform)
            {
                savePawn(item.gameObject.name, item.gameObject);
            }
            PawnPaneljump();//����Ĵ���Ӧ���ǵ��ý���嵯�ز�ߵķ���
            TurnBaseFSM.Instance.ChangeState(States.BattleRound);
        }
    }

}
