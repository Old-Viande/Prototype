using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;//const 是常量
    private const int MOVE_DIAGONAL_COST = 14;
    public GridMap<PathNode> grid;
    private List<PathNode> openNodes;
    private List<PathNode> closedNodes;

    public PathFinder(int maxwidth, int maxheight, int minwidth, int minheight, float celllong = 4f, Vector3 worldPosition = default)//此处是构造函数
    {
        //解释一下这段代码，这段代码是在构造一个网格，这个网格是用来存放PathNode的，这个网格的大小是由传入的参数决定的
        //这个网格是一个二维数组，每一个元素都是一个PathNode
        //这个网格是用来存放寻路的
        //这段代码里面的委托是用来初始化每一个PathNode的
       
        grid = new GridMap<PathNode>(maxwidth, maxheight, minwidth, minheight, celllong, worldPosition, delegate (GridMap<PathNode> grid, int x, int z) { return new PathNode(grid, x, z); }); //委托是一个函数的简写
    }

    public GridMap<PathNode> GetGrid()
    {
        return grid;
    }
    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ, bool canInteract = false) //传入开始点的网格坐标以及目标点的网格坐标
    {
        PathNode startNode = grid.GetValue(startX, startZ);
        PathNode endNode = grid.GetValue(endX, endZ);
        openNodes = new List<PathNode> { startNode };
        closedNodes = new List<PathNode>();

       

        foreach (var a in GetGrid().gridmap)
        {
            PathNode pathNode = grid.GetValue(a.Value.x, a.Value.z);
            pathNode.g = int.MaxValue;
            pathNode.Getf();
            pathNode.lastNode = null;
        }

        startNode.g = 0;
        startNode.h = GetDistanceCost(startNode, endNode);
        startNode.Getf();

        if (canInteract)
            endNode.canWalk = true;

        while (openNodes.Count > 0)//循环网格中的可行走格子
        {
            PathNode currentNode = GetCurrentNode(openNodes);
            if (currentNode == endNode)
            {
                var result = GetPath(endNode);
                if (canInteract)
                {
                    endNode.canWalk = false;
                    result.Remove(endNode);
                }
                return result;
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            foreach (PathNode aroundNode in AroundNodes(currentNode))//检测周围的节点
            {
                if (closedNodes.Contains(aroundNode)) continue;
                if (!aroundNode.canWalk) continue;
                int tryGetGCost = currentNode.g + GetDistanceCost(currentNode, aroundNode);
                if (tryGetGCost < aroundNode.g)
                {
                    aroundNode.lastNode = currentNode;
                    aroundNode.g = tryGetGCost;
                    aroundNode.h = GetDistanceCost(aroundNode, endNode);
                    aroundNode.Getf();

                    if (!openNodes.Contains(aroundNode))
                    {
                        openNodes.Add(aroundNode);
                    }
                }
            }
        }
        if (canInteract)
            endNode.canWalk = false;
        return new List<PathNode>();
    }

    private void WalkCheck(int x, int z, ref List<PathNode> aroundList)
    {
        PathNode node = GetNode(x, z);
        if (node != null)
        {
            if (node.canWalk)
            {
                aroundList.Add(node);
            }
        }

    }

    private List<PathNode> AroundNodes(PathNode currentnode)// 新版本寻路节点                                                          
    {
        List<PathNode> aroundList = new List<PathNode>();
        // Left
        if (currentnode.CheckDirection(PathNode.Direction.Left))
            WalkCheck(currentnode.x - 1, currentnode.z, ref aroundList);        
        // Right
        if (currentnode.CheckDirection(PathNode.Direction.Right))
            WalkCheck(currentnode.x + 1, currentnode.z, ref aroundList);
        // Up
        if (currentnode.CheckDirection(PathNode.Direction.Up))
            WalkCheck(currentnode.x, currentnode.z + 1, ref aroundList);
        // Down
        if (currentnode.CheckDirection(PathNode.Direction.Down))
            WalkCheck(currentnode.x, currentnode.z - 1, ref aroundList);
        return aroundList;
    }

    public List<PathNode> CheckAroundNodes(PathNode currentnode)//循环当前节点周围的节点,找出该节点周围的可以行走的点位
                                                                //ctrl+h 替换
    {
        List<PathNode> aroundList = new List<PathNode>();
        WalkCheck(currentnode.x - 1, currentnode.z, ref aroundList);
        WalkCheck(currentnode.x - 1, currentnode.z - 1, ref aroundList);
        WalkCheck(currentnode.x - 1, currentnode.z + 1, ref aroundList);
        // Right
        WalkCheck(currentnode.x + 1, currentnode.z, ref aroundList);
        // Right Down
        WalkCheck(currentnode.x + 1, currentnode.z - 1, ref aroundList);
        // Right Up
        WalkCheck(currentnode.x + 1, currentnode.z + 1, ref aroundList);

        // Up
        WalkCheck(currentnode.x, currentnode.z + 1, ref aroundList);
        // Down
        WalkCheck(currentnode.x, currentnode.z - 1, ref aroundList);
        return aroundList;
    }

    public PathNode GetNode(int x, int z)
    {
        return grid.GetValue(x, z);
    }
    private List<PathNode> GetPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.lastNode != null)
        {
            path.Add(currentNode.lastNode);
            currentNode = currentNode.lastNode;
        }
        path.Reverse();//这一段是在反转List的顺序
        return path;
    }

    public int GetDistanceCost(PathNode a, PathNode b)//这一段是在算距离花费
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public PathNode GetCurrentNode(List<PathNode> pathnodelist)//找出当前节点，也就是F最小的那个点
    {
        PathNode lowestF = pathnodelist[0];
        for (int i = 0; i < pathnodelist.Count; i++)
        {
            if (pathnodelist[i].f < lowestF.f)
            {
                lowestF = pathnodelist[i];
            }
        }
        return lowestF;
    }

}
