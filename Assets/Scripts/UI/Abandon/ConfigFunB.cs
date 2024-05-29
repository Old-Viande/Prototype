using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ConfigFunB : MonoBehaviour
{
    //列表用来储存子物体
    public List<GameObject> ArmorChildList = new List<GameObject>();
    public List<GameObject> WeaponChildList = new List<GameObject>();
    public Button drewArmor;
    public Button drewWeapon;
    public Button ConfigButton;
    /// <summary>
    /// UI各个面板获取
    /// </summary>
    #region
    public GameObject UnitStockPanel;
    public GameObject ArmorStockPanel;
    public GameObject CardDetil;
    public GameObject Equipbar;
    #endregion
    public GameObject UniteCard;
    public GameObject ArmorCard;
    private Unites unitModol;


    public float duration = 1.0f; // 旋转动画的持续时间
    public int flipTimes = 3; // 硬币旋转的次数
    // Start is called before the first frame update
    void Start()
    {
        UnitStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "UContent");
        ArmorStockPanel = UITool.Instance.FindDeepChild(this.gameObject, "AContent");
        CardDetil = UITool.Instance.FindDeepChild(this.gameObject, "DetilText");
        Equipbar = UITool.Instance.FindDeepChild(this.gameObject, "EquipBar");      
        drewArmor.onClick.AddListener(DrewArmor);
        drewWeapon.onClick.AddListener(DrewWeapon);
        ConfigButton.onClick.AddListener(UniteConfig);
    }
    /// <summary>
    /// 抽卡按钮相关函数
    /// </summary>
    #region
    public void DrewWeapon()
    {
        GameObject card = UITool.Instance.FindDeepChild(this.gameObject, "UnitPile");
        RectTransform rectTransform = card.GetComponent<RectTransform>();
        drewWeapon.onClick.RemoveListener(DrewWeapon);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // 设置动画的缓动类型，使其更自然
            // 动画结束时调用ADrewRstart函数，重新绑定按钮的点击事件
            .OnComplete(() => {               
                drewWeapon.onClick.AddListener(DrewWeapon);
            });
         GameManager.Instance.DrewUnite();        
        //读取资源
        UniteCard = LodManager.Instance.LoadUIResource("UniteCard");
        //调整卡片上储存的数据
        UnitStockSave(UniteCard);
    }
    public void DrewArmor()
    {
        GameObject card = UITool.Instance.FindDeepChild(this.gameObject,"ArmorPile");
        RectTransform rectTransform = card.GetComponent<RectTransform>();
        drewArmor.onClick.RemoveListener(DrewArmor);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // 设置动画的缓动类型，使其更自然
            .OnComplete(() => {
                drewArmor.onClick.AddListener(DrewArmor); ;
            });
        GameManager.Instance.DrewArmor();
        ArmorCard = LodManager.Instance.LoadUIResource("ArmorCard");             
        ArmorStockSave(ArmorCard);
    }    
    #endregion

    public void UnitStockSave(GameObject card)
    {
      GameObject gameObject =  Instantiate(card, UnitStockPanel.transform);
      CardDataSave(gameObject);
      gameObject.name = "UnitCard";
    }
    public void ArmorStockSave(GameObject card)
    {
       GameObject gameObject = Instantiate(card, ArmorStockPanel.transform);
        CardDataSave(gameObject);
        gameObject.name = "ArmorCard";
    }
    public void CardDataSave(GameObject obj)
    {
        unitModol = GameManager.Instance.currentUnit;
        obj.GetComponent<CardData>().unites = unitModol;
        obj.GetComponent<CardData>().cardName = unitModol.Name;
    }

    public void UniteConfig()
    {//三个单位数据，分别对应配置完成的单位数据，来自武器的数据，来自护甲的数据
        Unites unites = new Unites();
        Unites Wunites = new Unites();
        Unites Aunites = new Unites();

        foreach (Transform item in Equipbar.transform)
        {
            Debug.Log(item.gameObject);
           if( item.gameObject.name== "UnitCard")
            {
                Wunites = item.gameObject.GetComponent<CardData>().unites;
            }
            else if(item.gameObject.name == "ArmorCard")
            {
                Aunites = item.gameObject.GetComponent<CardData>().unites;
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
        CardDetil.GetComponent<TextMeshProUGUI>().text = "单位名称：" + unites.Name + "\n" + "攻击力：" + unites.Damage + "\n" + "防御力：" + unites.Defence + "\n" + "速度：" + unites.Speed + "\n" + "攻击距离：" + unites.Range + "\n" + "攻击类型：" + unites.attackType + "\n" + "是否可跨越：" + unites.CanCross + "\n" + "是否可装备护盾：" + unites.Canshield;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
