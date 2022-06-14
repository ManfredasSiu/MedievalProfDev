using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Utils
{
    public static Building SetBuildingType(int buildingIndex)
    {
        var newBuilding = buildingIndex switch
        {
            0 => new HouseBuilding(Globals.BUILDING_DATA[buildingIndex]),
            1 => new QuarryBuilding(Globals.BUILDING_DATA[buildingIndex]),
            2 => new Building(Globals.BUILDING_DATA[buildingIndex]),
            3 => new Building(Globals.BUILDING_DATA[buildingIndex]),
            4 => new StockpileBuilding(Globals.BUILDING_DATA[buildingIndex]),
            _ => null
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
