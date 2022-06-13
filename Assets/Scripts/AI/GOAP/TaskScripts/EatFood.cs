using System;
using UnityEngine;

public class EatFood : GAction
{
    [SerializeField]
    GameResourceEnum resourceType = GameResourceEnum.Food;
    
    public int foodEaten = 20;

    new void Awake()
    {
        base.Awake();

        target = GetComponent<GAgent>().houseReference;
    }

    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GameResources.ModifyResource(resourceType, -foodEaten);
        
        beliefs.RemoveState(StateKeys.AgentHungry);
        return true;
    }
}
