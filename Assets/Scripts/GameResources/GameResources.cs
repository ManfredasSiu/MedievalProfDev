using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    public static Dictionary<string, GameResource> GAME_RESOURCES = new Dictionary<string, GameResource>()
    {
        {"Wood",new GameResource("Wood", 1000)},
        {"Food",new GameResource("Food", 1000)},
        {"Mood",new GameResource("Mood", 1000)},
    };
}
