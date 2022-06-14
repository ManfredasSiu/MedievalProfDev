using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUnit : Unit
{
    public WorkerData Data { get; }
    public ArtificialWorker WorkerAI { get; }
    public WorkerInventory Inventory { get; }

    public WorkerUnit(WorkerData data, GameObject houseReference)
    {
        _unitObject = GameObject.Instantiate(data.prefab, houseReference.transform.position-Vector3.one, Quaternion.identity);
        _transform = _unitObject.transform;
        Data = data;
        _currentHealth = data.hp;
        _unitManager = _transform.GetComponent<WorkerManager>();
        ((WorkerManager)_unitManager).Initialize(this);
        WorkerAI = _transform.GetComponent<ArtificialWorker>();
        Inventory = _transform.GetComponent<WorkerInventory>();
        WorkerAI.houseReference = houseReference;
    }
}
