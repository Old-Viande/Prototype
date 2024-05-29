using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode 
{
    private GridMap<PathNode> grid;
    public int x;
    public int z;
    public bool canWalk = true;
    public GridNode lastOne;
    public int g;
    public int h;
    public int f;
    public GridNode(GridMap<PathNode> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }
    public void Getf()
    {
        f = g + h;
    }

    public void SetLastOne(GridNode lastOne)
    {
        this.lastOne = lastOne;
    }



}
