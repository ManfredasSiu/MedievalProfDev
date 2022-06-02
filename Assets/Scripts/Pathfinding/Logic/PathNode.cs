using Assets.Grid.Scripts.Map.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GenericGrid<PathNode> m_Grid;
    
    public int value;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public float moveDiagonalCost;
    public float moveStraightCost;

    public float percentileSlowness => PathfindingConstants.k_MoveDiagonaltBaseCost/ moveDiagonalCost;

    public bool isWalkable;
    public PathNode previousNode;

    public PathNode(GenericGrid<PathNode> grid, int x, int y)
    {
        this.m_Grid = grid;
        this.x = x;
        this.y = y;

        moveDiagonalCost = PathfindingConstants.k_MoveDiagonaltBaseCost;
        moveStraightCost = PathfindingConstants.k_MoveStraightBaseCost;
        
        
        isWalkable = true;
    }

    public void ApplySlowness(float slowness = 0)
    {
        moveDiagonalCost += moveDiagonalCost * slowness/100;
        moveStraightCost += moveStraightCost * slowness/100;
    }

    public void CalculatedFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return gCost + "," + hCost + "," + fCost;
    }
}
