using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ResourceNodeGold : ResourceNode
{
    protected new void Start()
    {
        base.Start();
        multiplier = 1f;
        Remainder *= multiplier;
        Utils.AddToDict(ResourceNodeEnums.Gold, gameObject, Globals.RESOURCE_NODES);
        print(Remainder);
    }
}
