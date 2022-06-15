using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class WorkerUnit : Unit
{
    public WorkerData Data { get; }
    public ArtificialWorker WorkerAI { get; }
    public WorkerInventory Inventory { get; }

    public WorkerUnit(WorkerData data, GameObject houseReference, int spawnOffset = 0)
    {
        var spawnCoords = PathfindingManager.FindBoundingTileCoordinates(houseReference);
        _unitObject = GameObject.Instantiate(data.prefab, spawnCoords[0+spawnOffset], Quaternion.identity);
        _transform = _unitObject.transform;
        Data = data;
        _currentHealth = data.hp;
        _unitManager = _transform.GetComponent<WorkerManager>();
        ((WorkerManager)_unitManager).Initialize(this);
        WorkerAI = _transform.GetComponent<ArtificialWorker>();
        WorkerAI.ResourceType = Random.Range(0, 10) < 5 ? BaseResources.Stone : BaseResources.Gold;
        WorkerAI.ResourceNodeType = Random.Range(0, 10) < 5 ? ResourceNodeEnums.Stone : ResourceNodeEnums.Gold;
        Inventory = _transform.GetComponent<WorkerInventory>();
        WorkerAI.houseReference = houseReference;
    }
}
