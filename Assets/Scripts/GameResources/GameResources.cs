using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    public static List<GameResource> GAME_RESOURCES = new List<GameResource>()
    {
        new GameResource("Something1", 1000),
        new GameResource("Something2", 1000),
        new GameResource("Something3", 1000),
        new GameResource("Something4", 1000),
        new GameResource("Something5", 1000),
        new GameResource("Something6", 1000),
        
    };
}
