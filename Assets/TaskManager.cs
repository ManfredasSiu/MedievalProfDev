using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_TempMiningTarget;

    [SerializeField]
    GameObject m_TempHaulingTarget;
    
    ArtificialTask m_CurrentGranularTask;
    
    ComplexTask m_CurrentComplexTask;
    
    ArtificialMovement m_ArtificialMovement;
    
    Queue<ArtificialTask> m_GranularTaskQueue;
    
    Queue<ComplexTask> m_ComplexTaskQueue;

    List<ArtificialTask> m_DoneTasks;

    public void AddTaskToTheQueue(ComplexTask complexTask)
    {
        m_ComplexTaskQueue.Enqueue(complexTask);
    }
    
    void Start()
    {
        m_ArtificialMovement = GetComponent<ArtificialMovement>();
        
        m_GranularTaskQueue = new Queue<ArtificialTask>();
        m_ComplexTaskQueue = new Queue<ComplexTask>();
        m_DoneTasks = new List<ArtificialTask>();

        SubscribeToMovementEvents();
        
        TaskSetup();
    }

    void Update()
    {
        if (m_CurrentComplexTask == null && m_ComplexTaskQueue.Any())
        {
            m_CurrentComplexTask = m_ComplexTaskQueue.Dequeue();
            foreach (var task in m_CurrentComplexTask.granularTasks)
            {
                m_GranularTaskQueue.Enqueue(task);
            }
        }

        if (m_CurrentGranularTask == null && m_GranularTaskQueue.Any())
        {
            ExecuteGranularTask();
        }
    }

    void TaskSetup()
    {
        var miningTask = CreateTaskByType(TaskType.Mining, true);
        var haulingTask = CreateTaskByType(TaskType.Hauling, true);
        
        AddTaskToTheQueue(miningTask);
        AddTaskToTheQueue(haulingTask);
    }

    ComplexTask CreateTaskByType(TaskType type, bool looping)
    {
        switch (type)
        {
            case TaskType.Mining:
                var miningTask = new ComplexTask(TaskType.Mining, m_TempMiningTarget, looping);

                var miningGranularTaskList = new List<ArtificialTask>();
                miningGranularTaskList.Add(new ArtificialTask(TaskType.Walking, m_TempMiningTarget));
                miningGranularTaskList.Add(new ArtificialTask(TaskType.Waiting));
                
                miningTask.AssignGranularTasks(miningGranularTaskList);
               
                return miningTask;
            case TaskType.Hauling:
                var haulingTask = new ComplexTask(TaskType.Mining, m_TempHaulingTarget, looping);

                var haulingGranularTaskList = new List<ArtificialTask>();
                haulingGranularTaskList.Add(new ArtificialTask(TaskType.Walking, m_TempHaulingTarget));
                haulingGranularTaskList.Add(new ArtificialTask(TaskType.Waiting));
                
                haulingTask.AssignGranularTasks(haulingGranularTaskList);
                
                return haulingTask;
        }

        return null;
    }

    void OnTaskTargetReached()
    {
        m_CurrentGranularTask.SetToDone();
        m_DoneTasks.Add(m_CurrentGranularTask);
        
        if (!m_GranularTaskQueue.Any())
        {
            m_CurrentComplexTask.SetToDone();
            if (m_CurrentComplexTask.isLoop)
            {
                AddTaskToTheQueue(new ComplexTask(m_CurrentComplexTask));
            }

            m_CurrentGranularTask = null;
            m_CurrentComplexTask = null;
            return;
        }
        ExecuteGranularTask();
    }

    void ExecuteGranularTask()
    {
        var task = m_GranularTaskQueue.Dequeue();

        m_CurrentGranularTask = task;
        switch (task.taskType)
        {
            case TaskType.Walking:
                m_ArtificialMovement.SetTarget(task.taskTarget);
                break;
            case TaskType.Waiting:
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
