using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Unite", menuName = "SO/Unite")]
public class UniteTypeSO : ScriptableObject
{
    //写一段使用list可以在外面添加新的单位类型的代码
    public List<Weapon> uniteTypeList = new List<Weapon>();

        
}
