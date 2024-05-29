using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;//const �ǳ���
    private const int MOVE_DIAGONAL_COST = 14;
    public GridMap<SPathNode> grid;
    private List<SPathNode> openNodes;
    private List<SPathNode> closedNodes;

    public SPathFinder(int maxwidth, int maxheight, int minwidth, int minheight, float celllong, Vector3 worldPosition = default)//�˴��ǹ��캯��
    {
        //����һ����δ��룬��δ������ڹ���һ����������������������SPathNode�ģ��������Ĵ�С���ɴ���Ĳ���������
        //���������һ����ά���飬ÿһ��Ԫ�ض���һ��SPathNode
        //����������������Ѱ·��
        //��δ��������ί����������ʼ��ÿһ��SPathNode��

        grid = new GridMap<SPathNode>(maxwidth, maxheight, minwidth, minheight, celllong, worldPosition, delegate (GridMap<SPathNode> grid, int x, int z) { return new SPathNode(grid, x, z); }); //ί����һ�������ļ�д
    }

    public GridMap<SPathNode> GetGrid()
    {
        return grid;
    }
    public List<SPathNode> FindPath(int startX, int startZ, int endX, int endZ, bool canInteract = false) //���뿪ʼ������������Լ�Ŀ������������
    {
        SPathNode startNode = grid.GetValue(startX, startZ);
        SPathNode endNode = grid.GetValue(endX, endZ);
        openNodes = new List<SPathNode> { startNode };
        closedNodes = new List<SPathNode>();



        foreach (var a in GetGrid().gridmap)
        {
            SPathNode pathNode = grid.GetValue(a.Value.x, a.Value.z);
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
            SPathNode currentNode = GetCurrentNode(openNodes);
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

            foreach (SPathNode aroundNode in AroundNodes(currentNode))//�����Χ�Ľڵ�
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
        return new List<SPathNode>();
    }

    private void WalkCheck(int x, int z, ref List<SPathNode> aroundList)
    {
        SPathNode node = GetNode(x, z);
        if (node != null)
        {
            if (node.canWalk)
            {
                aroundList.Add(node);
            }
        }

    }

    private List<SPathNode> AroundNodes(SPathNode currentnode)// �°汾Ѱ·�ڵ�                                                          
    {
        List<SPathNode> aroundList = new List<SPathNode>();
        // Left
        if (currentnode.CheckDirection(SPathNode.Direction.Left))
            WalkCheck(currentnode.x - 1, currentnode.z, ref aroundList);
        // Right
        if (currentnode.CheckDirection(SPathNode.Direction.Right))
            WalkCheck(currentnode.x + 1, currentnode.z, ref aroundList);
        // Up
        if (currentnode.CheckDirection(SPathNode.Direction.Up))
            WalkCheck(currentnode.x, currentnode.z + 1, ref aroundList);
        // Down
        if (currentnode.CheckDirection(SPathNode.Direction.Down))
            WalkCheck(currentnode.x, currentnode.z - 1, ref aroundList);
        return aroundList;
    }

    public List<SPathNode> CheckAroundNodes(SPathNode currentnode)//ѭ����ǰ�ڵ���Χ�Ľڵ�,�ҳ��ýڵ���Χ�Ŀ������ߵĵ�λ
                                                                //ctrl+h �滻
    {
        List<SPathNode> aroundList = new List<SPathNode>();
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

    public SPathNode GetNode(int x, int z)
    {
        return grid.GetValue(x, z);
    }
    private List<SPathNode> GetPath(SPathNode endNode)
    {
        List<SPathNode> path = new List<SPathNode>();
        path.Add(endNode);
        SPathNode currentNode = endNode;
        while (currentNode.lastNode != null)
        {
            path.Add(currentNode.lastNode);
            currentNode = currentNode.lastNode;
        }
        path.Reverse();//��һ�����ڷ�תList��˳��
        return path;
    }

    /*private int GetDistanceCost(SPathNode a, SPathNode b)//��һ����������뻨��
    {

        int xRoomA = a.x / 7, zRoomA = a.z / 7; // A���ڷ���
        int xRoomB = b.x / 7, zRoomB = b.z / 7; // B���ڷ���
        int xA = a.x % 7, zA = a.z % 7; // A�ڷ����ڵ�����
        int xB = b.x % 7, zB = b.z % 7; // B�ڷ����ڵ�����
        int result;
        if (xRoomA == xRoomB && zRoomA == zRoomB)
        {
            result = Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);
        }
        else if (xRoomA == xRoomB)
        {
            result = Mathf.Abs(a.z - b.z) + Mathf.Abs(xA - 3) + Mathf.Abs(xB - 3);
        }
        else if (zRoomA == zRoomB)
        {
            result = Mathf.Abs(a.x - b.x) + Mathf.Abs(zA - 3) + Mathf.Abs(zB - 3);
        }
        else
        {
            int temp1 = zRoomB > zRoomA ? (6 - zA + Mathf.Abs(3 - xA)) : (zA + Mathf.Abs(3 - xA));
            int temp2 = xRoomB > xRoomA ? (xB + Mathf.Abs(3 - zB)) : (6 - xB + Mathf.Abs(3 - zB));
            int temp3 = xRoomB > xRoomA ? (6 - xA + Mathf.Abs(3 - zA)) : (xA + Mathf.Abs(3 - zA));
            int temp4 = zRoomB > zRoomA ? (zB + Mathf.Abs(3 - xB)) : (6 - zB + Mathf.Abs(3 - xB));
            result = Mathf.Min(temp1 + temp2, temp3 + temp4) + 8 * (Mathf.Abs(xRoomA - xRoomB) + Mathf.Abs(zRoomA - zRoomB) - 1);
        }
        return result;
    }*/
    public int GetDistanceCost(SPathNode a, SPathNode b)//��һ����������뻨��
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public SPathNode GetCurrentNode(List<SPathNode> pathnodelist)//�ҳ���ǰ�ڵ㣬Ҳ����F��С���Ǹ���
    {
        SPathNode lowestF = pathnodelist[0];
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
