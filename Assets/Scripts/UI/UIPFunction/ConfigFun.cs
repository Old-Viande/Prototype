using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfigFun : Singleton<ConfigFun>
{
    public List<GameObject> ArmorChildList = new List<GameObject>();
    public List<GameObject> WeaponChildList = new List<GameObject>();
    public Button ConfigBtn;
    public Button ExitBtn;

    #region UI Panels
    public GameObject UnitStockPanel;
    public GameObject ArmorStockPanel;
    public GameObject PawnStockPanel;
    public GameObject CardDetil;
    public GameObject Equipbar;
    #endregion

    public GameObject UniteCard;
    public GameObject ArmorCard;
    public Dictionary<string, GameObject> PawnSave = new Dictionary<string, GameObject>();
    public List<Sprite> Unitlist = new List<Sprite>();
    private List<Transform> previousChildren;
    private List<GameObject> displayList = new List<GameObject>();

    private void Start()
    {
        InitializeUIPanels();
        ConfigBtn.onClick.AddListener(UniteConfig);
        ExitBtn.onClick.AddListener(PanelExit);
        UIStock();
        CheckInite();


    }
    public void CheckInite()//���װ����������仯���
    {
        previousChildren = new List<Transform>(Equipbar.transform.childCount);
        foreach (Transform child in Equipbar.transform)
        {
            previousChildren.Add(child);
        }
    }
    public void FixedUpdate()
    {
        EbarCheckForChanges();//���װ����������仯���
    }
    #region װ�������&������仯���
    private void InitializeUIPanels()
    {//��ʼ��UI���,��ȡ������������
        UnitStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "UContent");
        ArmorStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "AContent");
        PawnStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "PContent");
        CardDetil = UITool.Instance.FindDeepChild(this.gameObject, "DetilText");
        Equipbar = UITool.Instance.FindDeepChild(this.gameObject, "EquipBar");
    }
    void EbarCheckForChanges()
    {
        // ��������������Ƿ�仯
        if (Equipbar.transform.childCount != previousChildren.Count)
        {
            OnChildrenChanged();
            return;
        }

        // ����������Ƿ����仯�����罻����
        for (int i = 0; i < Equipbar.transform.childCount; i++)
        {
            if (Equipbar.transform.GetChild(i) != previousChildren[i])
            {
                OnChildrenChanged();
                return;
            }
        }
    }
    void OnChildrenChanged()//�����巢���仯ʱ����
    {
        // ��¼��ǰ��������״̬
        previousChildren.Clear();
        foreach (Transform child in Equipbar.transform)//����װ�����е�ÿ��������
        {
            if (child.gameObject.activeSelf)
            {
                previousChildren.Add(child);
            }
            else
            {
                displayList.Add(child.gameObject);//��װ�����е�δ����������Ϊ����Ҫ������
            }

        }

        // ������������
        // DisPlayListClear();
        UIPanelInfo();
    }

    private void UIPanelInfo()
    {//������λ���ݣ��ֱ��Ӧ������ɵĵ�λ���ݣ��������������ݣ����Ի��׵�����
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        if (!HasActiveItems(Equipbar))  // ���װ�����Ƿ��м����������
        {
            Debug.Log("No active items in Equipbar.");
            return;
        }

        foreach (Transform item in Equipbar.transform)
        {
            //�����и�bug�������ޣ������ݵ��жϸĻ����������
            //bug���޸�
            if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration|| TurnBaseFSM.Instance.currentStateType == States.AttackConfigurationRound)
            {
                if (item.gameObject.GetComponent<CardData>().unites is Weapon)
                {
                    Wunites = item.gameObject.GetComponent<CardData>().unites;
                }
                else if (item.gameObject.GetComponent<CardData>().unites is Armor)
                {
                    Aunites = item.gameObject.GetComponent<CardData>().unites;
                }
            }
            else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration)
            {
                if (item.gameObject.GetComponent<CardData>().unites is Weapon)
                {
                    Wunites = item.gameObject.GetComponent<CardData>().unites;

                }
                else if (item.gameObject.GetComponent<CardData>().unites is Armor)
                {
                    Aunites = item.gameObject.GetComponent<CardData>().unites;
                }
            }

            FillUniteData(unites, Wunites, Aunites);
            UpdateCardDetail(unites);
        }
    }

    private void DisPlayListClear()
    {
        foreach (var item in displayList)
        {
            GameManager.Instance.AbandonedPoolSave(item.name, item);//���ٲ���Ҫ������
        }

    }
    #endregion
    public void UIStock()//��UI���Ķ�Ӧλ�ô������ؿ����ڵĿ�Ƭ��
    {
        SaveUniteStock(GameManager.Instance.AttackUnitePool, GameManager.Instance.DefenceUnitePool);
        SaveArmorStock(GameManager.Instance.AttackArmorPool, GameManager.Instance.DefenceArmorPool);
    }
    private void SaveUniteStock(ObjectPool attackPool, ObjectPool defencePool)
    {
        ObjectPool currentPool;
        bool isAtk;
        switch (TurnBaseFSM.Instance.currentStateType)
        {

            case States.AttackConfiguration:
                currentPool = attackPool;
                isAtk = true;
                break;
            case States.DefenceConfiguration:
                currentPool = defencePool;
                isAtk = false;
                break;
            case States.AttackConfigurationRound:
                currentPool = attackPool;
                isAtk = true;
                break;
            case States.DefenceConfigurationRound:
                currentPool = defencePool;
                isAtk = false;
                break;
            default:
                return;
        }
        //var currentPool = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? attackPool : defencePool;
        //var isAtk = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? true : false;
        if (isAtk)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameObject obj = GameManager.Instance.AttackUnitePoolGet(item.Key);
                obj.transform.SetParent(UnitStockPanel.transform);
                obj.GetComponent<OnDragCard>().CardDragInit();

            }
            GameManager.Instance.AttackUnitePool.ClearPool();
        }
        else if (currentPool == defencePool)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameObject obj = GameManager.Instance.DefenceUnitePoolGet(item.Key);
                obj.transform.SetParent(UnitStockPanel.transform);
                obj.GetComponent<OnDragCard>().CardDragInit();
            }
            GameManager.Instance.DefenceUnitePool.ClearPool();
        }
    }
    private void SaveArmorStock(ObjectPool attackPool, ObjectPool defencePool)
    {
        ObjectPool currentPool;
        bool isAtk;
        switch (TurnBaseFSM.Instance.currentStateType)
        {

            case States.AttackConfiguration:
                currentPool = attackPool;
                isAtk = true;
                break;
            case States.DefenceConfiguration:
                currentPool = defencePool;
                isAtk = false;
                break;
            case States.AttackConfigurationRound:
                currentPool = attackPool;
                isAtk = true;
                break;
            case States.DefenceConfigurationRound:
                currentPool = defencePool;
                isAtk = false;
                break;
            default:
                return;
        }
        //var currentPool = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? attackPool : defencePool;
        //var isAtk = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? true : false;
        if (isAtk)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameObject obj =
                GameManager.Instance.AttackArmorPoolGet(item.Key);
                obj.transform.SetParent(ArmorStockPanel.transform);
                obj.GetComponent<OnDragCard>().CardDragInit();
            }
            GameManager.Instance.AttackArmorPool.ClearPool();
        }
        else if (currentPool == defencePool)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameObject obj =
                GameManager.Instance.DefenceArmorPoolGet(item.Key);
                obj.transform.SetParent(ArmorStockPanel.transform);
                obj.GetComponent<OnDragCard>().CardDragInit();
            }
            GameManager.Instance.DefenceArmorPool.ClearPool();
        }
    }

    public void UniteConfig()
    {
        // ������λ���ݣ��ֱ��Ӧ������ɵĵ�λ���ݣ��������������ݣ����Ի��׵�����
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        if (!HasActiveItems(Equipbar))  // ���װ�����Ƿ��м����������
        {
            Debug.Log("No active items in Equipbar.");
            return;
        }

        // ����һ����ʱ�б����洢Ҫ���������
        List<Transform> activeItems = new List<Transform>();

        // �ռ����м����������
        foreach (Transform item in Equipbar.transform)
        {
            var cardData = item.gameObject.GetComponent<CardData>();
            if (cardData == null) continue;
            if (!item.gameObject.activeSelf) continue;

            activeItems.Add(item);
        }

        // �����ռ���������
        foreach (Transform item in activeItems)
        {
            var cardData = item.gameObject.GetComponent<CardData>();
            if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration || TurnBaseFSM.Instance.currentStateType == States.AttackConfigurationRound)
            {
                SaveUniteData(cardData, ref Wunites, ref Aunites, GameManager.Instance.AbandonedPoolSave);
            }
            else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration || TurnBaseFSM.Instance.currentStateType == States.DefenceConfigurationRound)
            {
                SaveUniteData(cardData, ref Wunites, ref Aunites, GameManager.Instance.AbandonedPoolSave);
            }
        }

        FillUniteData(unites, Wunites, Aunites);
        UpdateCardDetail(unites);
        GameObject uniteCard = LodManager.Instance.LoadUIResource("PawnCard");
        PawnStock(Wunites, Aunites, unites, uniteCard);
    }

    private bool HasActiveItems(GameObject equipbar)
    {
        foreach (Transform item in equipbar.transform)
        {
            if (item.gameObject.activeSelf)
            {
                if (item.gameObject.GetComponent<CardData>().unites is Weapon)
                    return true;
            }
        }
        return false;
    }
    private void SaveUniteData(CardData cardData, ref Unites Wunites, ref Unites Aunites, Action<string, GameObject> saveAction)// ��������ǽ������ݵı��棬���ݿ�Ƭ����������������
    {
        if (cardData.unites is Weapon)
        {
            Wunites = cardData.unites;
        }
        else if (cardData.unites is Armor)
        {
            Aunites = cardData.unites;
        }
        saveAction(cardData.gameObject.name, cardData.gameObject);
    }
    //private void SaveUniteData(CardData cardData, ref Unites Wunites, ref Unites Aunites)
    //{
    //    if (cardData.unites is Weapon)
    //    {
    //        Wunites = cardData.unites;
    //    }
    //    else if (cardData.unites is Armor)
    //    {
    //        Aunites = cardData.unites;
    //    }

    //}
    /// <summary>
    /// �������������Ǹ��������ͻ��׵����������ģ���һ�����������ݺϲ��Ĺ���
    /// </summary>
    /// <param name="unites"></param>
    /// <param name="Wunites"></param>
    /// <param name="Aunites"></param>
    /// <returns></returns>
    private Unites FillUniteData(Unites unites, Unites Wunites, Unites Aunites)
    {
        unites.Name = Wunites.Name;
        unites.attackType = Wunites.attackType;
        unites.Damage = Wunites.Damage;
        unites.Range = Wunites.Range;
        unites.Canshield = Wunites.Canshield;
        unites.Defence = Aunites.Defence + Wunites.Defence;
        if (Aunites.Speed == 0)//��Ϊû�е�λ���ٶ�Ϊ0�����Ի���ֵ�ṩ���ٶ�Ϊ0ʱ������λû���·�����Ϊ�ٶ�Ϊ���ֵ
        { unites.Speed = Wunites.Speed; }
        else { unites.Speed = Aunites.Speed; }
        unites.CanCross = Aunites.CanCross;

        return unites;
    }

    private void UpdateCardDetail(Unites unites)
    {
        CardDetil.GetComponent<TextMeshProUGUI>().text = "<color=#446B9D>"+ unites.Name + "\n" +
                                                         "<sprite=0> Damage:" + unites.Damage+"\n" +
                                                         "<sprite=1>Defence:" + unites.Defence+"\n" +
                                                         "<sprite=2>Speed:" + unites.Speed+"\n" +
                                                         "<sprite=6>Range:" + unites.Range+"\n";
                                                        
    }

    public void PawnStock(Unites weapon, Unites armor, Unites unites, GameObject obj)//����λ���뵥λ���
    {
        GameObject card = Instantiate(obj, PawnStockPanel.transform);
        PawnCardImageSet(card, weapon.Name);//���ÿ�Ƭ��ͼƬ
        card.name = GenerateUniqueName(card);
        PawnSave.Add(card.name, card);
        var pawnData = card.GetComponent<PawnData>();
        pawnData.Unites = unites;
        pawnData.Weaopn = weapon;
        pawnData.Armor = armor;
        pawnData.Name = unites.Name;
    }
    private void PawnCardImageSet(GameObject obj, string name)
    {
        switch (name)
        {
            case "Sword":
                obj.GetComponent<Image>().sprite = Unitlist[0];
                break;
            case "Halberd":
                obj.GetComponent<Image>().sprite = Unitlist[1];
                break;
            case "GreatSword":
                obj.GetComponent<Image>().sprite = Unitlist[2];
                break;
            case "Spear":
                obj.GetComponent<Image>().sprite = Unitlist[3];
                break;
            case "LongBow":
                obj.GetComponent<Image>().sprite = Unitlist[4];
                break;
            case "CrossBow":
                obj.GetComponent<Image>().sprite = Unitlist[5];
                break;
        }
    }
    private string GenerateUniqueName(GameObject obj)//����Ψһ������
    {
        string newName = obj.name + "_" + Guid.NewGuid().ToString();
        obj.name = newName;
        return newName;
    }

    public void PanelCheck()//��UI���ر�ʱ�Ŀ���ʣ��Ŀ�Ƭ���浽��λ����
    {
        List<GameObject> atkcard = new List<GameObject>();//������δʹ�õ�������Ƭ
        List<GameObject> armcard = new List<GameObject>();//������δʹ�õĻ��׿�Ƭ      
        foreach (Transform item in UnitStockPanel.transform)
        {
            if (item.gameObject.activeSelf == true)
            {
                atkcard.Add(item.gameObject);//��������δʹ�õļ���״̬��������Ƭ��ӵ��б���
            }
        }
        foreach (Transform item in ArmorStockPanel.transform)
        {
            if (item.gameObject.activeSelf == true)
            {
                armcard.Add(item.gameObject);//��������δʹ�õļ���״̬�Ļ��׿�Ƭ��ӵ��б���
            }
        }
        if (atkcard.Count == 0 && armcard.Count == 0)
        {
            return;
        }
        if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration||TurnBaseFSM.Instance.currentStateType == States.AttackConfigurationRound)
        {

            foreach (var item in atkcard)
            {
                GameManager.Instance.AttackUnitePoolSave(item.name, item);//��������δʹ�õ�������Ƭ���浽��λ����
            }
            foreach (var item in armcard)
            {
                GameManager.Instance.AttackArmorPoolSave(item.name, item);//��������δʹ�õĻ��׿�Ƭ���浽��λ����
            }
        }
        else
        {
            foreach (var item in atkcard)
            {
                GameManager.Instance.DefenceUnitePoolSave(item.name, item);//��������δʹ�õ�������Ƭ���浽��λ����
            }
            foreach (var item in armcard)
            {
                GameManager.Instance.DefenceArmorPoolSave(item.name, item);//��������δʹ�õĻ��׿�Ƭ���浽��λ����
            }
        }
    }





    public void PanelExit()
    {
        if(TurnBaseFSM.Instance.currentStateType == States.AttackConfigurationRound || TurnBaseFSM.Instance.currentStateType == States.DefenceConfigurationRound)
        {

        }
        else { if (PawnSave.Count == 0) return; }
       

        Action<string, GameObject> saveAction;
        States nextState;
        switch (TurnBaseFSM.Instance.currentStateType)
        {

           case States.AttackConfiguration:
                saveAction = GameManager.Instance.AttackPawnPoolSave;
                nextState = States.AttackPlacement;
                break;
            case States.DefenceConfiguration:
                saveAction = GameManager.Instance.DefencePawnPoolSave;
                nextState = States.DefencePlacement;
                break;
           case States.AttackConfigurationRound:
                saveAction = GameManager.Instance.AttackPawnPoolSave;
                nextState = States.DefenceDrawRound;
                break;
            case States.DefenceConfigurationRound:
                saveAction = GameManager.Instance.DefencePawnPoolSave;
                nextState = States.RangeAttack;
                break;
            default:
                return;
        }
        PanelCheck();
        foreach (var item in PawnSave)
        {
            saveAction(item.Key, item.Value);
        }

        PawnSave.Clear();
        TurnBaseFSM.Instance.ChangeState(nextState);     
    }
}

