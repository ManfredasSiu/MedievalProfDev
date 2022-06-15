using System;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class MineGold : GAction
{
    [SerializeField]
    BaseResources resourceType = BaseResources.Gold;
    
    [SerializeField]
    ResourceNodeEnums resourceNodeType = ResourceNodeEnums.Gold;
    
    public int resourceAmountToAdd = 10;

    public override bool PrePerform()
    {
        if (Inventory.isFull || beliefs.GetStates().ContainsKey(StateKeys.NeedsTool))
        {
            return false;
        }
        
        var allWorkPlaces = Globals.RESOURCE_NODES[resourceNodeType].ToArray();

        target = PathfindingManager.FindBestTarget(gameObject.TransformPositionWithOffset(), allWorkPlaces);
        
        return target != null;
    }

    public override bool PostPerform()
    {
        Inventory.IncrementResource(resourceType, resourceAmountToAdd);
        target.GetComponent<ResourceNode>().RemoveResource(resourceAmountToAdd);

        return true;
    }
}
