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
    public void CheckInite()//检查装备栏子物体变化检测
    {
        previousChildren = new List<Transform>(Equipbar.transform.childCount);
        foreach (Transform child in Equipbar.transform)
        {
            previousChildren.Add(child);
        }
    }
    public void FixedUpdate()
    {
        EbarCheckForChanges();//检查装备栏子物体变化检测
    }
    #region 装备栏检查&子物体变化检测
    private void InitializeUIPanels()
    {//初始化UI面板,获取各个面板的引用
        UnitStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "UContent");
        ArmorStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "AContent");
        PawnStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "PContent");
        CardDetil = UITool.Instance.FindDeepChild(this.gameObject, "DetilText");
        Equipbar = UITool.Instance.FindDeepChild(this.gameObject, "EquipBar");
    }
    void EbarCheckForChanges()
    {
        // 检查子物体数量是否变化
        if (Equipbar.transform.childCount != previousChildren.Count)
        {
            OnChildrenChanged();
            return;
        }

        // 检查子物体是否发生变化（例如交换）
        for (int i = 0; i < Equipbar.transform.childCount; i++)
        {
            if (Equipbar.transform.GetChild(i) != previousChildren[i])
            {
                OnChildrenChanged();
                return;
            }
        }
    }
    void OnChildrenChanged()//子物体发生变化时调用
    {
        // 记录当前的子物体状态
        previousChildren.Clear();
        foreach (Transform child in Equipbar.transform)//遍历装备栏中的每个子物体
        {
            if (child.gameObject.activeSelf)
            {
                previousChildren.Add(child);
            }
            else
            {
                displayList.Add(child.gameObject);//在装备栏中的未激活物体则为不需要的物体
            }

        }

        // 调用其他方法
        // DisPlayListClear();
        UIPanelInfo();
    }

    private void UIPanelInfo()
    {//三个单位数据，分别对应配置完成的单位数据，来自武器的数据，来自护甲的数据
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        if (!HasActiveItems(Equipbar))  // 检查装备区是否有激活的子物体
        {
            Debug.Log("No active items in Equipbar.");
            return;
        }

        foreach (Transform item in Equipbar.transform)
        {
            //这里有个bug明天来修，把数据的判断改回子类的类型
            //bug已修复
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
            GameManager.Instance.AbandonedPoolSave(item.name, item);//销毁不需要的物体
        }

    }
    #endregion
    public void UIStock()//将UI面板的对应位置存入对象池卡牌内的卡片保
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
        // 三个单位数据，分别对应配置完成的单位数据，来自武器的数据，来自护甲的数据
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        if (!HasActiveItems(Equipbar))  // 检查装备区是否有激活的子物体
        {
            Debug.Log("No active items in Equipbar.");
            return;
        }

        // 创建一个临时列表来存储要处理的物体
        List<Transform> activeItems = new List<Transform>();

        // 收集所有激活的子物体
        foreach (Transform item in Equipbar.transform)
        {
            var cardData = item.gameObject.GetComponent<CardData>();
            if (cardData == null) continue;
            if (!item.gameObject.activeSelf) continue;

            activeItems.Add(item);
        }

        // 处理收集到的物体
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
    private void SaveUniteData(CardData cardData, ref Unites Wunites, ref Unites Aunites, Action<string, GameObject> saveAction)// 这个方法是进行数据的保存，根据卡片的类型来保存数据
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
    /// 这里的数据填充是根据武器和护甲的数据来填充的，是一个将两个数据合并的过程
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
        if (Aunites.Speed == 0)//因为没有单位的速度为0，所以护甲值提供的速度为0时，代表单位没穿衣服，因为速度为最大值
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

    public void PawnStock(Unites weapon, Unites armor, Unites unites, GameObject obj)//将单位存入单位面板
    {
        GameObject card = Instantiate(obj, PawnStockPanel.transform);
        PawnCardImageSet(card, weapon.Name);//设置卡片的图片
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
    private string GenerateUniqueName(GameObject obj)//生成唯一的名字
    {
        string newName = obj.name + "_" + Guid.NewGuid().ToString();
        obj.name = newName;
        return newName;
    }

    public void PanelCheck()//将UI面板关闭时的卡牌剩余的卡片保存到单位池中
    {
        List<GameObject> atkcard = new List<GameObject>();//卡槽中未使用的武器卡片
        List<GameObject> armcard = new List<GameObject>();//卡槽中未使用的护甲卡片      
        foreach (Transform item in UnitStockPanel.transform)
        {
            if (item.gameObject.activeSelf == true)
            {
                atkcard.Add(item.gameObject);//将卡槽中未使用的激活状态的武器卡片添加到列表中
            }
        }
        foreach (Transform item in ArmorStockPanel.transform)
        {
            if (item.gameObject.activeSelf == true)
            {
                armcard.Add(item.gameObject);//将卡槽中未使用的激活状态的护甲卡片添加到列表中
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
                GameManager.Instance.AttackUnitePoolSave(item.name, item);//将卡槽中未使用的武器卡片保存到单位池中
            }
            foreach (var item in armcard)
            {
                GameManager.Instance.AttackArmorPoolSave(item.name, item);//将卡槽中未使用的护甲卡片保存到单位池中
            }
        }
        else
        {
            foreach (var item in atkcard)
            {
                GameManager.Instance.DefenceUnitePoolSave(item.name, item);//将卡槽中未使用的武器卡片保存到单位池中
            }
            foreach (var item in armcard)
            {
                GameManager.Instance.DefenceArmorPoolSave(item.name, item);//将卡槽中未使用的护甲卡片保存到单位池中
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

