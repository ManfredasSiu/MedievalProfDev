using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float Cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public WorldStates beliefs;

    public Dictionary<StateKeys, int> conditions => preConditions?.ToDictionary(state => state.key, state => state.value);
    public Dictionary<StateKeys, int> effects => afterEffects?.ToDictionary(state => state.key, state => state.value);

    public WorldStates agentBeliefs;

    public bool running = false;

    void Awake()
    {
        beliefs = GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<StateKeys, int> givenConditions)
    {
        return conditions.Keys.All(givenConditions.ContainsKey);
    }
    
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
