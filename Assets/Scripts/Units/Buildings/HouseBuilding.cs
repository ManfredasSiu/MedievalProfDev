using System.Collections;
using System.Collections.Generic;
using PathFinding.Scripts.UIManagers;
using Unity.Mathematics;
using UnityEngine;

public class HouseBuilding : Building
{
    public List<WorkerUnit> WorkerUnits { get; set; }

    public HouseBuilding(BuildingData data) : base(data)
    {
        WorkerUnits = Data.code switch
        {
            BuildingEnum.House => new List<WorkerUnit>(2),
            BuildingEnum.BigHouse => new List<WorkerUnit>(4),
            _ => WorkerUnits
        };
    }

    public void SpawnWorkers()
    {
        for (var i = 0; i < WorkerUnits.Capacity; i++)
        {
            WorkerUnits.Add(new WorkerUnit(Resources.Load("ScriptableObjects/Units/Workers/WorkerMiner") as WorkerData , BuildingObject));
            ((WorkerManager) WorkerUnits[i]._unitManager).Initialize(WorkerUnits[i]);
        }
    }
    
    public override void Place()
    {
        base.Place();
        SpawnWorkers();
    }
}
