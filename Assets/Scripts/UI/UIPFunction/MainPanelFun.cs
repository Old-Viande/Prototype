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
            // 激活面板以及 pawnpanel 面板中的所有子物体
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
             //生成可拖拽的棋子后，清空对象池
             GameManager.Instance.AttackPawnPool.ClearPool();

         }
         else if (TurnBaseFSM.Instance.currentStateType == States.DefencePlacement)
         {
             foreach (var item in GameManager.Instance.DefencePawnPool.poolDictionary)
             {
                 GameObject obj = GameManager.Instance.DefencePawnPoolGet(item.Key);
                 obj.transform.SetParent(pcontent.transform);
             }
             //生成可拖拽的棋子后，清空对象池
             GameManager.Instance.DefencePawnPool.ClearPool();
         }
     }*/
    public void PawnPanelInit()
    {
        // 根据当前状态类型选择合适的棋子池和获取方法
        var (pawnPool, getPawn) = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
            ? (GameManager.Instance.AttackPawnPool, (Func<string, GameObject>)GameManager.Instance.AttackPawnPoolGet)
            : (GameManager.Instance.DefencePawnPool, GameManager.Instance.DefencePawnPoolGet);

        // 遍历棋子池中的所有对象，并将其添加到 pcontent 面板
        foreach (var item in pawnPool.poolDictionary)
        {
            GameObject obj = getPawn(item.Key);
            obj.transform.SetParent(pcontent.transform);
        }

        // 生成可拖拽的棋子后，清空对象池
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
        // 根据当前状态类型选择合适的保存方法
        var savePawn = TurnBaseFSM.Instance.currentStateType == States.AttackPlacement
            ? (Action<string, GameObject>)GameManager.Instance.AttackPawnPoolSave
            : GameManager.Instance.DefencePawnPoolSave;        
        // 遍历 pawnpanel 中的每个子物体，并调用保存方法
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
        // 隐藏 pawnpanel 面板
        pawnpanel.gameObject.SetActive(false);
    }
}
