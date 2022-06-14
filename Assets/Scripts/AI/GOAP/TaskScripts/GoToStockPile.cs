using System;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class GoToStockPile : GAction
{
    public override bool PrePerform()
    {
        target = PathfindingManager.FindBestTarget(gameObject.TransformPositionWithOffset(), Globals.BUILT_BUILDINGS[BuildingEnum.Stockpile].ToArray());
        if (target == null)
        {
            return false;
        }
        
        return true;
    }

    public override bool PostPerform()
    {
        var resources = Inventory.RemoveAllResources();
        GameResources.ModifyResource(resources);
        
        beliefs.RemoveState(StateKeys.InventoryFull);
        return true;
    }
}
