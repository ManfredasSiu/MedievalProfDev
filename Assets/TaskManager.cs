using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_TempMiningTarget;

    [SerializeField]
    GameObject m_TempHaulingTarget;
    
    ArtificialTask m_CurrentTask;
    
    ArtificialMovement m_ArtificialMovement;
    
    Queue<ArtificialTask> m_ArtificialTaskQueue;

    List<ArtificialTask> m_DoneTasks = new List<ArtificialTask>();

    public void AddTaskToTheQueue(TaskType type, GameObject targetGameObject)
    {
        var walkingTask = new ArtificialTask(TaskType.Walking, targetGameObject);
        var task = new ArtificialTask(type, null);
        
        m_ArtificialTaskQueue.Enqueue(walkingTask);
        m_ArtificialTaskQueue.Enqueue(task);
    }
    
    void Start()
    {
        m_ArtificialMovement = GetComponent<ArtificialMovement>();
        
        m_ArtificialTaskQueue = new Queue<ArtificialTask>();

        SubscribeToMovementEvents();
        
        TaskSetup();
    }

    void TaskSetup()
    {
        AddTaskToTheQueue(TaskType.Mining, m_TempMiningTarget);
        AddTaskToTheQueue(TaskType.Hauling, m_TempHaulingTarget);
        AddTaskToTheQueue(TaskType.Mining, m_TempMiningTarget);
        AddTaskToTheQueue(TaskType.Hauling, m_TempHaulingTarget);
        AddTaskToTheQueue(TaskType.Mining, m_TempMiningTarget);
        AddTaskToTheQueue(TaskType.Hauling, m_TempHaulingTarget);
        AddTaskToTheQueue(TaskType.Mining, m_TempMiningTarget);
        AddTaskToTheQueue(TaskType.Hauling, m_TempHaulingTarget);
        
        AssignNewTask();
    }

    void OnTaskTargetReached()
    {
        m_CurrentTask.SetToDone();
        m_DoneTasks.Add(m_CurrentTask);
        
        if (!m_ArtificialTaskQueue.Any())
        {
            return;
        }
        AssignNewTask();
    }

    void AssignNewTask()
    {
        var task = m_ArtificialTaskQueue.Dequeue();

        m_CurrentTask = task;
        switch (task.taskType)
        {
            case TaskType.Walking:
                m_ArtificialMovement.SetTarget(task.taskTarget);
                break;
            case TaskType.Hauling:
                m_ArtificialMovement.StopForSeconds(1);
                break;
            case TaskType.Mining:
                m_ArtificialMovement.StopForSeconds(1);
                break;
        }
    }
    
    void SubscribeToMovementEvents()
    {
        m_ArtificialMovement.onTargetReached -= OnTaskTargetReached;
        m_ArtificialMovement.onTargetReached += OnTaskTargetReached;
    }
}
