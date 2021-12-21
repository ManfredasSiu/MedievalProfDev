using Assets.Grid.Scripts.Map.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private readonly int k_MoveStraightCost = 10;
    private readonly int k_MoveDiagonaltCost = 14;

    private GenericGrid<PathNode> m_NodeGrid;
    private List<PathNode> m_OpenList;
    private List<PathNode> m_ClosedList;

    public GenericGrid<PathNode> NodeGrid => m_NodeGrid;

    public Pathfinding(int width, int height)
    {
        m_NodeGrid = new GenericGrid<PathNode>(width, height, 10f, Vector3.zero, (GenericGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        m_NodeGrid.GetXY(startWorldPosition, out int startX, out int startY);
        m_NodeGrid.GetXY(endWorldPosition, out int endX, out int endY);

        var path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * m_NodeGrid.CellSize + Vector3.one * m_NodeGrid.CellSize * .5f);
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var startNode = m_NodeGrid.GetGridObject(startX, startY);
        var endNode = m_NodeGrid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        m_OpenList = new List<PathNode>() { startNode };
        m_ClosedList = new List<PathNode>();

        for(int x = 0; x < m_NodeGrid.Width;x++)
        {
            for(int y = 0; y < m_NodeGrid.Height;y++)
            {
                var pathNode = m_NodeGrid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculatedFCost();
                pathNode.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculatedFCost();

        while(m_OpenList.Count > 0)
        {
            var currentNode = GetLowestFCostNode(m_OpenList);
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            m_OpenList.Remove(currentNode);
            m_ClosedList.Add(currentNode);

            foreach(var neighbor in GetNeighbourList(currentNode))
            {
                if (m_ClosedList.Contains(neighbor))
                {
                    continue;
                }

                if (!neighbor.isWalkable)
                {
                    m_ClosedList.Add(neighbor);
                    continue;
                }

                var tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbor);

                if(tentativeGCost < neighbor.gCost)
                {
                    neighbor.previousNode = currentNode;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateDistanceCost(neighbor, endNode);
                    neighbor.CalculatedFCost();

                    if(!m_OpenList.Contains(neighbor))
                    {
                        m_OpenList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        var neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
            if (currentNode.y + 1 < m_NodeGrid.Height)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
        }
        if (currentNode.x + 1 < m_NodeGrid.Width)
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
            if (currentNode.y + 1 < m_NodeGrid.Height)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
        }
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        if (currentNode.y + 1 < m_NodeGrid.Height)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;
    }

    public PathNode GetNode(int x, int y)
    {
        return m_NodeGrid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        var currentNode = endNode;

        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return k_MoveDiagonaltCost* Mathf.Min(xDistance, yDistance) + k_MoveStraightCost * remaining; 
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        var lowestfCostNode = pathNodeList[0];

        for(int i = 1; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestfCostNode.fCost)
            {
                lowestfCostNode = pathNodeList[i];
            }
        }

        return lowestfCostNode;
    }
}
