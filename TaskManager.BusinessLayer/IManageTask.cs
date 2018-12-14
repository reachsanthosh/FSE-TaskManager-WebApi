using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.BusinessLayer
{
    public interface IManageTask
    {
        Task<int> AddTaskAsync(TaskDetail taskDetail);
        Task<IEnumerable<TaskDetail>> ViewTasksAsync();
        Task<TaskDetail> GetTaskAsync(int id);
        Task<int> EditTaskAsync(int id, TaskDetail taskDetail);
        bool IsTaskValidToClose(TaskDetail taskDetail);
    }
}
