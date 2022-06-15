using System;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class GoToStockPile : GAction
{
    public override bool PrePerform()
    {
        target = PathfindingManager.FindBestTarget(gameObject.TransformPositionWithOffset(), Globals.BUILT_BUILDINGS[BuildingEnum.Stockpile].ToArray());
        
        return target != null;
    }

    public override bool PostPerform()
    {
        var resources = Inventory.RemoveAllResources();
        GameResources.ModifyResource(resources);
        
        beliefs.RemoveState(StateKeys.InventoryFull);
        return true;
    }
}
