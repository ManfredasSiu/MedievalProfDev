using System;
using UnityEngine;

public class MineGold : GAction
{
    [SerializeField]
    BaseResources resourceType = BaseResources.Gold;
    
    public int resourceAmountToAdd = 10;

    public override bool PrePerform()
    {
        target = ((ArtificialWorker) gAgentComponent).workPlaceGameObject;
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        Inventory.IncrementResource(resourceType, resourceAmountToAdd);
        target.GetComponent<ResourceNode>().RemoveResource(resourceAmountToAdd);

        return true;
    }
}
