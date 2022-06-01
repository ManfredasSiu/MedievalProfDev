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

    public void AddTaskToTheQueue(TaskType type, GameObject gameObject)
    {
        var task = new ArtificialTask(type, gameObject);
        
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
        var walkingTask = new ArtificialTask(TaskType.Walking, m_TempMiningTarget);
        var miningTask = new ArtificialTask(TaskType.Mining, null);
        var walkingTask2 = new ArtificialTask(TaskType.Walking, m_TempHaulingTarget);
        var haulingTask = new ArtificialTask(TaskType.Hauling, null);
        
        AssignNewTask(walkingTask);
        
        m_ArtificialTaskQueue.Enqueue(miningTask);
        m_ArtificialTaskQueue.Enqueue(walkingTask2);
        m_ArtificialTaskQueue.Enqueue(haulingTask);
    }

    void OnTaskTargetReached(GameObject taskTarget)
    {
        m_CurrentTask.SetToDone();
        m_DoneTasks.Add(m_CurrentTask);
        
        if (!m_ArtificialTaskQueue.Any())
        {
            return;
        }
        var nextTask = m_ArtificialTaskQueue.Dequeue();
        
        AssignNewTask(nextTask);
    }

    void AssignNewTask(ArtificialTask task)
    {
        m_CurrentTask = task;
        switch (task.taskType)
        {
            case TaskType.Walking:
                m_ArtificialMovement.SetTarget(task.taskTarget);
                break;
            case TaskType.Hauling:
                m_ArtificialMovement.StopForSeconds();
                break;
            case TaskType.Mining:
                m_ArtificialMovement.StopForSeconds();
                break;
        }
    }
    
    void SubscribeToMovementEvents()
    {
        m_ArtificialMovement.onTargetReached -= OnTaskTargetReached;
        m_ArtificialMovement.onTargetReached += OnTaskTargetReached;
    }
}
