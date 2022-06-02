using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    public static Dictionary<string, GameResource> GAME_RESOURCES = new Dictionary<string, GameResource>()
    {
        {"resource1",new GameResource("Wood", 1000)},
        {"resource2",new GameResource("Food", 1000)},
        {"resource3",new GameResource("Mood", 1000)},
};
}
