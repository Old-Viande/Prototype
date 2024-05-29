using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridMap<PathNode> grid; // 存储对GridMap的引用，以便PathNode能够访问和操作网格
    public int x; // 节点在网格中的x坐标
    public int z; // 节点在网格中的z坐标
    public bool canWalk = true; // 表示节点是否可以行走，true表示可以，false表示不可以

    // 使用System.Flags属性定义Direction枚举，允许一个变量存储多个值
    [System.Flags]
    public enum Direction
    {
        Up = 1 << 0,    // 向上移动，二进制表示0001
        Right = 1 << 1, // 向右移动，二进制表示0010
        Down = 1 << 2,  // 向下移动，二进制表示0100
        Left = 1 << 3,  // 向左移动，二进制表示1000
    }
    public Direction m_Direction; // 存储当前节点可以移动的方向

    public int g; // 从起点到当前节点的移动代价
    public int h; // 从当前节点到终点的估算移动代价
    public int f; // 节点的总移动代价（f = g + h），用于A*算法中选择路径

    public PathNode lastNode; // 存储到达当前节点的前一个节点，用于追踪路径

    // PathNode的构造函数，初始化节点
    public PathNode(GridMap<PathNode> grid, int x, int z)
    {
        this.grid = grid; // 设置网格引用
        this.x = x;       // 设置x坐标
        this.z = z;       // 设置z坐标

        // 初始化时允许节点向任意方向移动（除非后续逻辑对其进行限制）
        m_Direction = Direction.Up | Direction.Right | Direction.Down | Direction.Left;
    }

    // 检查节点是否可以向指定方向移动
    public bool CheckDirection(Direction direction)
    {
        return m_Direction.HasFlag(direction); // 使用HasFlag方法检查m_Direction是否包含指定的方向
    }

    // 计算节点的总代价f
    public void Getf()
    {
        f = g + h; // 总代价是从起点到当前节点的代价g与从当前节点到终点的估算代价h之和
    }
}


