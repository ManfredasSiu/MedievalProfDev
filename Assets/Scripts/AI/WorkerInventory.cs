using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkerInventory : MonoBehaviour
{
    Dictionary<GameResourceEnum, int> m_Resources = new Dictionary<GameResourceEnum, int>();

    int m_CurrentResourceAmount => m_Resources.Sum(resource => resource.Value);
    
    int m_MaxAmount = 100;
    
    public Dictionary<GameResourceEnum, int> resources => m_Resources;

    void Start()
    {
        foreach (var resourceType in (GameResourceEnum[])Enum.GetValues(typeof(GameResourceEnum)))
        {
            m_Resources[resourceType] = 0;
        }
    }

    public int EditResources(GameResourceEnum resourceType, int delta)
    {
        if (isFull())
        {
            return delta;
        }
        else
        {
            if(m_CurrentResourceAmount+delta > m_MaxAmount)
        }
        m_Resources[resourceType] += delta;
    }

    public bool isFull()
    {
        var resourceAmount = m_CurrentResourceAmount;

        return resourceAmount >= m_MaxAmount;
    }
}
