using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private Pathfinding m_Pathfinding;

    List<Vector3> path;
    Vector3 nextMilestone;

    private void Start()
    {
        m_Pathfinding = new Pathfinding(10, 10);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ResetPath();
            var mousePos = UtilsClass.GetMouseWorldPosition();

            FindPath(transform.position, mousePos);
        }

        PathControlls();

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousepos = UtilsClass.GetMouseWorldPosition();
            m_Pathfinding.NodeGrid.GetXY(mousepos, out int x, out int y);
            m_Pathfinding.GetNode(x,y).isWalkable = !m_Pathfinding.GetNode(x,y).isWalkable;
        }
    }

    public void FindPath(Vector3 currentTransformPos, Vector3 targetPos)
    {
        path = m_Pathfinding.FindPath(currentTransformPos, targetPos);
    }

    private void PathControlls()
    {
        if (path != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextMilestone, Time.deltaTime * 10);
            nextMilestone = path[0];
            if (Vector3.Distance(transform.position, nextMilestone) <= 0.5f)
            {
                SetNewMilestone();
            }

            if (path.Count <= 0)
            {
                ResetPath();
            }

            else if (Vector3.Distance(transform.position, nextMilestone) > m_Pathfinding.NodeGrid.CellSize * 1.5f)
            {
                path = m_Pathfinding.FindPath(transform.position, path[path.Count - 1]);
                SetNewMilestone();
            }
        }
    }

    private void ResetPath()
    {
        path = null;
        nextMilestone = default(Vector3);
    }

    private void SetNewMilestone()
    {
        path.Remove(path[0]);
        if(path.Count > 0)
        {
            nextMilestone = path[0];
        }
    }
}
