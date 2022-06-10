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

    public GameObject movementTarget => m_Target;

    public float distanceToTarget => Vector2.Distance(transform.position, m_NextMilestone + yDelta);
    
    public event Action onTargetReached;

    public void SetTarget(GameObject target)
    {
        m_Target = target;
    }

    public void StopForSeconds(float seconds = 5f)
    {
        m_Target = null;
        StartCoroutine(WaitForSec(seconds));
    }

    public GameObject FindBestTarget(params GameObject[] gameObjects)
    {
        if (gameObjects.Length == 0)
            return null;
        GameObject bestGameObject = gameObjects.First();
        float bestFCost = float.MaxValue;
        foreach (var target in gameObjects)
        {
            var path = m_Pathfinding.FindPath(positionWithDelta, target.transform.position, out var pathFCost);
            if (path == null)
            {
                continue;
            }
            
            if (pathFCost < bestFCost)
            {
                bestGameObject = target;
                bestFCost = pathFCost;
            }
        }

        return bestGameObject;
    }

    IEnumerator WaitForSec( float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onTargetReached?.Invoke();
    }

    private void Awake()
    {
        m_Pathfinding = PathfindingManager.pathfinding;
    }

    void Start()
    {
        PathfindingManager.OnPathfindingChanged += OnPathfindingEdited;
    }

    void Update()
    {
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

    void OnPathfindingEdited()
    {
        if (m_Target != null)
        {
            var targetPos = m_Target.TransformPositionWithOffset();
            FindPathToTheTargetAndSetMilestone(targetPos);
        }
    }

    void FindWalkingPathAndSetMilestone(Vector3 targetPosition)
    {
        m_Path = m_Pathfinding.FindPath(positionWithDelta, targetPosition, out var pathFCost);
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
        
        if (m_Path == null || !m_Path.Any())
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
