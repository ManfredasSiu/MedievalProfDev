using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO PLACEMENT CHECKING
public class BuildingManager : UnitManager
{
    // TODO PLACEMENT CHECKERS AND EVERYTHING ELSE

    private Building _building;

    public new void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Initialize(Building building)
    {
        _building = building;
    }
    
    
}
