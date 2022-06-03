using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    protected Collider2D _collider;

    private Transform _canvas;
    private GameObject _healthbar;

    public void Awake()
    {
        
    }

    protected virtual bool IsActive()
    {
        return true;
    }
    
    //TODO Methods for unit selections (and others)
}
