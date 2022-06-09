using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkerInventory : MonoBehaviour
{
    Dictionary<GameResourceEnum, int> m_Resources = new Dictionary<GameResourceEnum, int>();

    int m_CurrentResourceAmount => m_Resources.Sum(resource => resource.Value);
    
    int m_MaxAmount = 100;
    
    public Dictionary<GameResourceEnum, int> resources => m_Resources;

    public bool isEmpty => m_CurrentResourceAmount <= 0;

    public bool isFull => m_CurrentResourceAmount >= m_MaxAmount;
    
    void Start()
    {
        ReformInventory();
    }

    public int IncrementResource(GameResourceEnum resourceType, int increment)
    {
        if (isFull)
        {
            return increment;
        }

        var lastDelta = increment;
        var leftAmount = 0;
        
        if (m_CurrentResourceAmount + increment > m_MaxAmount)
        {
            lastDelta = m_MaxAmount - m_CurrentResourceAmount;
            leftAmount = increment - lastDelta;
        }

        m_Resources[resourceType] += lastDelta;

        return leftAmount;
    }

    public int DecrementResource(GameResourceEnum resourceType, int decrement)
    {
        if (isEmpty)
        {
            return decrement;
        }

        var lastDelta = decrement;
        var leftAmount = 0;
        
        if (m_Resources[resourceType] - lastDelta < 0)
        {
            lastDelta = m_CurrentResourceAmount;
            leftAmount = decrement - lastDelta;
        }

        m_Resources[resourceType] -= lastDelta;

        return leftAmount;
    }
    
    public Dictionary<GameResourceEnum, int>  RemoveAllResources()
    {
        var returnResources = new Dictionary<GameResourceEnum, int>(m_Resources);
        ReformInventory();

        return returnResources;
    }

    void ReformInventory()
    {
        foreach (var resourceType in (GameResourceEnum[])Enum.GetValues(typeof(GameResourceEnum)))
        {
            m_Resources[resourceType] = 0;
        }
    }
}
