using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PathNode is a class that represents a node in the grid. It contains information about the node's position, whether it can be walked on, and the cost of moving to and from the node.

public class SPathNode
{
    private GridMap<SPathNode> grid;
    public int x;
    public int z;
    public bool canWalk = true;

    [System.Flags]
    public enum Direction
    {
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
    }
    public Direction m_Direction;

    public int g;
    public int h;
    public int f;

    public SPathNode lastNode;

    // Constructor for PathNode class. Initializes the node with the given grid, x, and z coordinates.
    public SPathNode(GridMap<SPathNode> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;

        // 计算网格的实际宽度和高度
        int gridWidth = grid.maxwidth - grid.minwidth + 1;
        int gridHeight = grid.maxheight - grid.minheight + 1;

        // 使用网格的范围来动态计算方向
        int direction = 15; // 初始假设所有方向都可行

        // 列（X轴）边界
        if (x == grid.minwidth) // 最左列
            direction -= 8; // 移除向左的方向
        else if (x == grid.maxwidth) // 最右列
            direction -= 2; // 移除向右的方向

        // 行（Z轴）边界
        if (z == grid.minheight) // 最顶行
            direction -= 1; // 移除向上的方向
        else if (z == grid.maxheight) // 最底行
            direction -= 4; // 移除向下的方向

        m_Direction = (Direction)direction;
    }

    // Checks if the node has a specific direction.
    public bool CheckDirection(Direction direction)
    {
        return m_Direction.HasFlag(direction);
    }

    // Calculates the total cost of moving to and from the node.
    public void Getf()
    {
        f = g + h;
    }
}








