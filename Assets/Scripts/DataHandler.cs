using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DataHandler 
{
    public static void LoadGameData()
    {
        LoadBuildings();
    }

    private static void LoadBuildings()
    {
        Globals.BUILDING_DATA = Resources.LoadAll<BuildingData>("ScriptableObjects/Units/Buildings") as BuildingData[];
    }
}
