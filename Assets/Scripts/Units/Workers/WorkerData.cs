using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Worker", order = 1)]
public class WorkerData : ScriptableObject
{
    public WorkerEnum code;
    public string unitName;
    public int hp;
    public GameObject prefab;

    public WorkerData(WorkerEnum code, int hp)
    {
        this.code = code;
        this.hp = hp;
    }
}


