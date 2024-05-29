using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDrawFun : Singleton<CardDrawFun>
{
    public Button drewArmor;
    public Button drewWeapon;
    public Button finish;

    public GameObject UContent;
    public GameObject AContent;

    public GameObject UnitStockPanel;
    public GameObject ArmorStockPanel;

    public GameObject UniteCard;
    public GameObject ArmorCard;

    public float duration = 1.0f; // 旋转动画的持续时间
    public int flipTimes = 3; // 硬币旋转的次数
    private Unites unitModol;
    public int DrawTimes = 0;
    public GameObject textP;

    public Dictionary<string, GameObject> UniteSave = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ArmorSave = new Dictionary<string, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        drewArmor.onClick.AddListener(DrewArmor);
        drewWeapon.onClick.AddListener(DrewWeapon);
        finish.onClick.AddListener(DrawCardOver);
    }

    // Update is called once per frame   
    public void DrewWeapon()
    {
        RectTransform rectTransform = drewWeapon.GetComponent<RectTransform>();
        drewWeapon.onClick.RemoveListener(DrewWeapon);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // 设置动画的缓动类型，使其更自然
                                     // 动画结束时调用ADrewRstart函数，重新绑定按钮的点击事件
            .OnComplete(() =>
            {
                drewWeapon.onClick.AddListener(DrewWeapon);
            });
        GameManager.Instance.DrewUnite();
        //读取资源
        UniteCard = LodManager.Instance.LoadUIResource("UniteCard");
        //调整卡片上储存的数据
        UniteStockSave(UniteCard);
    }

    public void DrewArmor()
    {
        RectTransform rectTransform = drewArmor.GetComponent<RectTransform>();
        drewArmor.onClick.RemoveListener(DrewArmor);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // 设置动画的缓动类型，使其更自然
            .OnComplete(() =>
            {
                drewArmor.onClick.AddListener(DrewArmor); ;
            });
        GameManager.Instance.DrewArmor();
        ArmorCard = LodManager.Instance.LoadUIResource("ArmorCard");
        ArmorStockSave(ArmorCard);
    }

    private void ArmorStockSave(GameObject obj)
    {
        GameObject card = Instantiate(obj, AContent.transform);
        ArmorName(card);
        ArmorSave.Add(card.name, card);
        CardDataSave(card);
    }

    public void UniteStockSave(GameObject obj)
    {
        GameObject card = Instantiate(obj, UContent.transform);
        UniteName(card);
        UniteSave.Add(card.name, card);
        CardDataSave(card);
    }
    //数据储存
    public void CardDataSave(GameObject obj)
    {
        TextInfoSave();
        unitModol = GameManager.Instance.currentUnit;
        obj.GetComponent<CardData>().unites = unitModol;
        obj.GetComponent<CardData>().cardName = unitModol.Name;
    }
    private void TextInfoSave()
    {
        textP.GetComponentInChildren<TextMeshProUGUI>().text = DrawTimes + " card draws left.";

    }

    public string GenerateUniqueName(GameObject obj)
    {
        string newName = obj.name + "_" + Guid.NewGuid().ToString();
        obj.name = newName;
        return newName;
    }

    public string UniteName(GameObject obj)
    {
        return GenerateUniqueName(obj);
    }

    public string ArmorName(GameObject obj)
    {
        return GenerateUniqueName(obj);
    }
    public void DrawCardOver()
    {//结束抽卡 判断当前回合状态 以不同的状态存入不同的池中
        if(TurnBaseFSM.Instance.currentStateType == States.AttackDrawPile)
        {
            foreach (var item in UniteSave)
            {
                if(item.Value.activeSelf==false)                
                    continue;
               
                GameManager.Instance.AttackUnitePoolSave(item.Key, item.Value);
            }
            foreach (var item in ArmorSave)
            {
                if (item.Value.activeSelf == false)
                    continue;
                GameManager.Instance.AttackArmorPoolSave(item.Key, item.Value);
            }
            Debug.Log(GameManager.Instance.AttackUnitePool.poolDictionary.Count);
            Debug.Log(GameManager.Instance.AttackArmorPool.poolDictionary.Count);
            TurnBaseFSM.Instance.ChangeState(States.AttackConfiguration);//要是攻击方的回合就进入攻击方的后续状态
        }
        else if(TurnBaseFSM.Instance.currentStateType == States.DefenceDrawPile)
        {
            foreach (var item in UniteSave)
            {
                if (item.Value.activeSelf == false)
                    continue;
                GameManager.Instance.DefenceUnitePoolSave(item.Key, item.Value);
            }
            foreach (var item in ArmorSave)
            {
                if (item.Value.activeSelf == false)
                    continue;
                GameManager.Instance.DefenceArmorPoolSave(item.Key, item.Value);
            }
            Debug.Log(GameManager.Instance.DefenceUnitePool.poolDictionary.Count);
            Debug.Log(GameManager.Instance.DefenceArmorPool.poolDictionary.Count);
            TurnBaseFSM.Instance.ChangeState(States.DefenceConfiguration);//要是防守方的回合就进入防守方的后续状态
        }        
    }
}
