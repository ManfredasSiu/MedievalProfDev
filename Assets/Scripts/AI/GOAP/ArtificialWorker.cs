using System;
using UnityEngine;

public class ArtificialWorker : GAgent
{
    new void Start()
    {
        base.Start();
        var s1 = new SubGoal(StateKeys.GoldGathered, 1, false);
        
        goals.Add(s1, 3);
    }
}
