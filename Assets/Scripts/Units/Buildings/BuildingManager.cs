using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class BuildingManager : UnitManager
{
    private Building _building;
    private Camera mainCam;

    private Pathfinding _pathfinding;

    public new void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider2D>();
        mainCam = Camera.main;
    }

    public void Start()
    {
        _pathfinding = PathfindingManager.pathfinding;
    }

    public void Initialize(Building building)
    {
        _building = building;
    }

    public bool CheckPlacement(Vector3 mousePos)
    {
        if (_building == null) return false;
        if (_building.IsFixed) return false;
        var validPlacement = _HasValidPlacement(mousePos);
        _building.SetMaterials(!validPlacement ? BuildingPlacement.INVALID : BuildingPlacement.VALID);
        return validPlacement;
    }

    protected override bool IsActive()
    {
        return _building.IsFixed;
    }
    

    private bool _HasValidPlacement(Vector3 mousePos)
    {
        var nodes = PathfindingManager.pathfinding.GetNodes(mousePos, mousePos + transform.localScale);
        return nodes.All(node => node.isWalkable);
    }
    
    
}
