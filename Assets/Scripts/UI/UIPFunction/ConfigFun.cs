using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEditor.Rendering.LookDev;
using System;

/*public class ConfigFun : MonoBehaviour
{
    //列表用来储存子物体
    public List<GameObject> ArmorChildList = new List<GameObject>();
    public List<GameObject> WeaponChildList = new List<GameObject>();
    public Button ConfigBtn;
    public Button ExitBtn;
    /// <summary>
    /// UI各个面板获取
    /// </summary>
    #region
    public GameObject UnitStockPanel;
    public GameObject ArmorStockPanel;
    public GameObject PawnStockPanel;
    public GameObject CardDetil;
    public GameObject Equipbar;
    #endregion
    public GameObject UniteCard;
    public GameObject ArmorCard;
    public Dictionary<string, GameObject> PawnSave = new Dictionary<string, GameObject>();

    private void OnEnable()
    {

    }
    void Start()
    {
        UnitStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "UContent");
        ArmorStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "AContent");
        PawnStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "PContent");
        CardDetil = UITool.Instance.FindDeepChild(this.gameObject, "DetilText");
        Equipbar = UITool.Instance.FindDeepChild(this.gameObject, "EquipBar");
        ConfigBtn.onClick.AddListener(UniteConfig);
        ExitBtn.onClick.AddListener(PanelExit);
        UniteStockSave();
        ArmorStockSave();
    }

    public void UniteStockSave()
    {
        if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration)
        {
            foreach (var item in GameManager.Instance.AttackUnitePool.poolDictionary)
            {
                GameManager.Instance.AttackUnitePoolGet(item.Key).transform.SetParent(UnitStockPanel.transform);
            }
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration)
        {
            foreach (var item in GameManager.Instance.AttackUnitePool.poolDictionary)
            {
                GameManager.Instance.DefenceUnitePoolGet(item.Key).transform.SetParent(UnitStockPanel.transform);
            }
        }
    }
    public void ArmorStockSave()
    {
        if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration)
        {
            foreach (var item in GameManager.Instance.DefenceArmorPool.poolDictionary)
            {
                GameManager.Instance.AttackArmorPoolGet(item.Key).transform.SetParent(ArmorStockPanel.transform);
            }
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration)
        {
            foreach (var item in GameManager.Instance.DefenceArmorPool.poolDictionary)
            {
                GameManager.Instance.DefenceArmorPoolGet(item.Key).transform.SetParent(ArmorStockPanel.transform);
            }
        }
    }

    public void UniteConfig()
    {//三个单位数据，分别对应配置完成的单位数据，来自武器的数据，来自护甲的数据
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        // 检查装备区是否有激活的子物体
        bool hasActiveItem = false;
        foreach (Transform item in Equipbar.transform)
        {
            if (item.gameObject.activeSelf)
            {
                hasActiveItem = true;
                break;
            }
        }

        // 如果没有激活的子物体，直接返回
        if (!hasActiveItem)
        {
            Debug.Log("No active items in Equipbar.");
            return;
        }

        foreach (Transform item in Equipbar.transform)
        {
            //这里有个bug明天来修，把数据的判断改回子类的类型
            //bug已修复
            if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration)
            {
                if (item.gameObject.GetComponent<CardData>().unites is Weapon)
                {
                    Wunites = item.gameObject.GetComponent<CardData>().unites;
                    GameManager.Instance.AttackUnitePoolSave(item.gameObject.name, item.gameObject);
                }
                else if (item.gameObject.GetComponent<CardData>().unites is Armor)
                {
                    Aunites = item.gameObject.GetComponent<CardData>().unites;
                    GameManager.Instance.AttackUnitePoolSave(item.gameObject.name, item.gameObject);
                }
            }
            else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration)
            {
                if (item.gameObject.GetComponent<CardData>().unites is Weapon)
                {
                    Wunites = item.gameObject.GetComponent<CardData>().unites;
                    GameManager.Instance.DefenceUnitePoolSave(item.gameObject.name, item.gameObject);
                }
                else if (item.gameObject.GetComponent<CardData>().unites is Armor)
                {
                    Aunites = item.gameObject.GetComponent<CardData>().unites;
                    GameManager.Instance.DefenceUnitePoolSave(item.gameObject.name, item.gameObject);
                }
            }
        }
        //这些是来自武器的数据
        unites.Name = Wunites.Name;
        unites.attackType = Wunites.attackType;
        unites.Damage = Wunites.Damage;
        unites.Range = Wunites.Range;
        unites.Canshield = Wunites.Canshield;
        //这些是来自护甲的数据
        unites.Defence = Aunites.Defence;
        unites.Speed = Aunites.Speed;
        unites.CanCross = Aunites.CanCross;
        CardDetil.GetComponent<TextMeshProUGUI>().text = "Unite Name:" + unites.Name + "\n" + "Damage :" + unites.Damage + "\n" + "Defence ;" + unites.Defence + "\n" + "Speed :" + unites.Speed + "\n" + "Range :" + unites.Range + "\n" + "ATK Type :" + unites.attackType + "\n" + "CanCrose :" + unites.CanCross + "\n" + "CanShiled :" + unites.Canshield;
        GameObject UniteCard = LodManager.Instance.LoadUIResource("PawnCard");
        PawnStock(Wunites, Aunites, unites, UniteCard);
    }
    public void PawnStock(Unites weapon, Unites armor, Unites unites, GameObject obj)
    {
        GameObject card = Instantiate(obj, PawnStockPanel.transform);
        card.name = PawnName(card);
        PawnSave.Add(card.name, card);
        card.GetComponent<PawnData>().Unites = unites;
        card.GetComponent<PawnData>().Weaopn = weapon;
        card.GetComponent<PawnData>().Armor = armor;
        card.GetComponent<PawnData>().Name = unites.Name;

    }
    public string PawnName(GameObject obj)
    {
        string baseName = obj.name;
        string newName = baseName;
        int index = 1;

        // 确保新生成的名字在字典中不存在
        while (PawnSave.ContainsKey(newName))
        {
            newName = baseName + "_" + index;
            index++;
        }

        // 更新 GameObject 的名字
        obj.name = newName;
        return newName;
    }
    public void PanelExit()
    {
        if (PawnSave.Count == 0) return;
        if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration)
        {
            foreach (var item in PawnSave)
            {
                //将单位保存到单位池中
                GameManager.Instance.AttackPawnPoolSave(item.Key, item.Value);

            }//所有单位保存完毕后清空字典
            PawnSave.Clear();
            TurnBaseFSM.Instance.ChangeState(States.AttackPlacement);
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration)
        {
            foreach (var item in PawnSave)
            {
                //将单位保存到单位池中
                GameManager.Instance.DefencePawnPoolSave(item.Key, item.Value);

            }//所有单位保存完毕后清空字典
            PawnSave.Clear();
            TurnBaseFSM.Instance.ChangeState(States.DefencePlacement);
        }

    }
}*/

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

    private void Start()
    {
        InitializeUIPanels();
        ConfigBtn.onClick.AddListener(UniteConfig);
        ExitBtn.onClick.AddListener(PanelExit);
        UIStock();
    }
    public void Update()
    {
        
    }
    public void UIStock()
    {
        SaveUniteStock(GameManager.Instance.AttackUnitePool, GameManager.Instance.DefenceUnitePool);
        SaveArmorStock(GameManager.Instance.AttackArmorPool, GameManager.Instance.DefenceArmorPool);
    }
    private void InitializeUIPanels()
    {
        UnitStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "UContent");
        ArmorStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "AContent");
        PawnStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "PContent");
        CardDetil = UITool.Instance.FindDeepChild(this.gameObject, "DetilText");
        Equipbar = UITool.Instance.FindDeepChild(this.gameObject, "EquipBar");
    }
    private void SaveUniteStock(ObjectPool attackPool, ObjectPool defencePool)
    {
        var currentPool = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? attackPool : defencePool;
        var isAtk = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration? true : false;
        if (isAtk)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameManager.Instance.AttackUnitePoolGet (item.Key).transform.SetParent(UnitStockPanel.transform);
            }
        }else if (currentPool == defencePool)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameManager.Instance.DefenceUnitePoolGet(item.Key).transform.SetParent(UnitStockPanel.transform);
            }
        }
    }
    private void SaveArmorStock(ObjectPool attackPool, ObjectPool defencePool)
    {
        var currentPool = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? attackPool : defencePool;
        var isAtk = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ? true : false;
        if (isAtk)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameManager.Instance.AttackArmorPoolGet(item.Key).transform.SetParent(ArmorStockPanel.transform);
            }
        }else if (currentPool == defencePool)
        {
            foreach (var item in currentPool.poolDictionary)
            {
                GameManager.Instance.DefenceArmorPoolGet(item.Key).transform.SetParent(ArmorStockPanel.transform);
            }
        }
    }

    public void UniteConfig()
    {
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        if (!HasActiveItems(Equipbar))
        {
            Debug.Log("No active items in Equipbar.");
            return;
        }

        foreach (Transform item in Equipbar.transform)
        {
            var cardData = item.gameObject.GetComponent<CardData>();
            if (cardData == null) continue;

            if (TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration)
            {
                SaveUniteData(cardData, ref Wunites, ref Aunites, GameManager.Instance.AttackUnitePoolSave);
            }
            else if (TurnBaseFSM.Instance.currentStateType == States.DefenceConfiguration)
            {
                SaveUniteData(cardData, ref Wunites, ref Aunites, GameManager.Instance.DefenceUnitePoolSave);
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
                return true;
            }
        }
        return false;
    }

    private void SaveUniteData(CardData cardData, ref Unites Wunites, ref Unites Aunites, Action<string, GameObject> saveAction)
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

    private Unites FillUniteData(Unites unites, Unites Wunites, Unites Aunites)
    {
        unites.Name = Wunites.Name;
        unites.attackType = Wunites.attackType;
        unites.Damage = Wunites.Damage;
        unites.Range = Wunites.Range;
        unites.Canshield = Wunites.Canshield;
        unites.Defence = Aunites.Defence+ Wunites.Defence;
        unites.Speed = Aunites.Speed;
        unites.CanCross = Aunites.CanCross;

        return unites;
    }

    private void UpdateCardDetail(Unites unites)
    {
        CardDetil.GetComponent<TextMeshProUGUI>().text = $"Unite Name: {unites.Name}\n" +
                                                         $"Damage: {unites.Damage}\n" +
                                                         $"Defence: {unites.Defence}\n" +
                                                         $"Speed: {unites.Speed}\n" +
                                                         $"Range: {unites.Range}\n" +
                                                         $"ATK Type: {unites.attackType}\n" +
                                                         $"Can Cross: {unites.CanCross}\n" +
                                                         $"Can Shield: {unites.Canshield}";
    }

    public void PawnStock(Unites weapon, Unites armor, Unites unites, GameObject obj)
    {
        GameObject card = Instantiate(obj, PawnStockPanel.transform);
        card.name = GenerateUniqueName(card);
        PawnSave.Add(card.name, card);
        var pawnData = card.GetComponent<PawnData>();
        pawnData.Unites = unites;
        pawnData.Weaopn = weapon;
        pawnData.Armor = armor;
        pawnData.Name = unites.Name;
    }

    private string GenerateUniqueName(GameObject obj)
    {
        string newName = obj.name + "_" + Guid.NewGuid().ToString();
        obj.name = newName;
        return newName;
    }

    public void PanelExit()
    {
        if (PawnSave.Count == 0) return;

        var saveAction = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ?
                         (Action<string, GameObject>)GameManager.Instance.AttackPawnPoolSave :
                         GameManager.Instance.DefencePawnPoolSave;

        foreach (var item in PawnSave)
        {
            saveAction(item.Key, item.Value);
        }

        PawnSave.Clear();
        var nextState = TurnBaseFSM.Instance.currentStateType == States.AttackConfiguration ?
                        States.AttackPlacement :
                        States.DefencePlacement;

        TurnBaseFSM.Instance.ChangeState(nextState);
    }
}

