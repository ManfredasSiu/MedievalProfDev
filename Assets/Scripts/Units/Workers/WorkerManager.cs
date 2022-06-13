using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : UnitManager
{
    public WorkerUnit Worker { get; set; }

    public void Initialize(WorkerUnit workerUnit)
    {
        Worker = workerUnit;
    }

    public void Start()
    {
        
    }
    //for future worker thingies

}
