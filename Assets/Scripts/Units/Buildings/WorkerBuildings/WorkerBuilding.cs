using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBuilding : Building
{
    public int WorkerCapacity { get; }
    public int ResourceCapacity { get; }
    public List<WorkerUnit> Workers { get; set; } = new List<WorkerUnit>();
    //Dict because we might have a building that can gather multiple resources, also for stockpile
    public Dictionary<GameResourceEnum, int> ResourceCount { get; set; } = new Dictionary<GameResourceEnum, int>();

    public WorkerBuilding(BuildingData data) : base(data)
    {
    }
}
