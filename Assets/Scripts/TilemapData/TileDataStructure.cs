using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileDataStructure
{
   public TileType TileType;

   public List<TileBase> Tiles;

   public float SlownessIndicator;
}
