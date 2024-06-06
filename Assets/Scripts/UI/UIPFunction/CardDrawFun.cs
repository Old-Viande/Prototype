using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrawFun : Singleton<CardDrawFun>
{
    public Button drewArmor;
    public Button drewWeapon;
    public Button finish;
    public GameObject UContent;
    public GameObject AContent;
    public GameObject UniteCard;
    public GameObject ArmorCard;
    public GameObject DetailPanel;
    public GameObject DrawCount;

    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip[] cardDrawSoundList;

    public GameObject AtkIcon;
    public GameObject DefIcon;

    public float duration = 1.0f; // ��ת�����ĳ���ʱ��
    public int flipTimes = 3; // Ӳ����ת�Ĵ���
    private Unites unitModol;
    public int DrawTimes = 0;//��ǰ��մ���ʣ��

    public Camera mainCamera;
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    public Dictionary<string, GameObject> UniteSave = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ArmorSave = new Dictionary<string, GameObject>();
    public List<Sprite> Unitesprites = new List<Sprite>();
    public List<Sprite> Armorsprites = new List<Sprite>();

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (TurnBaseFSM.Instance.currentStateType == States.AttackDrawPile || TurnBaseFSM.Instance.currentStateType == States.AttackDrawRound)
        {
            AtkIcon.SetActive(true);
            DefIcon.SetActive(false);
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefenceDrawPile || TurnBaseFSM.Instance.currentStateType == States.DefenceDrawRound)
        {
            AtkIcon.SetActive(false);
            DefIcon.SetActive(true);
        }
        TextInfoSave();
    }
    void Start()
    {
        drewArmor.onClick.AddListener(DrewArmor);
        drewWeapon.onClick.AddListener(DrewWeapon);
       // finish.onClick.AddListener(DrawCardOver);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
       /* drewArmor.onClick.AddListener(() =>
        {
            DrewArmor();
            SoundPlay();
        });

        drewWeapon.onClick.AddListener(() =>
        {
            DrewWeapon();
            SoundPlay();
        });*/

        finish.onClick.AddListener(() =>
        {
            DrawCardOver();
            audioSource.PlayOneShot(buttonClickSound);
        });

        // ȷ�����������EventSystem����ȷ����
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (uiRaycaster == null)
        {
            uiRaycaster = FindObjectOfType<GraphicRaycaster>();
        }

        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

    }

    // Update is called once per frame   

    public void FixedUpdate()
    {
        var obj = GetObjectUnderMouse();
        if (obj != null)
        {
            DetailPanel.SetActive(true);

            RectTransform rectTransform = DetailPanel.GetComponent<RectTransform>();
            Vector3 mousePosition = Input.mousePosition;

            // ���Ŀ�Ⱥ͸߶�
            float panelWidth = rectTransform.rect.width;
            float panelHeight = rectTransform.rect.height;

            // �����������λ��
            Vector3 panelPosition;
            if (mousePosition.x < Screen.width / 2)
            {
                // �������Ļ��࣬�����ʾ���Ҳ�
                panelPosition = new Vector3(mousePosition.x + panelWidth / 2, mousePosition.y - panelHeight / 2, 0);
            }
            else
            {
                // �������Ļ�Ҳ࣬�����ʾ�����
                panelPosition = new Vector3(mousePosition.x - panelWidth / 2, mousePosition.y - panelHeight / 2, 0);
            }

            // ȷ����岻�ᳬ����Ļ��Χ
            panelPosition.x = Mathf.Clamp(panelPosition.x, panelWidth / 2, Screen.width - panelWidth / 2);
            panelPosition.y = Mathf.Clamp(panelPosition.y, panelHeight / 2, Screen.height - panelHeight / 2);

            rectTransform.position = panelPosition;
            /* DetailPanel.SetActive(true);
             //����������Ļ������������ʾ�����λ�õ��Ҳ࣬��֮����ʾ����࣬���𳬳���Ļ��Χ
             if (Input.mousePosition.x < Screen.width / 2)
             {
                 DetailPanel.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x + 150, Input.mousePosition.y-30, 0);
             }
             else
             {
                 DetailPanel.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x - 150, Input.mousePosition.y-30, 0);
             }*/

            Unites unit = obj.GetComponent<CardData>().unites;
            if (unit is Weapon)
            {
                DetailPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<sprite=0><color=red> Damage : " + unit.Damage + "\r\n<sprite=1><color=green> Defence :" + unit.Defence + "\r\n" +
                  TextShow(unit);


            }
            else if (unit is Armor)
            {
                DetailPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<sprite=1><color=green> Defence : " + unit.Defence + "\r\n<sprite=2><color=blue> Speed : " + unit.Speed + "\r\n" +
                    TextShow(unit);
            }
        }
        else
        {
            DetailPanel.SetActive(false);
        }
    }

    // ��SoundPlay�����������������������߼�
    private void SoundPlay()
    {
        // ����һ���������
        int randomIndex = UnityEngine.Random.Range(0, cardDrawSoundList.Length);

        // �������ѡ�������
        audioSource.PlayOneShot(cardDrawSoundList[randomIndex]);
    }
    private string TextShow(Unites unit)
    {
        if (unit is Weapon)
        {

            switch (unit.attackType)
            {
                case AttackType.SingleAttack:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is melee attack and will only attack target one square in front of it.";
                case AttackType.MeleeAttack_Min2Max:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is melee attack and will prioritize attacking enemy target that are closer to you in attack range.";
                case AttackType.MeleeAttack_Max2Min:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is melee attack and will prioritize attacking enemy target that are further away from you in the attack range.";
                case AttackType.RangedAttack_Max2Min:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is range attack and will prioritize attacking enemy target that are further away from you in the attack range.";
                case AttackType.RangedAttack_Min2Max:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is range attack and will prioritize attacking enemy target that are closer to you in attack range.";
                case AttackType.GroupAttack_HpHigh2Low:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is melee attack and attack all target in the row in front of him, starting with the highest defence target";
                case AttackType.GroupAttack_HpLow2High:
                    return "<color=yellow>The attack range of this Pawn is <color=green>" + unit.Range + "<color=yellow>\r\n This Pawn attacks is melee attack and attack all target in the row in front of him, starting with the Lowest defence target";


            }
        }
        else if (unit is Armor)
        {
            switch (unit.Speed)
            {
                case 1:
                    return "<color=white>This armor is very heavy, so the attacks of the unit wearing it will be made last.";
                case 2:
                    return "<color=white>This armor is of moderate weight, so the attack speed of the unit wearing it is normal.";
                case 3:
                    return "<color=white>This armor is very lightweight, so the attacks of the unit wearing it will be made first.";
            }
        }

        return null;
    }
    public void DrewWeapon()
    {
        if (DrawTimes <= 0)
        {
            return;
        }
        DrawTimes--;
        SoundPlay();
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
        if (DrawTimes <= 0)
        {
            return;
        }
        DrawTimes--;
        SoundPlay();
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
        if (unitModol is Weapon)
        {
            SetImageWeapon(obj, unitModol.Name);
        }
        else if (unitModol is Armor)
        {
            SetImageArmor(obj, unitModol.Name);
        }
        obj.GetComponent<CardData>().cardName = unitModol.Name;
    }
    public void SetImageWeapon(GameObject obj, string name)
    {
        switch (name)
        {
            case "Sword":
                obj.GetComponent<Image>().sprite = Unitesprites[0];
                break;
            case "Halberd":
                obj.GetComponent<Image>().sprite = Unitesprites[1];
                break;
            case "GreatSword":
                obj.GetComponent<Image>().sprite = Unitesprites[2];
                break;
            case "Spear":
                obj.GetComponent<Image>().sprite = Unitesprites[3];
                break;
            case "LongBow":
                obj.GetComponent<Image>().sprite = Unitesprites[4];
                break;
            case "CrossBow":
                obj.GetComponent<Image>().sprite = Unitesprites[5];
                break;


        }
    }
    public void SetImageArmor(GameObject obj, string name)
    {
        switch (name)
        {
            case "Cape":
                obj.GetComponent<Image>().sprite = Armorsprites[0];
                break;
            case "Breastplate":
                obj.GetComponent<Image>().sprite = Armorsprites[1];
                break;
            case "PlateArmor":
                obj.GetComponent<Image>().sprite = Armorsprites[2];
                break;
        }
    }
    private void TextInfoSave()
    {
        string text = null;
        if (TurnBaseFSM.Instance.currentStateType == States.AttackDrawPile || TurnBaseFSM.Instance.currentStateType == States.AttackDrawRound)
        {
           text = "You're the attacker. This is your turn to draw cards.";
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefenceDrawPile || TurnBaseFSM.Instance.currentStateType == States.DefenceDrawRound)
        {
            text = "You're the defender, This is your turn to draw cards..";
        }
        DrawCount.GetComponentInChildren<TextMeshProUGUI>().text = DrawTimes + " card draws left." + "\n" + text;
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
    {
        if (DrawTimes > 0)
        {
            return;
        }
        //�����鿨 �жϵ�ǰ�غ�״̬ �Բ�ͬ��״̬���벻ͬ�ĳ���
        if (TurnBaseFSM.Instance.currentStateType == States.AttackDrawPile || TurnBaseFSM.Instance.currentStateType == States.AttackDrawRound)
        {
            foreach (var item in UniteSave)
            {
                //if (item.Value.activeSelf == false)//���д���ע�͵���Ҳû���κη�Ӧ����
                //    continue;

                GameManager.Instance.AttackUnitePoolSave(item.Key, item.Value);
            }
            foreach (var item in ArmorSave)
            {
                //if (item.Value.activeSelf == false)
                //    continue;
                GameManager.Instance.AttackArmorPoolSave(item.Key, item.Value);
            }
            //Debug.Log(GameManager.Instance.AttackUnitePool.poolDictionary.Count);
            // Debug.Log(GameManager.Instance.AttackArmorPool.poolDictionary.Count);
            if (TurnBaseFSM.Instance.currentStateType == States.AttackDrawPile)
            {
                TurnBaseFSM.Instance.ChangeState(States.AttackConfiguration);//Ҫ�ǹ������ĻغϾͽ��빥�����ĺ���״̬
            }
            else
            {
                TurnBaseFSM.Instance.ChangeState(States.AttackConfigurationRound);
            }
            //�ǵ�����ֵ�
            UniteSave.Clear();
            ArmorSave.Clear();
        }
        else if (TurnBaseFSM.Instance.currentStateType == States.DefenceDrawPile || TurnBaseFSM.Instance.currentStateType == States.DefenceDrawRound)
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
            // Debug.Log(GameManager.Instance.DefenceUnitePool.poolDictionary.Count);
            //Debug.Log(GameManager.Instance.DefenceArmorPool.poolDictionary.Count);
            if (TurnBaseFSM.Instance.currentStateType == States.DefenceDrawPile)
            {
                TurnBaseFSM.Instance.ChangeState(States.DefenceConfiguration);//Ҫ�Ƿ��ط��ĻغϾͽ�����ط��ĺ���״̬
            }
            else
            {
                TurnBaseFSM.Instance.ChangeState(States.DefenceConfigurationRound);
            }
            //�ǵ�����ֵ�
            UniteSave.Clear();
            ArmorSave.Clear();
        }
    }

    private GameObject GetObjectUnderMouse()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerEventData, results);
        foreach (RaycastResult result in results)
        {
            // Debug.Log("UI Element: " + result.gameObject.name);
            if (result.gameObject.name.Contains("ArmorCard") || result.gameObject.name.Contains("UniteCard"))
            {
                return result.gameObject;
            }
        }
        return null;
    }
}
