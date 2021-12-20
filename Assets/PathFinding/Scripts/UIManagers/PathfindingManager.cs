using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private Pathfinding m_Pathfinding;
    
    private void Start()
    {
        m_Pathfinding = new Pathfinding(10, 10);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mousePos = UtilsClass.GetMouseWorldPosition();

            m_Pathfinding.NodeGrid.GetXY(mousePos, out int x, out int y);

            var path = m_Pathfinding.FindPath(0, 0, x, y);

            if(path != null)
            {
                for(int i = 0; i < path.Count -1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 100);
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousepos = UtilsClass.GetMouseWorldPosition();
            m_Pathfinding.NodeGrid.GetXY(mousepos, out int x, out int y);
            m_Pathfinding.GetNode(x,y).isWalkable = !m_Pathfinding.GetNode(x,y).isWalkable;
        }
    }
}
