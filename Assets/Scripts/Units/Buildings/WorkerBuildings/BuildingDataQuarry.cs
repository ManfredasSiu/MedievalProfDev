using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuarryBuilding", menuName = "Scriptable Objects/Resource Building/Quarry Building", order = 1)]
public class BuildingDataQuarry : BuildingData
{
    public ResourceNodeEnums quarryType;
    public BuildingDataQuarry(BuildingEnum code, int hp, List<GameResource> cost) : base(code, hp, cost)
    {
    }
}
