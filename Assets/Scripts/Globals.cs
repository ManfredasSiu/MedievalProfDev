using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Globals
    {
        public static BuildingData[] BUILDING_DATA;
        public static Dictionary<BuildingEnum, List<GameObject>> BUILT_BUILDINGS = 
            new Dictionary<BuildingEnum, List<GameObject>>();
        public static Dictionary<ResourceNodeEnums, List<GameObject>> RESOURCE_NODES =
            new Dictionary<ResourceNodeEnums, List<GameObject>>();
    }
}