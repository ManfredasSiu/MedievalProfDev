using System.Collections.Generic;
using System.Linq;
using PathFinding.Scripts.UIManagers;
using UnityEngine;

public class ArtificialMovement : MonoBehaviour
{
    [SerializeField] 
    float m_Speed = 5f;

    [SerializeField]
    public GameObject target;
    
    Pathfinding m_Pathfinding;

    PathNode m_TargetPathNode = null;

    Rigidbody2D m_Rigidbody;
    
    List<Vector3> m_Path;

    Vector3 m_NextMilestone;

    Vector3 positionWithDelta => gameObject.TransformPositionWithOffset();

    Vector3 yDelta => gameObject.GetYDeltaForTransform();
    

    void Start()
    {
        m_Pathfinding = PathfindingManager.pathfinding;
    }

    void Update()
    {
        if (target == null && Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                var cameraToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                FindWalkingPathAndSetMilestone(cameraToWorld);
            }
        }

        if (target != null && m_TargetPathNode != m_Pathfinding.GetNode(target.TransformPositionWithOffset()))
        {
            var targetPos = target.TransformPositionWithOffset();

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
                return;
            }
            
            m_NextMilestone = m_Path.First();
            m_Path.Remove(m_NextMilestone);
        }
    }
}
