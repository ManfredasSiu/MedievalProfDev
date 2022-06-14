using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ResourceNodeStone : ResourceNode
{
    protected new void Start()
    {
        base.Start();
        multiplier = 2f;
        Remainder *= multiplier;
        Utils.AddToDict(ResourceNodeEnums.Stone, gameObject, Globals.RESOURCE_NODES);
    }
}
