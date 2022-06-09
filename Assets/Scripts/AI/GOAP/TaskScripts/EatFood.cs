using System;
using UnityEngine;

public class EatFood : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState(StateKeys.AgentHungry);
        return true;
    }
}
