using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding.Scripts.UIManagers
{
    public class PathfindingManager : MonoBehaviour
    {
        private static Pathfinding m_Pathfinding;

        private static GridLayout m_UnifiedGrid; 

        [SerializeField] 
        private Vector3 m_OriginPosition = Vector3.zero;
        
        [SerializeField] 
        private int m_Width;

        [SerializeField] 
        private int m_Height;

        [SerializeField] 
        private float m_CellSize;

        [SerializeField] 
        private Tilemap m_PathTilemap;
        
        [SerializeField] 
        private Tilemap m_ColliderTilemap;

        [SerializeField] 
        private TileScriptableObject m_TileStructuresContainer;

        public static Pathfinding pathfinding => m_Pathfinding;

        void Awake()
        {
            if (m_PathTilemap.transform.parent != m_ColliderTilemap.transform.parent)
            {
                Debug.LogError("Tilemaps have different parents, unified grid cannot be found");
                return;
            }
            else
            {
                m_UnifiedGrid = m_PathTilemap.transform.parent.GetComponentInParent<GridLayout>();
            }
            
            if (m_PathTilemap != null || m_ColliderTilemap != null)
            {
                Tilemap.tilemapTileChanged += UpdateWall;
            }

            m_Pathfinding = CreatePathfinding();
        }

        private void UpdateWall(Tilemap tilemap, Tilemap.SyncTile[] changedTiles)
        {
            var grid = m_Pathfinding.NodeGrid;
            var wallTileLists = m_TileStructuresContainer.tileDataStructures.Where(tileData =>
                tileData.TileType == TileType.Obstacle || tileData.TileType == TileType.Water).Select(tileDataStructure => tileDataStructure.Tiles);

            var tileLists = wallTileLists.ToList();
            
            if (!tileLists.Any())
            {
                return;
            }

            var wallTiles = new List<TileBase>();
            
            foreach (var tileList in tileLists)
            {
                wallTiles.AddRange(tileList);
            }
            
            foreach (var tile in changedTiles)
            {
                var worldPos = m_UnifiedGrid.CellToWorld(tile.position);
                
                var gridTile = grid.GetGridObject(worldPos);
                if (wallTiles.Contains(tile.tile))
                {
                    gridTile.isWalkable = false;
                }
                else
                {
                    gridTile.isWalkable = true;
                }
            }
        }

        private Pathfinding CreatePathfinding()
        {
            var currentPathFinding = new Pathfinding(m_Width, m_Height, m_CellSize, m_OriginPosition);
            var grid = currentPathFinding.NodeGrid;
            
            var tileStructures = m_TileStructuresContainer.tileDataStructures;
            
            var wallTileLists = tileStructures.Where(tileData => tileData.TileType == TileType.Obstacle || tileData.TileType == TileType.Water)
                .Select(tileDataStructure => tileDataStructure.Tiles)
                .SelectMany(x => x);

            var slownessStructure = tileStructures.First(tileData => tileData.TileType == TileType.Ground_Slow);
            
            var slownessTiles = slownessStructure.Tiles;

            for (var x = m_OriginPosition.x; x < m_Width+m_OriginPosition.x; x++)
            {
                for (var y = m_OriginPosition.y; y < m_Height+m_OriginPosition.y; y++)
                {
                    var worldPos = new Vector3(x, y, m_OriginPosition.z);
                    var cellPosition = m_UnifiedGrid.WorldToCell(worldPos);
                    
                    var pathTile = m_PathTilemap.GetTile(Vector3Int.FloorToInt(cellPosition));
                    var colliderTile = m_ColliderTilemap.GetTile(Vector3Int.FloorToInt(cellPosition));
                    
                    if (wallTileLists.Contains(colliderTile))
                    {
                        grid.GetGridObject(worldPos).isWalkable = false;
                    }

                    if (slownessTiles.Contains(pathTile))
                    {
                        grid.GetGridObject(worldPos).ApplySlowness(slownessStructure.SlownessIndicator);
                    }
                }
            }
            
            return currentPathFinding;
        }
    }
}
