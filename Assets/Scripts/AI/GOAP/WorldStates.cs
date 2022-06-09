using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StateKeys
{
    ArrivedToGoldMine,
    GoldMined,
    WoodChopped,
    AgentHungry,
    AgentFed,
    ResourcesHauled,
    InventoryFull,
}

[Serializable]
public class WorldState
{
    public StateKeys key;
    public int value;
}

public class WorldStates
{
    Dictionary<StateKeys, int> m_States;

    public WorldStates()
    {
        m_States = new Dictionary<StateKeys, int>();
    }

    public bool HasState(StateKeys key)
    {
        return m_States.ContainsKey(key);
    }

    void AddState(StateKeys key, int value)
    {
        m_States.Add(key, value);
    }

    public void ModifyState(StateKeys key, int value)
    {
        if (m_States.ContainsKey(key))
        {
            m_States[key] += value;
            if (m_States[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else
        {
            m_States.Add(key, value);
        }
    }

    public void RemoveState(StateKeys key)
    {
        if (m_States.ContainsKey(key))
        {
            m_States.Remove(key);
        }
    }

    public void SetState(StateKeys key, int value)
    {
        if (m_States.ContainsKey(key))
        {
            m_States[key] = value;
        }
        else
        {
            m_States.Add(key, value);
        }
    }

    public Dictionary<StateKeys, int> GetStates()
    {
        return m_States;
    }
}
