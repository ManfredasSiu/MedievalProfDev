using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    protected float Remainder { get; set; }
    protected float multiplier;

    protected void Start()
    {
        var localScale = transform.localScale;
        Remainder = 1000 * (localScale.x + localScale.y);
    }

    public void RemoveResource(float count)
    {
        //TODO IF REMAINDER LESS THAN COUNT THAN GIVE BOT ONLY REMAINDER INSTEAD OF MAX CARRYABLE CAPACITY
        Remainder -= count;
        if (Remainder <= 0)
        {
            Destroy(gameObject);
            
        }
            
    }

    protected void OnDestroy()
    {
        foreach (var pair in Globals.RESOURCE_NODES)
        {
            pair.Value.Remove(gameObject);
        }
    }
}

