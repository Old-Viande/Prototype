using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelFun : Singleton<MainPanelFun>
{
    public GameObject pawnpanel;
    public GameObject pcontent;
    public Button placementBtn;
    public Dictionary<string, GameObject> pawnDic = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        if (pawnpanel != null)
        {
            // ��������Լ� pawnpanel ����е�����������
            pawnpanel.SetActive(true);
            foreach (Transform item in pawnpanel.transform)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
    public void Start()
    {
        pcontent = UITool.Instance.FindDeepChild("PContent");
        pawnpanel = UITool.Instance.FindDeepChild("PawnPanel");
        placementBtn.onClick.AddListener(PlacementFinish);
        PawnPanelInit();
    }
    /* public void PawnPanelInit()
     {
         if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement)
         {
             foreach (var item in GameManager.Instance.AttackPawnPool.poolDictionary)
             {
                 GameObject obj = GameManager.Instance.AttackPawnPoolGet(item.Key);
                 obj.transform.SetParent(pcontent.transform);
             }
             //���ɿ���ק�����Ӻ���ն����
             GameManager.Instance.AttackPawnPool.ClearPool();

         }
         else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement)
         {
             foreach (var item in GameManager.Instance.DefencePawnPool.poolDictionary)
             {
                 GameObject obj = GameManager.Instance.DefencePawnPoolGet(item.Key);
                 obj.transform.SetParent(pcontent.transform);
             }
             //���ɿ���ק�����Ӻ���ն����
             GameManager.Instance.DefencePawnPool.ClearPool();
         }
     }*/
    public void PawnPanelInit()
    {
        // ���ݵ�ǰ״̬����ѡ����ʵ����ӳغͻ�ȡ����
        var (pawnPool, getPawn) = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
            ? (GameManager.Instance.AttackPawnPool, (Func<string, GameObject>)GameManager.Instance.AttackPawnPoolGet)
            : (GameManager.Instance.DefencePawnPool, GameManager.Instance.DefencePawnPoolGet);

        // �������ӳ��е����ж��󣬲�������ӵ� pcontent ���
        foreach (var item in pawnPool.poolDictionary)
        {
            GameObject obj = getPawn(item.Key);
            obj.transform.SetParent(pcontent.transform);
        }

        // ���ɿ���ק�����Ӻ���ն����
        pawnPool.ClearPool();
    }
    /*public void PlacementFinish()
    {
        
        foreach (Transform item in pawnpanel.transform)
        {
            GameManager.Instance.AttackPawnPoolSave(item.gameObject.name, item.gameObject);
        }
        pawnpanel.gameObject.SetActive(false);
    }*/
    public void PlacementFinish()
    {
        // ���ݵ�ǰ״̬����ѡ����ʵı��淽��
        var savePawn = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
            ? (Action<string, GameObject>)GameManager.Instance.AttackPawnPoolSave
            : GameManager.Instance.DefencePawnPoolSave;        
        // ���� pawnpanel �е�ÿ�������壬�����ñ��淽��
        foreach (Transform item in pawnpanel.transform)
        {
            savePawn(item.gameObject.name, item.gameObject);
        }
        if (TurnBaseFSM.Instance.currentStateType == States.AttackPlacement)
        {
            TurnBaseFSM.Instance.ChangeState(States.DefenceDrawPile);
        }
        else if(TurnBaseFSM.Instance.currentStateType == States.DefencePlacement)
        {
            TurnBaseFSM.Instance.ChangeState(States.BattleRound);
        }
        // ���� pawnpanel ���
        pawnpanel.gameObject.SetActive(false);
    }
}
