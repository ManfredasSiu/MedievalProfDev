using UnityEngine;

namespace DefaultNamespace
{
    public enum TaskType
    {
        Walking,
        Mining,
        Hauling
    }
    
    public class ArtificialTask
    {
        bool m_IsDone;

        TaskType m_TaskType;

        GameObject m_TaskTarget;
        
        public TaskType taskType => m_TaskType;

        public bool isDone => m_IsDone;

        public GameObject taskTarget => m_TaskTarget;

        public ArtificialTask(TaskType taskType, GameObject taskTarget)
        {
            m_TaskType = taskType;
            m_IsDone = false;
            m_TaskTarget = taskTarget;
        }

        public void SetToDone()
        {
            m_IsDone = false;
        }

        public void SetTaskTarget(GameObject gameObject)
        {
            m_TaskTarget = gameObject;
        }
    }
}
