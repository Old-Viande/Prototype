using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Armor", menuName = "SO/Armor")]
public class ArmorTypeSO : ScriptableObject
{
    public List<Armor> ArmorTypeList = new List<Armor>();
}
