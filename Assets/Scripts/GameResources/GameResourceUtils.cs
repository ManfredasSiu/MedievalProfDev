using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResourceUtils
{
    
    public static List<GameResource> TurnResourceCostToNegative(this List<GameResource> resourceList)
    {
        foreach (var resource in resourceList)
        {
            resource.TurnCostToNegative();
        }

        return resourceList;
    }
}
