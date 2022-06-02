using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ComplexTask : ArtificialTask
    {
        IEnumerable<ArtificialTask> m_GranularTasks;

        public IEnumerable<ArtificialTask> granularTasks => m_GranularTasks;

        public bool isLoop;
        
        public ComplexTask(TaskType taskType, GameObject taskTarget, bool loop = false)
            : base(taskType, taskTarget)
        {
            isLoop = loop;
        }
        
        public ComplexTask(ComplexTask complexTask)
            : base(complexTask.taskType, complexTask.taskTarget)
        {
            m_GranularTasks = complexTask.granularTasks;
            isLoop = complexTask.isLoop;
        }

        public void AssignGranularTasks(IEnumerable<ArtificialTask> tasks)
        {
            m_GranularTasks = tasks;
        }
    }
}
