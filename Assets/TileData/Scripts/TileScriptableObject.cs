using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Data Container", menuName = "TileData/Tile Data Container", order = 1)]
public class TileScriptableObject : ScriptableObject
{
    [SerializeField]
    public List<TileDataStructure> tileDataStructures = new List<TileDataStructure>();
}
