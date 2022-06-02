using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    public static List<GameResource> GAME_RESOURCES = new List<GameResource>()
    {
        new GameResource("Wood", 1000),
        new GameResource("Food", 1000),
        new GameResource("Gold", 1000),
    };
}
