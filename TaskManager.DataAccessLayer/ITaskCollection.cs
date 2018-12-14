using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DataAccessLayer
{
    public interface ITaskCollection
    {
        Task<IEnumerable<TaskDetails>> GetAllTasksAsync();
        Task<TaskDetails> GetTaskAsync(int id);
        Task<int> InsertTaskAsync(TaskDetails entity);
        Task<int> UpdateTaskAsync(int id, TaskDetails entity);
        Task<int> DeleteTaskAsync(TaskDetails entity);
    }
}
