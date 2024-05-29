using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ConfigFunB : MonoBehaviour
{
    //�б���������������
    public List<GameObject> ArmorChildList = new List<GameObject>();
    public List<GameObject> WeaponChildList = new List<GameObject>();
    public Button drewArmor;
    public Button drewWeapon;
    public Button ConfigButton;
    /// <summary>
    /// UI��������ȡ
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


    public float duration = 1.0f; // ��ת�����ĳ���ʱ��
    public int flipTimes = 3; // Ӳ����ת�Ĵ���
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
    /// �鿨��ť��غ���
    /// </summary>
    #region
    public void DrewWeapon()
    {
        GameObject card = UITool.Instance.FindDeepChild(this.gameObject, "UnitPile");
        RectTransform rectTransform = card.GetComponent<RectTransform>();
        drewWeapon.onClick.RemoveListener(DrewWeapon);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // ���ö����Ļ������ͣ�ʹ�����Ȼ
            // ��������ʱ����ADrewRstart���������°󶨰�ť�ĵ���¼�
            .OnComplete(() => {               
                drewWeapon.onClick.AddListener(DrewWeapon);
            });
         GameManager.Instance.DrewUnite();        
        //��ȡ��Դ
        UniteCard = LodManager.Instance.LoadUIResource("UniteCard");
        //������Ƭ�ϴ��������
        UnitStockSave(UniteCard);
    }
    public void DrewArmor()
    {
        GameObject card = UITool.Instance.FindDeepChild(this.gameObject,"ArmorPile");
        RectTransform rectTransform = card.GetComponent<RectTransform>();
        drewArmor.onClick.RemoveListener(DrewArmor);
        rectTransform.DORotate(new Vector3(0, 360 * flipTimes, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad) // ���ö����Ļ������ͣ�ʹ�����Ȼ
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
    {//������λ���ݣ��ֱ��Ӧ������ɵĵ�λ���ݣ��������������ݣ����Ի��׵�����
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
        //��Щ����������������
        unites.Name = Wunites.Name;
        unites.attackType = Wunites.attackType;
        unites.Damage = Wunites.Damage;
        unites.Range = Wunites.Range;
        unites.Canshield = Wunites.Canshield;
        //��Щ�����Ի��׵�����
        unites.Defence = Aunites.Defence;
        unites.Speed = Aunites.Speed;
        unites.CanCross = Aunites.CanCross;
        CardDetil.GetComponent<TextMeshProUGUI>().text = "��λ���ƣ�" + unites.Name + "\n" + "��������" + unites.Damage + "\n" + "��������" + unites.Defence + "\n" + "�ٶȣ�" + unites.Speed + "\n" + "�������룺" + unites.Range + "\n" + "�������ͣ�" + unites.attackType + "\n" + "�Ƿ�ɿ�Խ��" + unites.CanCross + "\n" + "�Ƿ��װ�����ܣ�" + unites.Canshield;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
