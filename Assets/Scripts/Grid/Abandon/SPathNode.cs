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

        // ���������ʵ�ʿ�Ⱥ͸߶�
        int gridWidth = grid.maxwidth - grid.minwidth + 1;
        int gridHeight = grid.maxheight - grid.minheight + 1;

        // ʹ������ķ�Χ����̬���㷽��
        int direction = 15; // ��ʼ�������з��򶼿���

        // �У�X�ᣩ�߽�
        if (x == grid.minwidth) // ������
            direction -= 8; // �Ƴ�����ķ���
        else if (x == grid.maxwidth) // ������
            direction -= 2; // �Ƴ����ҵķ���

        // �У�Z�ᣩ�߽�
        if (z == grid.minheight) // ���
            direction -= 1; // �Ƴ����ϵķ���
        else if (z == grid.maxheight) // �����
            direction -= 4; // �Ƴ����µķ���

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








