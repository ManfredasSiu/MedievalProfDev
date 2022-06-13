using Assets.Grid.Scripts.Map.Logic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    GenericGrid<PathNode> m_NodeGrid;
    List<PathNode> m_OpenList;
    List<PathNode> m_ClosedList;

    public GenericGrid<PathNode> NodeGrid => m_NodeGrid;

    public Pathfinding(int width, int height, float cellSize = 10f, Vector3 origin = default)
    {
        m_NodeGrid = new GenericGrid<PathNode>(width, height, cellSize, origin, (g, x, y) => new PathNode(g, x, y));
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition, out float fCost)
    {
        fCost = float.MaxValue;
        m_NodeGrid.GetXY(startWorldPosition, out var startX, out var startY);
        m_NodeGrid.GetXY(endWorldPosition, out var endX, out var endY);

        var path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            var vectorPath = new List<Vector3>();
            foreach (var pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * m_NodeGrid.CellSize + Vector3.one * m_NodeGrid.CellSize * .5f + m_NodeGrid.Origin);
            }

            fCost = path.Last().fCost;
            return vectorPath;
        }
    }

    public Vector3 GetCenterEdge(Vector3 lowerLeftPos, Vector3 upperRightPos)
    {
        var normalizedUpper = upperRightPos - lowerLeftPos;
        return normalizedUpper / 2;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var startNode = m_NodeGrid.GetGridObject(startX, startY);
        var endNode = m_NodeGrid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null || !endNode.isWalkable)
        {
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

            var neighbourList = GetNeighbourList(currentNode);
            
            foreach(var neighbor in neighbourList)
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
                
                if (neighbor.isWalkable && neighbor.x != currentNode.x && neighbor.y != currentNode.y)
                {
                    if (neighbourList.Any(node => !node.isWalkable 
                                                  && (node.x == neighbor.x + 1 
                                                  || node.x == neighbor.x - 1 
                                                  || node.y == neighbor.y + 1 
                                                  || node.y == neighbor.y - 1)))
                    {
                        continue;
                    }
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
    
    public PathNode GetNode(Vector3 worldPos)
    {
        return m_NodeGrid.GetGridObject(worldPos);
    }
    
    public float GetSlownessAtCurrentPos(Vector3 worldPos)
    {
        return m_NodeGrid.GetGridObject(worldPos).percentileSlowness;
    }
    
    public Vector3 GetNodeCenterPosition(Vector3 worldPos)
    {
        var node = GetNode(worldPos);
        return node == null ? worldPos : m_NodeGrid.GetWorldPosition(node.x, node.y);
    }
    
    public Vector3 GetNodeCenterPosition(PathNode node)
    {
        return m_NodeGrid.GetWorldPosition(node.x, node.y);
    }

    public void SetWalkable(Vector3 lowerLeftPos, Vector3 upperRightPos, bool isWalkable)
    {
        var affectedNodes = GetNodes(lowerLeftPos, upperRightPos);

        affectedNodes.ForEach(node => node.isWalkable = false);
    }

    public List<PathNode> GetNodes(Vector3 lowerLeftPos, Vector3 upperRightPos)
    {
        var currentPos = lowerLeftPos;

        var nodeList = new List<PathNode>();
        while (currentPos.y < upperRightPos.y)
        {
            while (currentPos.x < upperRightPos.x)
            {
                var node = GetNode(currentPos);
                if (node == null)
                    return nodeList;
                nodeList.Add(node);
                currentPos.x += m_NodeGrid.CellSize;
            }

            currentPos.x = lowerLeftPos.x;
            currentPos.y += m_NodeGrid.CellSize;
        }

        return nodeList;
    }

    public List<PathNode> GetBoundingNodes(Vector3 lowerLeftPos, Vector3 upperRightPos)
    {
        var currentPos = lowerLeftPos;

        var nodeList = new List<PathNode>();
        while (currentPos.y < upperRightPos.y)
        {
            var leftNode = GetNode(new Vector3(lowerLeftPos.x, currentPos.y));
            var rightNode = GetNode(new Vector3(upperRightPos.x, currentPos.y));
            
            nodeList.Add(leftNode);
            nodeList.Add(rightNode);
            
            currentPos.y += m_NodeGrid.CellSize;
        }
        
        while (currentPos.x < upperRightPos.x)
        {
            var upNode = GetNode(new Vector3(currentPos.x, upperRightPos.y));
            var downNode = GetNode(new Vector3(currentPos.x, lowerLeftPos.y));

            if (!nodeList.Contains(upNode))
            {
                nodeList.Add(upNode);
            }
            
            if (!nodeList.Contains(downNode))
            {
                nodeList.Add(downNode);
            }

            currentPos.x += m_NodeGrid.CellSize;
        }

        return nodeList;
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

        int moveDiagonalCost = (int)(a.moveDiagonalCost + b.moveDiagonalCost);
        int moveStraightCost = (int)(a.moveStraightCost + b.moveStraightCost);

        return moveDiagonalCost * Mathf.Min(xDistance, yDistance) + moveStraightCost * remaining; 
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
