using Assets.Grid.Scripts.Map.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GenericGrid<PathNode> grid;
    public int value;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode previousNode;

    public PathNode(GenericGrid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;

        isWalkable = true;
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
