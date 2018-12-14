using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.BusinessLayer;
using TaskManager.Model;

namespace TaskManager.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/Tasks")]
    public class TasksController : Controller
    {
        private readonly ITaskHandler manageTask;
        private readonly ILogger<TasksController> logger;
        public TasksController(ITaskHandler manageTask, ILogger<TasksController> logger)
        {
            this.manageTask = manageTask;
            this.logger = logger;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            try
            {
                logger.LogInformation("Controller: Get all the tasks is being executed!");
                return Ok(await manageTask.ViewTasksAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);                
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error , please verify the log and try again!");               
            }          
        }

        // GET: api/Tasks/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetTaskAsync(int id)
        {
            try
            {
                logger.LogInformation($"Controller: Geting the tasks for Task Id-{id}");
                return Ok(await manageTask.GetTaskAsync(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error , please verify the log and try again!");
            }
        }
        
        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostTaskAsync([FromBody]TaskDetails taskDetail)
        {
            try
            {
                if (taskDetail == null)
                {
                    logger.LogInformation($"Task is null or invalid, please provide valid task details.");
                    return BadRequest();
                }
                logger.LogInformation($"Controller: Posting the tasks for Task Id-{taskDetail.TaskId}");
                await manageTask.AddTaskAsync(taskDetail);
                logger.LogInformation($"Task is added successfully and the new task id is { taskDetail.TaskId } and task name {taskDetail.TaskName}");
               
                return Ok($"Task is added successfully and the new task id is { taskDetail.TaskId } and task name { taskDetail.TaskName}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error , please verify the log and try again!");
            }
        }
        
        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskAsync(int id, [FromBody]TaskDetails taskDetail)
        {
            try
            { 
                
                if (taskDetail == null || id != taskDetail.TaskId)
                {
                    logger.LogInformation("Task is null or invalid, please provide valid task details.");
                    return BadRequest("Task is null or invalid, please provide valid task details.");
                }
                if (taskDetail.EndTask && !manageTask.IsTaskValidToClose(taskDetail))
                {
                    logger.LogInformation("You can not close this task as the task have child tasks");
                    return BadRequest("You can not close this task as the task have child tasks");
                }
                logger.LogInformation($"Controller: Updating the tasks for Task Id-{taskDetail.TaskId}");
                await manageTask.EditTaskAsync(id, taskDetail);
                logger.LogInformation($"Task is updated successfully and the task id is { taskDetail.TaskId } and task name { taskDetail.TaskName}");
                return Ok($"Task is updated successfully and the task id is { taskDetail.TaskId } and task name { taskDetail.TaskName}");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error , please verify the log and try again!");
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteTaskAsync(TaskDetails taskDetail)
        {
            logger.LogInformation($"Controller: Deleting the tasks for Task Id-{taskDetail.TaskId}");
            if (taskDetail == null)
            {
                logger.LogInformation("Task is null or invalid, please provide valid task details.");
                return BadRequest("Task is null or invalid, please provide valid task details.");
            }
            if (manageTask.IsTaskValidToClose(taskDetail))
               await manageTask.DeleteTaskAsync(taskDetail);
            
            logger.LogInformation($"Task is deleted successfully for the task id is { taskDetail.TaskId } and task name { taskDetail.TaskName}");
            return Ok($"Task is deleted successfully for the task id is { taskDetail.TaskId } and task name { taskDetail.TaskName}");
        }
    }
}
