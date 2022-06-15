using System;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtificialWorker : GAgent
{
    [SerializeField]
    BuildingEnum workPlace;
    
    [SerializeField]
    BaseResources resourceType = BaseResources.Gold;
    
    [SerializeField]
    ResourceNodeEnums resourceNodeType = ResourceNodeEnums.Gold;

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

    public BaseResources ResourceType
    {
        get => resourceType;
        set => resourceType = value;
    }
    
    public ResourceNodeEnums ResourceNodeType
    {
        get => resourceNodeType;
        set => resourceNodeType = value;
    }

    new void Start()
    {
        base.Start();
        var s1 = new SubGoal(StateKeys.GoldMined, 1, false);
        var s4 = new SubGoal(StateKeys.HasTool, 1, false);

        var s2 = new SubGoal(StateKeys.ResourcesHauled, 1, false);
        var s3 = new SubGoal(StateKeys.AgentFed, 1, false);
        
        goals.Add(s4, 5);
        goals.Add(s1, 2);
        goals.Add(s2, 3);
        goals.Add(s3, 4);
        
        Invoke(nameof(GetHungry), Random.Range(5,10));
        Invoke(nameof(ToolBroken), Random.Range(5,10));
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

    //ToDo: below functions are bound to be removed later
    void GetHungry()
    {
        if (!beliefs.GetStates().ContainsKey(StateKeys.AgentHungry))
        {
            beliefs.SetState(StateKeys.AgentHungry, 0);
        }
        Invoke(nameof(GetHungry), Random.Range(5,10));
    }
    
    void ToolBroken()
    {
        beliefs.SetState(StateKeys.NeedsTool, 0);

        Invoke(nameof(ToolBroken), Random.Range(5,10));
    }
}
