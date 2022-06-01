using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileDataStructure
{
   public TileType TileType;

   public List<TileBase> Tiles;

   public float SlownessIndicator;
}
