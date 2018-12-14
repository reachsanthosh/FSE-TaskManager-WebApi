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
    public class ManageTask : IManageTask
    {
        private readonly ITaskRepository taskRepository;
        private readonly ILogger<ManageTask> logger;
        public ManageTask(ITaskRepository taskRepository, ILogger<ManageTask> logger)
        {
            this.taskRepository = taskRepository;
            this.logger = logger;
        }
            
        public async Task<int> AddTaskAsync(TaskDetail taskDetail)
        {
            return await taskRepository.InsertAsync(taskDetail);
        }

        public async Task<int> EditTaskAsync(int id, TaskDetail taskDetail)
        {
            return await taskRepository.UpdateAsync(id, taskDetail);
        }

        public async Task<IEnumerable<TaskDetail>> ViewTasksAsync()
        {
           logger.LogInformation("Getting All Tasks");
           return await taskRepository.GetAllAsync();
        }

       public async Task<TaskDetail> GetTaskAsync(int id)
        {
            logger.LogInformation($"Getting task details for the id { id }");
            return await taskRepository.GetAsync(id);
        }

        public bool IsTaskValidToClose(TaskDetail taskDetail)
        {
            logger.LogInformation("Check if Task is valid to close it");
            var taskCollection = taskRepository.GetAllAsync().Result;
            return !taskCollection.Any(task => task.ParentId == taskDetail.Id && !task.EndTask);
        }
    }
}
