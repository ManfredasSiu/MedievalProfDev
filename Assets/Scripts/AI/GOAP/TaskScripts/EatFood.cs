using System;
using UnityEngine;

public class EatFood : GAction
{
    [SerializeField]
    BaseResources resourceType = BaseResources.Food;
    
    public int foodEaten = 20;

    public override bool PrePerform()
    {
        target = gAgentComponent.houseReference;
        
        return target != null;
    }

    public override bool PostPerform()
    {
        GameResources.ModifyResource(resourceType, -foodEaten);
        
        beliefs.RemoveState(StateKeys.AgentHungry);
        return true;
    }
}
