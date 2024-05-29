using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap <TGameobject> 
{
    
    public Dictionary<Vector2, TGameobject> gridmap =  new Dictionary<Vector2, TGameobject>();
    public Vector3 pointPosition;
    public float celllong;
    //���ɹ��캯����ı���
    public int maxwidth;
    public int maxheight;
    public int minwidth;
    public int minheight;

    public GridMap(int maxwidth, int maxheight, int minwidth, int minheight, float celllong,Vector3 pointPosition, Func<GridMap<TGameobject>, int, int, TGameobject> createGrid) 
    {   //������Ĳ�����ֵ�����캯����ı���
        this.maxwidth = maxwidth;
        this.maxheight = maxheight;
        this.minwidth = minwidth;
        this.minheight = minheight;
        this.celllong = celllong;
        this.pointPosition = pointPosition;
        for (int x = minwidth; x < maxwidth; x++)
        {
            for (int z = minheight; z < maxheight; z++)
            {
                gridmap.Add(new Vector2(x, z), createGrid(this, x, z));
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
            }
        }
    }
    public Vector3 GetWorldPosition(int x, int z)//������������ת����������ֵ 
    {
        return new Vector3(x,0, z) * celllong + pointPosition;
    }
    public Vector3 GetGridCenter(int x, int z)
    {
        return GetWorldPosition(x, z) + new Vector3(celllong / 2f, 0, celllong / 2f);
    }
    public void GetGridXZ(Vector3 WorldPosition, out int x, out int z)//������������ֵת����Ӧ����������
    {
        x = Mathf.FloorToInt((WorldPosition - pointPosition).x / celllong);//mathf.floortoint ��������ȡ����
        z = Mathf.FloorToInt((WorldPosition - pointPosition).z / celllong);
    }

    public void SetValue(int x, int z, TGameobject value)//�ֵ�汾����ֵ,Ҳ���������������������µĸ���(�°汾)
    {
        if (!gridmap.ContainsKey(new Vector2(x, z)))
        {
            gridmap.Add(new Vector2(x, z), value);
           
        }
        else
        {

            gridmap.Remove(new Vector2(x, z));
            gridmap.Add(new Vector2(x, z), value);
        }
    }

    public void SetValue(Vector3 WorldPosition, TGameobject value)
    {
        int x, z;
        GetGridXZ(WorldPosition, out x, out z);
        SetValue(x, z, value);

    }
    public TGameobject GetValue(int x, int z)//�ֵ�汾�Ķ�ȡ����(�°汾)
    {
        TGameobject Value;

        if (gridmap.TryGetValue(new Vector2(x, z), out Value))
            return Value;
        else
            return default(TGameobject);
    }
    public TGameobject GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetGridXZ(worldPosition, out x, out z);
        return GetValue(x, z);

    }
}
