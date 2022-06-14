using System;
using UnityEngine;

public class EatFood : GAction
{
    [SerializeField]
    BaseResources resourceType = BaseResources.Food;
    
    public int foodEaten = 20;

    public override bool PrePerform()
    {
        target = GetComponent<GAgent>().houseReference;
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        GameResources.ModifyResource(resourceType, -foodEaten);
        
        beliefs.RemoveState(StateKeys.AgentHungry);
        return true;
    }
}
