using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;//const �ǳ���
    private const int MOVE_DIAGONAL_COST = 14;
    public GridMap<PathNode> grid;
    private List<PathNode> openNodes;
    private List<PathNode> closedNodes;

    public PathFinder(int maxwidth, int maxheight, int minwidth, int minheight, float celllong = 4f, Vector3 worldPosition = default)//�˴��ǹ��캯��
    {
        //����һ����δ��룬��δ������ڹ���һ����������������������PathNode�ģ��������Ĵ�С���ɴ���Ĳ���������
        //���������һ����ά���飬ÿһ��Ԫ�ض���һ��PathNode
        //����������������Ѱ·��
        //��δ��������ί����������ʼ��ÿһ��PathNode��
       
        grid = new GridMap<PathNode>(maxwidth, maxheight, minwidth, minheight, celllong, worldPosition, delegate (GridMap<PathNode> grid, int x, int z) { return new PathNode(grid, x, z); }); //ί����һ�������ļ�д
    }

    public GridMap<PathNode> GetGrid()
    {
        return grid;
    }
    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ, bool canInteract = false) //���뿪ʼ������������Լ�Ŀ������������
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

        while (openNodes.Count > 0)//ѭ�������еĿ����߸���
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

            foreach (PathNode aroundNode in AroundNodes(currentNode))//�����Χ�Ľڵ�
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

    private List<PathNode> AroundNodes(PathNode currentnode)// �°汾Ѱ·�ڵ�                                                          
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

    public List<PathNode> CheckAroundNodes(PathNode currentnode)//ѭ����ǰ�ڵ���Χ�Ľڵ�,�ҳ��ýڵ���Χ�Ŀ������ߵĵ�λ
                                                                //ctrl+h �滻
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
        path.Reverse();//��һ�����ڷ�תList��˳��
        return path;
    }

    public int GetDistanceCost(PathNode a, PathNode b)//��һ����������뻨��
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public PathNode GetCurrentNode(List<PathNode> pathnodelist)//�ҳ���ǰ�ڵ㣬Ҳ����F��С���Ǹ���
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
