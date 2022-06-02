using System;

[Flags]
public enum TileType
{
    Obstacle = 1,
    Ground_Normal = 2,
    Ground_Slow = 4,
    Water = 8,
}
