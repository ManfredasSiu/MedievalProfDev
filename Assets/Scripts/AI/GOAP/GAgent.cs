using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal
{
    public Dictionary<StateKeys, int> sGoals;

    public bool remove;

    public SubGoal(StateKeys key, int value, bool remove)
    {
        sGoals = new Dictionary<StateKeys, int>();
        sGoals.Add(key, value);

        this.remove = remove;
    }
}

public class GAgent : MonoBehaviour
{
    bool m_Invoked;
    
    SubGoal m_CurrentGoal;
    
    GPlanner m_Planner;

    ArtificialMovement m_MovementControls;
    
    Queue<GAction> m_ActionQueue;
    
    public List<GAction> actions = new List<GAction>();
    
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    public GAction currentAction;

    public WorldStates beliefs = new WorldStates();

    public WorkerInventory inventory;
    
    public GameObject houseReference;

    public void Start()
    {
        m_MovementControls = GetComponent<ArtificialMovement>();
        var acts = GetComponents<GAction>();
        actions.AddRange(acts);
    }

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        m_Invoked = false;
    }
    
    void LateUpdate()
    {
        if (inventory.isFull && !beliefs.HasState(StateKeys.InventoryFull))
        {
            beliefs.SetState(StateKeys.InventoryFull, 0);
        }
        
        if (currentAction != null && currentAction.running)
        {
            if (m_MovementControls.movementTarget && m_MovementControls.distanceToTarget < 0.1f)
            {
                if (!m_Invoked)
                {
                    Invoke(nameof(CompleteAction), currentAction.duration);
                    m_Invoked = true;
                }
            }

            return;
        }
        
        if (m_Planner == null || m_ActionQueue == null)
        {
            m_Planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (var subGoal in sortedGoals)
            {
                m_ActionQueue = m_Planner.Plan(actions, subGoal.Key.sGoals, beliefs);
                if (m_ActionQueue != null)
                {
                    m_CurrentGoal = subGoal.Key;
                    break;
                }
            }
        }

        if (m_ActionQueue != null && !m_ActionQueue.Any())
        {
            if (m_CurrentGoal.remove)
            {
                goals.Remove(m_CurrentGoal);
            }

            m_Planner = null;
        }

        if (m_ActionQueue != null && m_ActionQueue.Any())
        {
            currentAction = m_ActionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    m_MovementControls.SetTarget(currentAction.target);
                }
            }
            else
            {
                m_ActionQueue = null;
            }
        }
    }
}
