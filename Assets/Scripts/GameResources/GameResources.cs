using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameResourceEnum
{
    Wood,
    Food,
    Gold
}

public class GameResources : MonoBehaviour
{
    public static event Action OnResourcesModified;
    
    public static Dictionary<GameResourceEnum, GameResource> GAME_RESOURCES = new Dictionary<GameResourceEnum, GameResource>()
    {
        {GameResourceEnum.Wood,new GameResource(GameResourceEnum.Wood, 1000)},
        {GameResourceEnum.Food,new GameResource(GameResourceEnum.Food, 1000)},
        {GameResourceEnum.Gold,new GameResource(GameResourceEnum.Gold, 1000)},
    };

    public static void ModifyResource(Dictionary<GameResourceEnum, int> resourceDict)
    {
        foreach (var resource in resourceDict)
        {
            ModifyResource(resource.Key, resource.Value);
        }
        OnResourcesModified?.Invoke();
    }
    
    public static void ModifyResources(List<GameResource> resourceList)
    {
        foreach (var resource in resourceList)
        {
            ModifyResource(resource.code, resource.amount);
        }
        OnResourcesModified?.Invoke();
    }

    public static void ModifyResource(GameResourceEnum resourceType, int amount)
    {
        GAME_RESOURCES[resourceType].AddOrRemove(amount);
    }
}
