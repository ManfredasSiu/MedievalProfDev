using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class ArtificialMovement : MonoBehaviour
{
    [SerializeField] 
    float m_Speed = 5f;

    [SerializeField]
    GameObject m_Target;
    
    Pathfinding m_Pathfinding;

    PathNode m_TargetPathNode;
    
    List<Vector3> m_Path;

    Vector3 m_NextMilestone;

    Vector3 positionWithDelta => gameObject.TransformPositionWithOffset();

    Vector3 yDelta => gameObject.GetYDeltaForTransform();

    public event Action<GameObject> onTargetReached;

    public void SetTarget(GameObject target)
    {
        m_Target = target;
    }

    public void StopForSeconds(float seconds = 5f)
    {
        m_Target = null;
        StartCoroutine(WaitForSec(seconds));
    }

    IEnumerator WaitForSec( float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onTargetReached?.Invoke(null);
    }

    void Start()
    {
        m_Pathfinding = PathfindingManager.pathfinding;
    }

    void Update()
    {
        if (m_Target == null && Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                var cameraToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                FindWalkingPathAndSetMilestone(cameraToWorld);
            }
        }

        if (m_Target != null && m_TargetPathNode != m_Pathfinding.GetNode(m_Target.TransformPositionWithOffset()))
        {
            var targetPos = m_Target.TransformPositionWithOffset();

            FindPathToTheTargetAndSetMilestone(targetPos);
        }

        if (m_Path != null)
        {
            ControlledMovement();
        }
    }

    void FindWalkingPathAndSetMilestone(Vector3 targetPosition)
    {
        m_Path = m_Pathfinding.FindPath(positionWithDelta, targetPosition);
        if (m_Path == null)
        {
            return;
        }
        m_NextMilestone = m_Path.First();
        m_Path.Remove(m_NextMilestone);
    }

    void FindPathToTheTargetAndSetMilestone(Vector3 targetPosition)
    {
        m_TargetPathNode = m_Pathfinding.GetNode(targetPosition);

        FindWalkingPathAndSetMilestone(targetPosition);
        
        if (!m_Path.Any())
        {
            m_Path = null;
            return;
        }
        
        m_Path.RemoveAt(m_Path.Count-1);
    }
    
    void PrecalculatePathForTheNextTarget(Vector3 targetPosition)
    {
        m_TargetPathNode = m_Pathfinding.GetNode(targetPosition);

        m_Path = m_Pathfinding.FindPath(m_Path.Last(), targetPosition);
        if (m_Path == null)
        {
            return;
        }
        if (!m_Path.Any())
        {
            m_Path = null;
            return;
        }
        
        m_Path.RemoveAt(m_Path.Count-1);
    }

    void ControlledMovement()
    {
        var slowness = m_Pathfinding.GetSlownessAtCurrentPos(positionWithDelta);

        var movementSpeed = Time.deltaTime * m_Speed * slowness;

        transform.position = Vector2.MoveTowards(transform.position,
            m_NextMilestone+ yDelta, movementSpeed);

        if (Vector2.Distance(transform.position, m_NextMilestone+ yDelta) < 0.1f)
        {
            if (!m_Path.Any())
            {
                m_Path = null;
                onTargetReached?.Invoke(m_Target);

                m_Target = null;
                return;
            }
            
            m_NextMilestone = m_Path.First();
            m_Path.Remove(m_NextMilestone);
        }
    }
}
