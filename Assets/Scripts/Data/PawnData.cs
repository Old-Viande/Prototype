using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PawnData : MonoBehaviour
{
    public string Name;
    public Unites Weaopn;//������ֵ���������������н��д��棬�����Ѿ������ȷʵ���������
    public Unites Armor;//������ֵ���������������н��д��棬�����Ѿ������ȷʵ���������
    public Unites Unites;//������ֵ���������������н��д��棬�����Ѿ������ȷʵ���������
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
