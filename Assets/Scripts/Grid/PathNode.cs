using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridMap<PathNode> grid; // �洢��GridMap�����ã��Ա�PathNode�ܹ����ʺͲ�������
    public int x; // �ڵ��������е�x����
    public int z; // �ڵ��������е�z����
    public bool canWalk = true; // ��ʾ�ڵ��Ƿ�������ߣ�true��ʾ���ԣ�false��ʾ������

    // ʹ��System.Flags���Զ���Directionö�٣�����һ�������洢���ֵ
    [System.Flags]
    public enum Direction
    {
        Up = 1 << 0,    // �����ƶ��������Ʊ�ʾ0001
        Right = 1 << 1, // �����ƶ��������Ʊ�ʾ0010
        Down = 1 << 2,  // �����ƶ��������Ʊ�ʾ0100
        Left = 1 << 3,  // �����ƶ��������Ʊ�ʾ1000
    }
    public Direction m_Direction; // �洢��ǰ�ڵ�����ƶ��ķ���

    public int g; // ����㵽��ǰ�ڵ���ƶ�����
    public int h; // �ӵ�ǰ�ڵ㵽�յ�Ĺ����ƶ�����
    public int f; // �ڵ�����ƶ����ۣ�f = g + h��������A*�㷨��ѡ��·��

    public PathNode lastNode; // �洢���ﵱǰ�ڵ��ǰһ���ڵ㣬����׷��·��

    // PathNode�Ĺ��캯������ʼ���ڵ�
    public PathNode(GridMap<PathNode> grid, int x, int z)
    {
        this.grid = grid; // ������������
        this.x = x;       // ����x����
        this.z = z;       // ����z����

        // ��ʼ��ʱ����ڵ������ⷽ���ƶ������Ǻ����߼�����������ƣ�
        m_Direction = Direction.Up | Direction.Right | Direction.Down | Direction.Left;
    }

    // ���ڵ��Ƿ������ָ�������ƶ�
    public bool CheckDirection(Direction direction)
    {
        return m_Direction.HasFlag(direction); // ʹ��HasFlag�������m_Direction�Ƿ����ָ���ķ���
    }

    // ����ڵ���ܴ���f
    public void Getf()
    {
        f = g + h; // �ܴ����Ǵ���㵽��ǰ�ڵ�Ĵ���g��ӵ�ǰ�ڵ㵽�յ�Ĺ������h֮��
    }
}


