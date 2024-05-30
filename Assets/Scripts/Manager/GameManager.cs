using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
//序列化
[System.Serializable]
public class GameManager : Singleton<GameManager>
{   //解释：这个类是一个单例类，用来管理游戏的一些全局变量，比如玩家的身份，玩家的手牌，玩家的牌库等等
    //这里还会储存一些数据
    // Start is called before the first frame update
    //在这里生成一个用来储存卡牌的功能 
    public GameObject floor;
    public GridMap<GameObject> floorGridMap;//网格生成地板
    public GridMap<GameObject> unitesGridMap;//单位生成地板
    public PathFinder pathFinder;//寻路算法
    public UniteTypeSO uniteTypeSO;//单位类型储存，牌库系统需要通过这个来获取单位的类型，并调整可以获取到的单位类型
    public ArmorTypeSO armorTypeSO;//装备类型储存，牌库系统需要通过这个来获取装备的类型，并调整可以获取到的装备类型
    public Dictionary<string, int> UniteTypeSave = new Dictionary<string, int>();//从牌库系统中获取到单位卡牌类型，并提供给抽取系统进行抽取
    public Dictionary<string, int> ArmorTypeSave = new Dictionary<string, int>();//从牌库系统中获取到护甲卡牌类型，并提供给抽取系统进行抽取
    /*public Dictionary<string, Weapon> unitesDict = new Dictionary<string, Weapon>();//用来储存所有的单位牌
    public Dictionary<string, Armor> armorDict = new Dictionary<string, Armor>();//用来储存所有的装备牌*/
    public Dictionary<string, GameObject> CharacterDict = new Dictionary<string, GameObject>();//用来储存所有配置完成的单位
    public Unites currentUnit;//用来储存当前单位
    #region 移动和攻击相关字典和变量
    public Dictionary<Vector2, GameObject> AttackMovelis = new Dictionary<Vector2, GameObject>();//用来储存攻击方单位移动
    public Dictionary<Vector2, GameObject> DefenceMovelis = new Dictionary<Vector2, GameObject>();//用来储存防守方单位移动
    public Dictionary<int, GameObject> Attacklis = new Dictionary<int, GameObject>();//用来储存所有攻击单位
    //public Dictionary<int, GameObject> Defencelis = new Dictionary<int, GameObject>();//用来储存所有防守单位
    public Unites weatherEffect;//用来储存天气造成的效果
    #endregion
    public float celllong = 4.0f;
    public int maxX = 11;
    public int maxY = 3;
    public int minX = 0;
    public int minY = 0;
    public Vector3 origenPoint = Vector3.zero;
    public Weathere weathere;
    public List<Weathere> weatheres = new List<Weathere>();
    #region 玩家双方对象池相关
    //储存已经抽出的单位牌的对象池
    public ObjectPool AttackUnitePool;//攻击方单位牌对象池
    public ObjectPool DefenceUnitePool;//防守方单位牌对象池
    //储存已经抽出的护甲牌的对象池
    public ObjectPool AttackArmorPool;
    public ObjectPool DefenceArmorPool;
    //储存已经生成的单位的对象池
    public ObjectPool AttackPawnPool;
    public ObjectPool DefencePawnPool;
    #endregion
    public ObjectPool abandonedPool;//储存被废弃的单位
    void Start()
    {
        floorGridMap = new GridMap<GameObject>(maxX, maxY, minX, minY, celllong, origenPoint, floorSave);
        unitesGridMap = new GridMap<GameObject>(maxX, maxY, minX, minY, celllong, origenPoint, UniteClear);
        pathFinder = new PathFinder(maxX, maxY, minX, minY, celllong, origenPoint);
        //对象池的初始化
        AttackUnitePool = new ObjectPool();
        DefenceUnitePool = new ObjectPool();
        AttackArmorPool = new ObjectPool();
        DefenceArmorPool = new ObjectPool();
        AttackPawnPool = new ObjectPool();
        DefencePawnPool = new ObjectPool();
        abandonedPool = new ObjectPool();
    }

