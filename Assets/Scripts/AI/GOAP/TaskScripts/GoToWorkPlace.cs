using System;
using UnityEngine;

public class GoToWorkPlace : GAction
{
    public override bool PrePerform()
    {
        target = ((ArtificialWorker) gAgentComponent).workPlaceGameObject;
        
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState(StateKeys.NeedsTool);
        
        return true;
    }
}
