using System;
using UnityEngine;

public class GoToStockPile : GAction
{
    public override bool PrePerform()
    {
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
