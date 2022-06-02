using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building", order = 1)]
public class BuildingData : ScriptableObject
{
    public string code;
    public string unitName;
    public int hp;
    public GameObject prefab;
    public List<GameResource> cost;

    public BuildingData(string code, int hp, List<GameResource> cost)
    {
        this.code = code;
        this.hp = hp;
        this.cost = cost;
    }

    public bool CanBuy()
    {
        return cost.All(resource => GameResources.GAME_RESOURCES[resource.Name].Amount >= resource.Amount);
    }
}
