using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    //TODO EVERYTHING 

    private Building _placedBuilding;
    private Vector3 _lastPlacementPosition; // might need to do vector2 not sure yet
    private Camera _mainCam;
    private UIManager _uiManager;

    public event EventHandler<BuildingPlacedEvent> RaiseBuildingPlacedEvent;

    private void Awake()
    {
        _mainCam = Camera.main;
        _uiManager = GetComponent<UIManager>();

    }

    private void Update()
    {
        
    }

    public void SelectPlacedBuilding(int buildingDataIndex)
    {
        
    }

    private void _PreparePlacedBuilding(int buildingDataIndex)
    {
        
    }

    private void _CancelPlacedBuilding()
    {
        
    }

    private void _PlaceBuilding()
    {
        
    }

    protected virtual void OnRaiseBuildingPlacedEvent(BuildingPlacedEvent e)
    {
        var raiseEvent = RaiseBuildingPlacedEvent;
        raiseEvent?.Invoke(this, e);
    }
}

public class BuildingPlacedEvent : EventArgs
{
    private List<GameResource> cost { get; }
    private Vector3 pos { get; }
    public BuildingPlacedEvent(List<GameResource> cost, Vector3 pos)
    {
        this.cost = cost;
        this.pos = pos;
    }
}
