using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    //TODO EVERYTHING 

    private Building _placedBuilding;
    private Vector3 _lastPlacementPosition; // might need to do vector2 not sure yet
    private Camera _mainCam;
    private UIManager _uiManager;
    private Pathfinding _pathfinding;
    private Vector3 _cameraToWorld;
    
    private const int gridLayer = 1 << 6;

    public static event EventHandler<BuildingPlacedEvent> RaiseBuildingPlacedEvent;

    private void Awake()
    {
        _mainCam = Camera.main;
        _uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        _pathfinding = PathfindingManager.pathfinding;
    }

    private void Update()
    {
        if (_placedBuilding != null)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _CancelPlacedBuilding();
                return;
            }
            
            _cameraToWorld = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            _cameraToWorld.z = 0;
            var node = _pathfinding.GetNodeCenterPosition(_cameraToWorld);
            _placedBuilding.SetPosition(node);
            if (_lastPlacementPosition != node)
            {
                _placedBuilding.CheckValidPlacement(node);
            }

            if (_placedBuilding.HasValidPlacement && Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _PlaceBuilding(node);
            }

        }
    }
    

    public void SelectPlacedBuilding(int buildingDataIndex)
    {
        _PreparePlacedBuilding(buildingDataIndex);   
    }

    private void _PreparePlacedBuilding(int buildingDataIndex)
    {
        if (_placedBuilding is {IsFixed: false})
        {
            Destroy(_placedBuilding.Transform.gameObject);
        }

        var building = new Building(Globals.BUILDING_DATA[buildingDataIndex]);
        building.BuildingManager.Initialize(building);
        _placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;
    }

    private void _CancelPlacedBuilding()
    {
        Destroy(_placedBuilding.Transform.gameObject);
        _placedBuilding = null;
    }

    private void _PlaceBuilding(Vector3 placedPos)
    {
        _placedBuilding.Place();
        _PreparePlacedBuilding(_placedBuilding.DataIndex);
        OnRaiseBuildingPlacedEvent(new BuildingPlacedEvent(_placedBuilding.Data.cost, _cameraToWorld));
        if(!_placedBuilding.CanBuy())
            _CancelPlacedBuilding();
        
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

    public Vector3 Pos => pos;
}
