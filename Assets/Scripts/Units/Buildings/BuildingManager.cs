using System;
using System.Collections;
using System.Collections.Generic;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

//TODO PLACEMENT CHECKING
public class BuildingManager : UnitManager
{
    // TODO PLACEMENT CHECKERS AND EVERYTHING ELSE

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

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCam != null)
            {
                var cameraToWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
                var centerPos = _pathfinding.GetNodeCenterPosition(cameraToWorld);
                var g = Instantiate(Resources.Load("Prefabs/Buildings/House")) as GameObject;
                g.transform.position = centerPos;
            }
        }

    }

    public void Initialize(Building building)
    {
        _building = building;
    }
    
    
}
