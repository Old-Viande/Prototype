using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap <TGameobject> 
{
    
    public Dictionary<Vector2, TGameobject> gridmap =  new Dictionary<Vector2, TGameobject>();
    public Vector3 pointPosition;
    public float celllong;
    //生成构造函数里的变量
    public int maxwidth;
    public int maxheight;
    public int minwidth;
    public int minheight;

    public GridMap(int maxwidth, int maxheight, int minwidth, int minheight, float celllong,Vector3 pointPosition, Func<GridMap<TGameobject>, int, int, TGameobject> createGrid) 
    {   //将输入的参数赋值给构造函数里的变量
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
    public Vector3 GetWorldPosition(int x, int z)//根据网格坐标转化世界坐标值 
    {
        return new Vector3(x,0, z) * celllong + pointPosition;
    }
    public Vector3 GetGridCenter(int x, int z)
    {
        return GetWorldPosition(x, z) + new Vector3(celllong / 2f, 0, celllong / 2f);
    }
    public void GetGridXZ(Vector3 WorldPosition, out int x, out int z)//根据世界坐标值转换对应的网格坐标
    {
        x = Mathf.FloorToInt((WorldPosition - pointPosition).x / celllong);//mathf.floortoint 的作用是取整数
        z = Mathf.FloorToInt((WorldPosition - pointPosition).z / celllong);
    }

    public void SetValue(int x, int z, TGameobject value)//字典版本存入值,也可以用来在网格内增加新的格子(新版本)
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
    public TGameobject GetValue(int x, int z)//字典版本的读取数据(新版本)
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
