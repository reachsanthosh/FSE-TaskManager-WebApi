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
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext taskDbContext;
        private readonly ILogger<TaskRepository> logger;
        public TaskRepository(TaskDbContext taskDbContext, ILogger<TaskRepository> logger)
        {
            this.taskDbContext = taskDbContext;
            this.logger = logger;
        }
        public async Task<int> DeleteAsync(TaskDetail entity)
        {
            taskDbContext.Tasks.Remove(entity);

            return await taskDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskDetail>> GetAllAsync()
        {
            return await taskDbContext.Tasks.AsNoTracking<TaskDetail>().ToListAsync();
        }

        public async Task<TaskDetail> GetAsync(int id)
        {
            return await taskDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> InsertAsync(TaskDetail entity)
        {
            taskDbContext.Tasks.Add(entity);
            return await taskDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(int id,TaskDetail entity)
        {           
            taskDbContext.Tasks.Update(entity);
            return await taskDbContext.SaveChangesAsync();
        }
    }
}
