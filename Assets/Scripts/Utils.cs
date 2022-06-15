using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public static class Utils
{
    public static Building SetBuildingType(int buildingIndex)
    {
        var buildingType = Globals.BUILDING_DATA[buildingIndex].code;
        Building newBuilding = buildingType switch
        {
            BuildingEnum.House => new HouseBuilding(Globals.BUILDING_DATA[buildingIndex]),
            BuildingEnum.Quarry => new QuarryBuilding(Globals.BUILDING_DATA[buildingIndex]),
            BuildingEnum.Stockpile => new StockpileBuilding(Globals.BUILDING_DATA[buildingIndex]),
            _=> null
        };

        return newBuilding;
    }

    public static void AddToBuildingsDict(BuildingEnum key, GameObject building)
    {
        var dict = Globals.BUILT_BUILDINGS;
        if (!dict.ContainsKey(key))
        {
            var list = new List<GameObject> {building};
            dict.Add(key, list);
        }
        else
        {
            var existingBuildings = dict[key];
            existingBuildings.Add(building);
            dict[key] = existingBuildings;
        }
    }
    
    public static void AddToDict<T>(T key, GameObject obj, Dictionary<T,List<GameObject>> dict)
    {
        if (!dict.ContainsKey(key))
        {
            var list = new List<GameObject> {obj};
            dict.Add(key, list);
        }
        else
        {
            var existingBuildings = dict[key];
            existingBuildings.Add(obj);
            dict[key] = existingBuildings;
        }
    }
    
    public static void CleanDict<T>(Dictionary<T,List<GameObject>> dict)
    {
        foreach (var pair in dict)
        {
            pair.Value.RemoveAll(item => item == null);
        }
        
    }
}
