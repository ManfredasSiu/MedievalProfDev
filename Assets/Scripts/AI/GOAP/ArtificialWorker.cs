using System;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtificialWorker : GAgent
{
    [SerializeField]
    BuildingEnum workPlace;

    public GameObject workPlaceGameObject; 
    
    public BuildingEnum WorkPlaceType
    {
        get => workPlace;
        set
        {
            workPlaceGameObject = null;
            workPlace = value;
        }
    }

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

    void Update()
    {
        if (workPlaceGameObject == null)
        {
            if (!Globals.BUILT_BUILDINGS.ContainsKey(workPlace))
            {
                return;
            }
            
            var allWorkPlaces = Globals.BUILT_BUILDINGS[workPlace].ToArray();

            workPlaceGameObject = PathfindingManager.FindBestTarget(gameObject.TransformPositionWithOffset(), allWorkPlaces);
        }
    }

    void GetHungry()
    {
        beliefs.ModifyState(StateKeys.AgentHungry, 0);
        Invoke(nameof(GetHungry), Random.Range(10,30));
    }
}
