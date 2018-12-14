using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DataAccessLayer
{
    public interface ITaskRepository
    {
         Task<IEnumerable<TaskDetail>> GetAllAsync();
        Task<TaskDetail> GetAsync(int id);
        Task<int> InsertAsync(TaskDetail entity);
        Task<int> UpdateAsync(int id, TaskDetail entity);
        Task<int> DeleteAsync(TaskDetail entity);
    }
}
