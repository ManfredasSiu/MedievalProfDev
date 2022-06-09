using System;
using UnityEngine;

public class MineGold : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.SetState(StateKeys.InventoryFull, 1);

        return true;
    }
}
