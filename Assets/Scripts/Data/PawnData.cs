using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PawnData : MonoBehaviour
{
    public string Name;
    public Unites Weaopn;//这个部分的数据在配置面板中进行储存，并且已经检测了确实有这个数据
    public Unites Armor;//这个部分的数据在配置面板中进行储存，并且已经检测了确实有这个数据
    public Unites Unites;//这个部分的数据在配置面板中进行储存，并且已经检测了确实有这个数据
    public string Shield;
    public int Defence;

    public void Start()
    {
        if (this.transform.Find("Text (TMP)"))
        {
            this.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = Name;
        }
    }
    public void PawnSet()
    {

        Debug.Log(Weaopn);
        Debug.Log(Armor);
        Defence = Unites.Defence;
    }
}
