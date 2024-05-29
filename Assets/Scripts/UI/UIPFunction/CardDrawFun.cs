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

    public float duration = 1.0f; // ��ת�����ĳ���ʱ��
    public int flipTimes = 3; // Ӳ����ת�Ĵ���
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
            .SetEase(Ease.InOutQuad) // ���ö����Ļ������ͣ�ʹ�����Ȼ
                                     // ��������ʱ����ADrewRstart���������°󶨰�ť�ĵ���¼�
            .OnComplete(() =>
            {
                drewWeapon.onClick.AddListener(DrewWeapon);
            });
        GameManager.Instance.DrewUnite();
        //��ȡ��Դ
        UniteCard = LodManager.Instance.LoadUIResource("UniteCard");
        //������Ƭ�ϴ��������
        UniteStockSave(UniteCard);
    }

    public void DrewArmor()
    {
        RectTransform rectTransform = drewArmor.GetComponent<RectTransform>();
        drewArmor.onClick.RemoveListener(DrewArmor);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // ���ö����Ļ������ͣ�ʹ�����Ȼ
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
    //���ݴ���
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
    {//�����鿨 �жϵ�ǰ�غ�״̬ �Բ�ͬ��״̬���벻ͬ�ĳ���
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
            TurnBaseFSM.Instance.ChangeState(States.AttackConfiguration);//Ҫ�ǹ������ĻغϾͽ��빥�����ĺ���״̬
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
            TurnBaseFSM.Instance.ChangeState(States.DefenceConfiguration);//Ҫ�Ƿ��ط��ĻغϾͽ�����ط��ĺ���״̬
        }        
    }
}
