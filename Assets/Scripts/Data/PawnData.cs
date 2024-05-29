using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PawnData : MonoBehaviour
{
    public string Name;
    public Unites Weaopn;
    public Unites Armor;
    public Unites Unites;
    public string Shield;
    public int Defence;

    public void Start()
    {  
        if(this.transform.Find("Text (TMP)"))
        {
        this.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = Name;
        }      
        Defence = Unites.Defence;
    }
}
