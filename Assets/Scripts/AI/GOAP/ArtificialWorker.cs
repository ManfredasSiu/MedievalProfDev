using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtificialWorker : GAgent
{
    new void Start()
    {
        base.Start();
        var s1 = new SubGoal(StateKeys.GoldMined, 1, false);
        
        var s2 = new SubGoal(StateKeys.ResourcesHauled, 1, false);
        var s3 = new SubGoal(StateKeys.AgentFed, 1, false);
        
        goals.Add(s1, 3);
        goals.Add(s2, 5);
        goals.Add(s3, 5);
        Invoke(nameof(GetHungry), Random.Range(10,30));
    }

    void GetHungry()
    {
        beliefs.ModifyState(StateKeys.AgentHungry, 0);
        Invoke(nameof(GetHungry), Random.Range(10,30));
    }
}
