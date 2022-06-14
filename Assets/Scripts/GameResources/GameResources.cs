using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GameResources
{
    public static event Action OnResourcesModified;
    
    public static Dictionary<BaseResources, GameResource> GAME_RESOURCES = new Dictionary<BaseResources, GameResource>()
    {
        {BaseResources.Wood,new GameResource(BaseResources.Wood, 1000)},
        {BaseResources.Food,new GameResource(BaseResources.Food, 1000)},
        {BaseResources.Gold,new GameResource(BaseResources.Gold, 1000)},
        {BaseResources.Stone,new GameResource(BaseResources.Stone, 1000)},
    };

    public static void ModifyResource(Dictionary<BaseResources, int> resourceDict, bool invertMinus = false)
    {
        foreach (var resource in resourceDict)
        {
            if (invertMinus)
            {
                ModifyResource(resource.Key, -resource.Value);
            }
            else
            {
                ModifyResource(resource.Key, resource.Value);
            }
            
        }
        OnResourcesModified?.Invoke();
    }
    
    public static void ModifyResources(List<GameResource> resourceList, bool invertMinus = false)
    {
        foreach (var resource in resourceList)
        {
            if (invertMinus)
            {
                ModifyResource(resource.code, -resource.amount);
            }
            else
            {
                ModifyResource(resource.code, resource.amount);
            }
        }
        OnResourcesModified?.Invoke();
    }

    public static void ModifyResource(BaseResources resourceType, int amount)
    {
        GAME_RESOURCES[resourceType].AddOrRemove(amount);
    }
}
