using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//–Ú¡–ªØ
[System.Serializable]
public class CardData : MonoBehaviour
{
   public Unites unites;
   public string cardName;
   public int Damage;
   public int Defence;
   public int Speed;
   public int Range;
   public AttackType attackType;
   public bool Canshield;
   public bool CanCross;

  /*  public void OnEnable()
    {
        this.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = cardName;

    }*/
    public void Start()
    {
        this.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = cardName;
       /* Damage = unites.Damage;
        Defence = unites.Defence;
        Speed = unites.Speed;
        Range = unites.Range;
        attackType = unites.attackType;
        Canshield = unites.Canshield;
        CanCross = unites.CanCross;*/
    }
}
