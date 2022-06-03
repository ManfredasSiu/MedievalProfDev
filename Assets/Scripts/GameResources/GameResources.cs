using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameResourceEnum
{
    Wood,
    Food,
    Gold
}

public class GameResources : MonoBehaviour
{
    public static Dictionary<GameResourceEnum, GameResource> GAME_RESOURCES = new Dictionary<GameResourceEnum, GameResource>()
    {
        {GameResourceEnum.Wood,new GameResource(GameResourceEnum.Wood, 1000)},
        {GameResourceEnum.Food,new GameResource(GameResourceEnum.Food, 1000)},
        {GameResourceEnum.Gold,new GameResource(GameResourceEnum.Gold, 1000)},
    };
}
