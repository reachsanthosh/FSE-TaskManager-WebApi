using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DataAccessLayer
{
    public class TaskCollection : ITaskCollection
    {
        private readonly TaskDbContext taskDbContext;
        private readonly ILogger<TaskCollection> logger;
        public TaskCollection(TaskDbContext taskDbContext, ILogger<TaskCollection> logger)
        {
            this.taskDbContext = taskDbContext;
            this.logger = logger;
        }
        public async Task<int> DeleteTaskAsync(TaskDetails entity)
        {
            taskDbContext.Tasks.Remove(entity);
            return await taskDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskDetails>> GetAllTasksAsync()
        {
            return await taskDbContext.Tasks.AsNoTracking<TaskDetails>().ToListAsync();
        }

        public async Task<TaskDetails> GetTaskAsync(int id)
        {
            return await taskDbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async Task<int> InsertTaskAsync(TaskDetails entity)
        {
            taskDbContext.Tasks.Add(entity);
            return await taskDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateTaskAsync(int id,TaskDetails entity)
        {           
            taskDbContext.Tasks.Update(entity);
            return await taskDbContext.SaveChangesAsync();
        }
    }
}
