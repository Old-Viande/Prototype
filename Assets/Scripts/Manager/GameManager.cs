using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
//���л�
[System.Serializable]
public class GameManager : Singleton<GameManager>
{   //���ͣ��������һ�������࣬����������Ϸ��һЩȫ�ֱ�����������ҵ���ݣ���ҵ����ƣ���ҵ��ƿ�ȵ�
    //���ﻹ�ᴢ��һЩ����
    // Start is called before the first frame update
    //����������һ���������濨�ƵĹ��� 
    public GameObject floor;
    public GridMap<GameObject> floorGridMap;//�������ɵذ�
    public GridMap<GameObject> unitesGridMap;//��λ���ɵذ�
    public PathFinder pathFinder;//Ѱ·�㷨
    public UniteTypeSO uniteTypeSO;//��λ���ʹ��棬�ƿ�ϵͳ��Ҫͨ���������ȡ��λ�����ͣ����������Ի�ȡ���ĵ�λ����
    public ArmorTypeSO armorTypeSO;//װ�����ʹ��棬�ƿ�ϵͳ��Ҫͨ���������ȡװ�������ͣ����������Ի�ȡ����װ������
    public Dictionary<string, int> UniteTypeSave = new Dictionary<string, int>();//���ƿ�ϵͳ�л�ȡ����λ�������ͣ����ṩ����ȡϵͳ���г�ȡ
    public Dictionary<string, int> ArmorTypeSave = new Dictionary<string, int>();//���ƿ�ϵͳ�л�ȡ�����׿������ͣ����ṩ����ȡϵͳ���г�ȡ
    /*public Dictionary<string, Weapon> unitesDict = new Dictionary<string, Weapon>();//�����������еĵ�λ��
    public Dictionary<string, Armor> armorDict = new Dictionary<string, Armor>();//�����������е�װ����*/
    public Dictionary<string, GameObject> CharacterDict = new Dictionary<string, GameObject>();//������������������ɵĵ�λ
    public Unites currentUnit;//�������浱ǰ��λ
    #region �ƶ��͹�������ֵ�ͱ���
    public Dictionary<Vector2, GameObject> AttackMovelis = new Dictionary<Vector2, GameObject>();//�������湥������λ�ƶ�
    public Dictionary<Vector2, GameObject> DefenceMovelis = new Dictionary<Vector2, GameObject>();//����������ط���λ�ƶ�
    public Dictionary<int, GameObject> Attacklis = new Dictionary<int, GameObject>();//�����������й�����λ
    //public Dictionary<int, GameObject> Defencelis = new Dictionary<int, GameObject>();//�����������з��ص�λ
    public Unites weatherEffect;//��������������ɵ�Ч��
    #endregion
    public float celllong = 4.0f;
    public int maxX = 11;
    public int maxY = 3;
    public int minX = 0;
    public int minY = 0;
    public Vector3 origenPoint = Vector3.zero;
    public Weathere weathere;
    public List<Weathere> weatheres = new List<Weathere>();
    #region ���˫����������
    //�����Ѿ�����ĵ�λ�ƵĶ����
    public ObjectPool AttackUnitePool;//��������λ�ƶ����
    public ObjectPool DefenceUnitePool;//���ط���λ�ƶ����
    //�����Ѿ�����Ļ����ƵĶ����
    public ObjectPool AttackArmorPool;
    public ObjectPool DefenceArmorPool;
    //�����Ѿ����ɵĵ�λ�Ķ����
    public ObjectPool AttackPawnPool;
    public ObjectPool DefencePawnPool;
    #endregion
    public ObjectPool abandonedPool;//���汻�����ĵ�λ
    void Start()
    {
        floorGridMap = new GridMap<GameObject>(maxX, maxY, minX, minY, celllong, origenPoint, floorSave);
        unitesGridMap = new GridMap<GameObject>(maxX, maxY, minX, minY, celllong, origenPoint, UniteClear);
        pathFinder = new PathFinder(maxX, maxY, minX, minY, celllong, origenPoint);
        //����صĳ�ʼ��
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
    //�����ƿ�ĳ�ʼ��
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
    //�����ƿ�ĳ�ȡ,���غ���
    private string CardPealDrew(UniteTypeSO uniteType)
    {   //���������ĵ�λ�Ƴ�ȡ
        //�Ե�λ�ƽ��г�ȡ����unitTypeSave�ֵ��г�ȡһ����λ����Ȼ�󽫴�����1
        if (UniteTypeSave.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, UniteTypeSave.Count);
            string key = UniteTypeSave.ToArray()[index].Key.ToString();
            //�ӵ�λ�����л�ȡ����λ����Ӧ�ĵ�λ����
            Weapon weapon = uniteType.uniteTypeList.Find(x => x.Name == key);
            //���ǵ�SO�Ǵ���Դ�ļ����ж�ȡ�ģ�����������Ҫ����������ٽ��д���!!!!
            currentUnit = weapon;
            //unitesDict.Add(key, weapon);
            UniteTypeSave[key]--;
            return currentUnit.Name;
        }
        return null;

    }
    private string CardPealDrew(ArmorTypeSO armorType)
    {//�����Ƴ�ȡ
        //�Ի����ƽ��г�ȡ����armorTypeSave�ֵ��г�ȡһ����������Ȼ�󽫴�����1
        if (ArmorTypeSave.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, ArmorTypeSave.Count);
            string key = ArmorTypeSave.ToArray()[index].Key.ToString();
            //�ӻ��������л�ȡ����������Ӧ�Ļ�������
            Armor armor = armorType.ArmorTypeList.Find(x => x.Name == key);
            //���ǵ�SO�Ǵ���Դ�ļ����ж�ȡ�ģ�����������Ҫ����������ٽ��д���!!!!
            currentUnit = armor;
            //armorDict.Add(key, armor);
            ArmorTypeSave[key]--;
            return currentUnit.Name;
        }
        return null;
    }
    //�����ƿ������
    public void UniteDiscard(Unites unites)
    {//�����λ����cardTypeSave�ֵ��ڴ��ڣ��򽫴�����1�����򽫵�λ�������ֵ䲢������Ϊ1
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
    { //�����λ����cardTypeSave�ֵ��ڴ��ڣ��򽫴�����1�����򽫵�λ�������ֵ䲢������Ϊ1
        if (ArmorTypeSave.ContainsKey(armor.Name))
        {
            ArmorTypeSave[armor.Name]++;
        }
        else
        {
            ArmorTypeSave.Add(armor.Name, 1);
        }
    }


    //����ƿ����Ƿ��п�Ƭ���ʹ���Ϊ0
    public void UniteCardPealCheck()
    {
        foreach (var item in UniteTypeSave)
        {
            if (item.Value == 0)
            {
                //�������Ϊ0��ɾ�������Ƭ����
                UniteTypeSave.Remove(item.Key);
            }
        }
    }
    //�����ƿ�ĳ�ʼ��  
    public void ArmorCardPealCheck()
    {
        foreach (var item in ArmorTypeSave)
        {
            if (item.Value == 0)
            {
                //�������Ϊ0��ɾ�������Ƭ����
                ArmorTypeSave.Remove(item.Key);
            }
        }
    }
    #region ����ϵͳ���
    //��������������������ȡ
    public void RandomWeather()
    {
        weathere = (Weathere)UnityEngine.Random.Range(0, 7);
        weatheres.Add(weathere);

    }
    #endregion
    #region Attack��������
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
    #region Defence��������
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
    #region ���ϵ�λ���
    public Type PawnTypeCheck(string Pawnname)
    {
        //һ��Switchͨ����λ���ƽ����ж�
        switch (Pawnname)
        {
            //������ݵ�����
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
    #region ��λ����˳����
    public List<KeyValuePair<int, GameObject>> AttackSetOrder()
    {
        var sortedList = Attacklis.OrderByDescending(kv => kv.Key) // ����key�Ĵ�С��������                                
                                  .ToList();
        return sortedList;
    }

    public void AttackSettlement(GameObject damage, GameObject defence)
    {
        //���ù��ܱ�����ʱ����Ҫ�Ƚ������ߺͱ������ߵĵ�λ���д���
        //Ȼ����й������ͷ��ط������ݻ�ȡ
        Unites Au = damage.GetComponent<PawnData>().Unites;
        Unites Du = defence.GetComponent<PawnData>().Unites;
        int Defence = defence.GetComponent<PawnData>().Defence;
        int Damage = damage.GetComponent<PawnData>().Unites.Damage;
        Defence += weatherEffect.Defence;//����ֵ�յ�������Ӱ��
        Defence-=Damage+weatherEffect.Damage;//�˺�ֵ�յ�������Ӱ��
        if (Defence <= 0&&Au.Speed>Du.Speed)
        {
            //������ط��ķ�����С�ڵ���0�����ҹ��������ٶȴ��ڷ��ط����ٶȣ��򽫷��ط��ĵ�λ�����Ƴ�
            unitesGridMap.GetGridXZ(defence.transform.position, out int x, out int z);
            unitesGridMap.SetValue(x, z, null);
            defence.SetActive(false);
        }
        else
        {
        //��λ��δ���̻�ɱ�����ǽ����ط��ķ�������ȥ���������˺�ֵ���������ط��ķ�������ֵ�����ط��ĵ�λ
        defence.GetComponent<PawnData>().Defence = Defence;
        }
    }

    public void AttackSettlement(GameObject damage, GameObject[] defencelis)
    {
        Unites Au = damage.GetComponent<PawnData>().Unites;
        int Damage = damage.GetComponent<PawnData>().Unites.Damage;
        Damage += weatherEffect.Damage;//�˺�ֵ�յ�������Ӱ��
        foreach (var item in defencelis)
        {
          Unites Du = item.GetComponent<PawnData>().Unites;
            int Defence = item.GetComponent<PawnData>().Defence;
            Defence += weatherEffect.Defence;//����ֵ�յ�������Ӱ��
            Defence -= Damage;
            if (Defence <= 0 && Au.Speed > Du.Speed)
            {
                //������ط��ķ�����С�ڵ���0�����ҹ��������ٶȴ��ڷ��ط����ٶȣ��򽫷��ط��ĵ�λ�����Ƴ�
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
                //�Ե�λ������������
                unitesGridMap.GetGridXZ(item.Value.transform.position, out int x, out int z);
                unitesGridMap.SetValue(x, z, null);
                item.Value.SetActive(false);
            }
        }
    }
    #endregion
    #region ��ҵ�λ�ƶ�������
    public void AttackPawnMoveOrder()
    { // ���ռ�ֵ�е�Vector2.x��������
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
        // �����Ӱ� y ���꣨z ���꣩�Ӵ�С���ٰ� x ����Ӵ�С����
        var sortedList = AttackMovelis.OrderByDescending(kv => kv.Key.x) // ���Ȱ� x �������򣨴Ӵ�С��
                                  
                                   .ToList();

        return sortedList;
    }


    public Vector2 AttackPawnPointSet(int x, int z,GameObject obj)
    {
        // ���������ƶ�
        if (x < maxX - 1)
        {
            // ����Ҳ��Ƿ�Ϊ��
            if (unitesGridMap.GetValue(x + 1, z) == null)
            {
                unitesGridMap.SetValue(x, z, null);
                unitesGridMap.SetValue(x+1, z, obj);
                return new Vector2(x + 1, z);
            }
        }

        // ����޷������ƶ����Ҳ������壩���򷵻�ԭλ��
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
        // �����Ӱ� y ���꣨z ���꣩�Ӵ�С���ٰ� x ����Ӵ�С����
        var sortedList = DefenceMovelis.OrderByDescending(kv => kv.Key.x) // ���Ȱ� y �������򣨴Ӵ�С��
                                   
                                   .ToList();

        return sortedList;
    }
    private Vector2 DefencePawnPointSet(int x, int z, GameObject obj)
    {
        // ���������ƶ�
        if (x > minX - 1)
        {
            // �������Ƿ�Ϊ��
            if (unitesGridMap.GetValue(x - 1, z) == null)
            {
                unitesGridMap.SetValue(x, z, null);
                unitesGridMap.SetValue(x - 1, z, obj);
                return new Vector2(x - 1, z);
            }
           
        }

        // ����޷������ƶ�����������壩���򷵻�ԭλ��
        return new Vector2(x, z);
    }

    #endregion


}
