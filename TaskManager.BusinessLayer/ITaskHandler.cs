using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.BusinessLayer
{
    public interface ITaskHandler
    {
        Task<int> AddTaskAsync(TaskDetails taskDetail);
        Task<IEnumerable<TaskDetails>> ViewTasksAsync();
        Task<TaskDetails> GetTaskAsync(int id);
        Task<int> EditTaskAsync(int id, TaskDetails taskDetail);
        bool IsTaskValidToClose(TaskDetails taskDetail);
        Task<int> DeleteTaskAsync(TaskDetails taskDetails);
    }
}
