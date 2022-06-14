using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ArtificialMovement : MonoBehaviour
{
    [SerializeField] 
    float m_Speed = 5f;

    [SerializeField]
    GameObject m_Target;
    
    // Pathfinding m_Pathfinding;

    PathNode m_TargetPathNode;
    
    List<Vector3> m_Path;

    Vector3 m_NextMilestone;

    Vector3 positionWithDelta => gameObject.TransformPositionWithOffset();

    Vector3 yDelta => gameObject.GetYDeltaForTransform();

    public GameObject movementTarget => m_Target;

    public float distanceToTarget => Vector2.Distance(transform.position, m_NextMilestone + yDelta);
    
    public event Action onTargetReached;

    public void SetTarget(GameObject target)
    {
        m_Target = target;
    }
    
    void Start()
    {
        PathfindingManager.OnPathfindingChanged += OnPathfindingEdited;
    }

    void Update()
    {
        if (m_Target != null && m_TargetPathNode != PathfindingManager.pathfinding.GetNode(m_Target.transform.position - new Vector3(0, 0.5f)))
        {
            var targetPos = m_Target.transform.position - new Vector3(0, 0.5f);

            FindPathToTheTargetAndSetMilestone(targetPos);
        }

        if (m_Path != null)
        {
            ControlledMovement();
        }
    }

    void OnPathfindingEdited()
    {
        if (m_Target != null)
        {
            var targetPos = m_Target.transform.position - new Vector3(0, 1f);
            FindPathToTheTargetAndSetMilestone(targetPos);
        }
    }

    void FindWalkingPathAndSetMilestone(Vector3 targetPosition)
    {
        m_Path = PathfindingManager.pathfinding.FindPath(positionWithDelta, targetPosition, out var pathFCost);
        if (m_Path == null)
        {
            return;
        }
        m_NextMilestone = m_Path.First();
        m_Path.Remove(m_NextMilestone);
    }

    void FindPathToTheTargetAndSetMilestone(Vector3 targetPosition)
    {
        m_TargetPathNode = PathfindingManager.pathfinding.GetNode(targetPosition);

        FindWalkingPathAndSetMilestone(targetPosition);
        
        if (m_Path == null || !m_Path.Any())
        {
            m_Path = null;
            return;
        }
        
        m_Path.RemoveAt(m_Path.Count-1);
    }

    void ControlledMovement()
    {
        var slowness = PathfindingManager.pathfinding.GetSlownessAtCurrentPos(positionWithDelta);

        var movementSpeed = Time.deltaTime * m_Speed * slowness;

        transform.position = Vector2.MoveTowards(transform.position,
            m_NextMilestone+ yDelta, movementSpeed);

        if (distanceToTarget < 0.1f)
        {
            if (!m_Path.Any())
            {
                m_Path = null;
                onTargetReached?.Invoke();

                //m_Target = null;
                return;
            }
            
            m_NextMilestone = m_Path.First();
            m_Path.Remove(m_NextMilestone);
        }
    }
}