    private GameObject floorSave(GridMap<GameObject> map, int x, int z)
    {
        return Instantiate(floor, map.GetGridCenter(x, z), Quaternion.identity);
    }
    private GameObject UniteClear(GridMap<GameObject> map, int x, int z)
    {
        return null;
    }
    //进行牌库的初始化
    public void CardPealInit()
    {
        if (uniteTypeSO.uniteTypeList.Count != 0)
        {
            foreach (var item in uniteTypeSO.uniteTypeList)
            {
                UniteTypeSave.Add(item.Name, 4);
            }
        }
        if (armorTypeSO.ArmorTypeList.Count != 0)
        {
            foreach (var item in armorTypeSO.ArmorTypeList)
            {
                ArmorTypeSave.Add(item.Name, 4);
            }
        }
    }
    public string DrewUnite()
    {
        return CardPealDrew(uniteTypeSO);
    }
    public string DrewArmor()
    {
        return CardPealDrew(armorTypeSO);
    }
    //进行牌库的抽取,重载函数
    private string CardPealDrew(UniteTypeSO uniteType)
    {   //带有武器的单位牌抽取
        //对单位牌进行抽取，从unitTypeSave字典中抽取一个单位名，然后将存量减1
        if (UniteTypeSave.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, UniteTypeSave.Count);
            string key = UniteTypeSave.ToArray()[index].Key.ToString();
            //从单位类型中获取到单位名对应的单位类型
            Weapon weapon = uniteType.uniteTypeList.Find(x => x.Name == key);
            //考虑到SO是从资源文件夹中读取的，所以这里需要进行深拷贝后再进行储存!!!!
            currentUnit = weapon;
            //unitesDict.Add(key, weapon);
            UniteTypeSave[key]--;
            return currentUnit.Name;
        }
        return null;

    }
    private string CardPealDrew(ArmorTypeSO armorType)
    {//护甲牌抽取
        //对护甲牌进行抽取，从armorTypeSave字典中抽取一个护甲名，然后将存量减1
        if (ArmorTypeSave.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, ArmorTypeSave.Count);
            string key = ArmorTypeSave.ToArray()[index].Key.ToString();
            //从护甲类型中获取到护甲名对应的护甲类型
            Armor armor = armorType.ArmorTypeList.Find(x => x.Name == key);
            //考虑到SO是从资源文件夹中读取的，所以这里需要进行深拷贝后再进行储存!!!!
            currentUnit = armor;
            //armorDict.Add(key, armor);
            ArmorTypeSave[key]--;
            return currentUnit.Name;
        }
        return null;
    }
    //进行牌库的弃牌
    public void UniteDiscard(Unites unites)
    {//如果单位名在cardTypeSave字典内存在，则将存量加1，否则将单位名加入字典并存量设为1
        if (UniteTypeSave.ContainsKey(unites.Name))
        {
            UniteTypeSave[unites.Name]++;
        }
        else
        {
            UniteTypeSave.Add(unites.Name, 1);
        }

    }
    public void ArmorDiscard(Armor armor)
    { //如果单位名在cardTypeSave字典内存在，则将存量加1，否则将单位名加入字典并存量设为1
        if (ArmorTypeSave.ContainsKey(armor.Name))
        {
            ArmorTypeSave[armor.Name]++;
        }
        else
        {
            ArmorTypeSave.Add(armor.Name, 1);
        }
    }


    //检查牌库内是否有卡片类型存量为0
    public void UniteCardPealCheck()
    {
        foreach (var item in UniteTypeSave)
        {
            if (item.Value == 0)
            {
                //如果存量为0则删除这个卡片类型
                UniteTypeSave.Remove(item.Key);
            }
        }
    }
    //护甲牌库的初始化  
    public void ArmorCardPealCheck()
    {
        foreach (var item in ArmorTypeSave)
        {
            if (item.Value == 0)
            {
                //如果存量为0则删除这个卡片类型
                ArmorTypeSave.Remove(item.Key);
            }
        }
    }
    #region 天气系统相关
    //在这里进行天气的随机抽取
    public void RandomWeather()
    {
        weathere = (Weathere)UnityEngine.Random.Range(0, 7);
        weatheres.Add(weathere);

    }
    #endregion
    #region Attack对象池相关
    public void AttackUnitePoolSave(string key, GameObject obj)
    {
        AttackUnitePool.ReturnObject(key, obj);
    }
    public void AttackArmorPoolSave(string key, GameObject obj)
    {
       AttackArmorPool.ReturnObject(key, obj);
    }
    public void AttackPawnPoolSave(string key, GameObject obj)
    {
        AttackPawnPool.ReturnObject(key, obj);
    }
    public GameObject AttackUnitePoolGet(string key)
    {
        return AttackUnitePool.GetObject(key);
    }
    public GameObject AttackArmorPoolGet(string key)
    {
        return AttackArmorPool.GetObject(key);
    }
    public GameObject AttackPawnPoolGet(string key)
    {
        return AttackPawnPool.GetObject(key);
    }
    #endregion
    #region Defence对象池相关
    public void DefenceUnitePoolSave(string key, GameObject obj)
    {
        DefenceUnitePool.ReturnObject(key, obj);
    }
    public void DefenceArmorPoolSave(string key, GameObject obj)
    {
        DefenceArmorPool.ReturnObject(key, obj);
    }
    public void DefencePawnPoolSave(string key, GameObject obj)
    {
        DefencePawnPool.ReturnObject(key, obj);
    }
    public GameObject DefenceUnitePoolGet(string key)
    {
        return DefenceUnitePool.GetObject(key);
    }
    public GameObject DefenceArmorPoolGet(string key)
    {
        return DefenceArmorPool.GetObject(key);
    }
    public GameObject DefencePawnPoolGet(string key)
    {
        return DefencePawnPool.GetObject(key);
    }
    #endregion
    #region 场上单位相关
    public Type PawnTypeCheck(string Pawnname)
    {
        //一个Switch通过单位名称进行判断
        switch (Pawnname)
        {
            //检测数据的名字
            case "Sword":
                return typeof(Sword);

            case "Spear":
                return typeof(Spear);

            case "Halberd":
                return typeof(Halberd);

            case "LongBow":
                return typeof(LongBow);

            case "CrossBow":
                return typeof(CrossBow);

            case "GreatSword":
                return typeof(GreatSword);

        }

        return null;

    }

    public Component AddComponent(GameObject obj, Type type)
    {
        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            return obj.AddComponent(type);
        }
        else
        {
            Debug.LogWarning("Type must be a subclass of MonoBehaviour.");
            return null;
        }
    }

    #endregion    
    #region 单位攻击顺序安排
    public List<KeyValuePair<int, GameObject>> AttackSetOrder()
    {
        var sortedList = Attacklis.OrderByDescending(kv => kv.Key) // 按照key的大小进行排序                                
                                  .ToList();
        return sortedList;
    }

    public void AttackSettlement(GameObject damage, GameObject defence)
    {
        //当该功能被调用时，需要先将调用者和被调用者的单位进行传入
        //然后进行攻击方和防守方的数据获取
        Unites Au = damage.GetComponent<PawnData>().Unites;
        Unites Du = defence.GetComponent<PawnData>().Unites;
        int Defence = defence.GetComponent<PawnData>().Defence;
        int Damage = damage.GetComponent<PawnData>().Unites.Damage;
        Defence += weatherEffect.Defence;//防御值收到天气的影响
        Defence-=Damage+weatherEffect.Damage;//伤害值收到天气的影响
        if (Defence <= 0&&Au.Speed>Du.Speed)
        {
            //如果防守方的防御力小于等于0，并且攻击方的速度大于防守方的速度，则将防守方的单位立刻移除
            unitesGridMap.GetGridXZ(defence.transform.position, out int x, out int z);
            unitesGridMap.SetValue(x, z, null);
            defence.SetActive(false);
        }
        else
        {
        //单位并未立刻击杀，则是将防守方的防御力减去攻击方的伤害值，并将防守方的防御力赋值给防守方的单位
        defence.GetComponent<PawnData>().Defence = Defence;
        }
    }

    public void AttackSettlement(GameObject damage, GameObject[] defencelis)
    {
        Unites Au = damage.GetComponent<PawnData>().Unites;
        int Damage = damage.GetComponent<PawnData>().Unites.Damage;
        Damage += weatherEffect.Damage;//伤害值收到天气的影响
        foreach (var item in defencelis)
        {
          Unites Du = item.GetComponent<PawnData>().Unites;
            int Defence = item.GetComponent<PawnData>().Defence;
            Defence += weatherEffect.Defence;//防御值收到天气的影响
            Defence -= Damage;
            if (Defence <= 0 && Au.Speed > Du.Speed)
            {
                //如果防守方的防御力小于等于0，并且攻击方的速度大于防守方的速度，则将防守方的单位立刻移除
                unitesGridMap.GetGridXZ(item.transform.position, out int x, out int z);
                unitesGridMap.SetValue(x, z, null);
                item.SetActive(false);
            }
            else
            {
                item.GetComponent<PawnData>().Defence = Defence;
                Damage -= Defence;
            }

        }
    }
    
    public void UniteRoundEnd()
    {
        foreach (var item in Attacklis)
        {
            if (item.Value.GetComponent<PawnData>().Defence<=0)
            {
                //对单位进行消除操作
                unitesGridMap.GetGridXZ(item.Value.transform.position, out int x, out int z);
                unitesGridMap.SetValue(x, z, null);
                item.Value.SetActive(false);
            }
        }
    }
    #endregion
    #region 玩家单位移动排序功能
    public void AttackPawnMoveOrder()
    { // 按照键值中的Vector2.x进行排序
        List<KeyValuePair<Vector2, GameObject>> sortedAttackLis = GetATKSortedPieces();
        /* for (int i = AttackMovelis.Count - 1; i >= 0; i--)
         {
             GameObject obj = AttackMovelis.ToArray()[i].Value;  
             Vector3 point = obj.transform.position;
             unitesGridMap.GetGridXZ(point, out int x, out int z);
             Vector2 coordinate = AttackPawnPointSet(x, z,obj);            
             AttackMovelis.ToArray()[i].Value.GetComponent<BaseAction>().coordinate = coordinate;
         }*/
        for (int i = 0; i < AttackMovelis.Count; i++)
        {
            GameObject obj = sortedAttackLis[i].Value;
            Vector3 point = obj.transform.position;
            unitesGridMap.GetGridXZ(point, out int x, out int z);
            Vector2 coordinate = AttackPawnPointSet(x, z, obj);
            sortedAttackLis.ToArray()[i].Value.GetComponent<BaseAction>().coordinate = coordinate;
        }
    }

    public List<KeyValuePair<Vector2, GameObject>> GetATKSortedPieces()
    {
        // 将棋子按 y 坐标（z 坐标）从大到小，再按 x 坐标从大到小排序
        var sortedList = AttackMovelis.OrderByDescending(kv => kv.Key.x) // 首先按 x 坐标排序（从大到小）
                                  
                                   .ToList();

        return sortedList;
    }


    public Vector2 AttackPawnPointSet(int x, int z,GameObject obj)
    {
        // 尝试向右移动
        if (x < maxX - 1)
        {
            // 检查右侧是否为空
            if (unitesGridMap.GetValue(x + 1, z) == null)
            {
                unitesGridMap.SetValue(x, z, null);
                unitesGridMap.SetValue(x+1, z, obj);
                return new Vector2(x + 1, z);
            }
        }

        // 如果无法向右移动（右侧有物体），则返回原位置
        return new Vector2(x, z);
    }

    public void DefencePawnMoveOrder()
    {
        List<KeyValuePair<Vector2, GameObject>> sortedDefenceLis = GetDEFSortedPieces();
        /* for (int i = 0 ; i < DefenceMovelis.Count; i++)
         {
             GameObject obj = DefenceMovelis.ToArray()[i].Value;
             Vector3 point = obj.transform.position;
             unitesGridMap.GetGridXZ(point, out int x, out int z);
             Vector2 coordinate = DefencePawnPointSet(x, z, obj);
             DefenceMovelis.ToArray()[i].Value.GetComponent<BaseAction>().coordinate = coordinate;
         }*/
        for (int i = DefenceMovelis.Count - 1; i >= 0; i--)
        {
            GameObject obj = sortedDefenceLis[i].Value;
            Vector3 point = obj.transform.position;
            unitesGridMap.GetGridXZ(point, out int x, out int z);
            Vector2 coordinate = DefencePawnPointSet(x, z, obj);
            sortedDefenceLis.ToArray()[i].Value.GetComponent<BaseAction>().coordinate = coordinate;
        }
    }
    public List<KeyValuePair<Vector2, GameObject>> GetDEFSortedPieces()
    {
        // 将棋子按 y 坐标（z 坐标）从大到小，再按 x 坐标从大到小排序
        var sortedList = DefenceMovelis.OrderByDescending(kv => kv.Key.x) // 首先按 y 坐标排序（从大到小）
                                   
                                   .ToList();

        return sortedList;
    }
    private Vector2 DefencePawnPointSet(int x, int z, GameObject obj)
    {
        // 尝试向左移动
        if (x > minX - 1)
        {
            // 检查左侧是否为空
            if (unitesGridMap.GetValue(x - 1, z) == null)
            {
                unitesGridMap.SetValue(x, z, null);
                unitesGridMap.SetValue(x - 1, z, obj);
                return new Vector2(x - 1, z);
            }
           
        }

        // 如果无法向左移动（左侧有物体），则返回原位置
        return new Vector2(x, z);
    }

    #endregion


}
