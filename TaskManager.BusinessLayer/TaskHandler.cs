using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DataAccessLayer;
using TaskManager.Model;

namespace TaskManager.BusinessLayer
{
    public class TaskHandler : ITaskHandler
        {
        private readonly ITaskCollection taskCollection;
        private readonly ILogger<TaskHandler> logger;
        public TaskHandler(ITaskCollection taskCollection, ILogger<TaskHandler> logger)
        {
            this.taskCollection = taskCollection;
            this.logger = logger;
        }
            
        public async Task<int> AddTaskAsync(TaskDetails taskDetails)
        {
            logger.LogInformation($"Insert a task for the id { taskDetails.TaskId }");
            return await taskCollection.InsertTaskAsync(taskDetails);
        }

        public async Task<int> EditTaskAsync(int id, TaskDetails taskDetails)
        {
            logger.LogInformation($"Update a task for the id { id }");
            return await taskCollection.UpdateTaskAsync(id, taskDetails);
        }

        public async Task<IEnumerable<TaskDetails>> ViewTasksAsync()
        {
           logger.LogInformation("Getting All Tasks");
           return await taskCollection.GetAllTasksAsync();
        }

       public async Task<TaskDetails> GetTaskAsync(int id)
        {
            logger.LogInformation($"Getting task details for the id { id }");
            return await taskCollection.GetTaskAsync(id);
        }

        public bool IsTaskValidToClose(TaskDetails taskDetails)
        {
            logger.LogInformation("Check if the ask is valid to close it");
            var tasks = taskCollection.GetAllTasksAsync().Result;
            return !tasks.Any(task => task.ParentId == taskDetails.TaskId && !task.EndTask);
        }
    }
}
