using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<StateKeys, int> state;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<StateKeys, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = new Dictionary<StateKeys, int>(allStates);
        this.action = action;
    }
    
    public Node(Node parent, float cost, Dictionary<StateKeys, int> allStates, Dictionary<StateKeys, int> beliefStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = new Dictionary<StateKeys, int>(allStates);
        foreach (var beliefState in beliefStates)
        {
            if (state.ContainsKey(beliefState.Key))
            {
                continue;
            }
            
            state.Add(beliefState.Key, beliefState.Value);
        }
        this.action = action;
    }
}

public class GPlanner 
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<StateKeys, int> goal, WorldStates beliefStates)
    {
        var usableActions = new List<GAction>();
        
        var achievableAction = actions.Where(action => action.IsAchievable());
        usableActions.AddRange(achievableAction);

        var leaves = new List<Node>();
        var start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(),null);

        var success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            return null;
        }

        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
                continue;
            }

            if (leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }

        var result = new List<GAction>();
        var cheapNode = cheapest;
        while (cheapNode != null)
        {
            if (cheapNode.action != null)
            {
                result.Insert(0, cheapNode.action);
            }

            cheapNode = cheapNode.parent;
        }

        var queue = new Queue<GAction>();

        foreach (var actionResult in result)
        {
            queue.Enqueue(actionResult);
        }

        return queue;
    }

    bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<StateKeys, int> goal)
    {
        var foundPath = false;

        foreach (var action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                var currentState = new Dictionary<StateKeys, int>(parent.state);
                foreach (var effects in action.effects)
                {
                    if (!currentState.ContainsKey(effects.Key))
                    {
                        currentState.Add(effects.Key, effects.Value);
                    }
                }

                var node = new Node(parent, parent.cost + action.Cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    var subset = ActionSubset(usableActions, action);
                    var found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    List<GAction> ActionSubset(List<GAction> usableActions, GAction removeMe)
    {
        var subset = new List<GAction>();

        foreach (var action in usableActions)
        {
            if (!action.Equals(removeMe))
            {
                subset.Add(action);
            }
        }

        return subset;
    }

    bool GoalAchieved(Dictionary<StateKeys, int> goals, Dictionary<StateKeys, int> currentStates)
    {
        foreach (var goal in goals)
        {
            if (!currentStates.ContainsKey(goal.Key))
            {
                return false;
            }
        }

        return true;
    }
}
