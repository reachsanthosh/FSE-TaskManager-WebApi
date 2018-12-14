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
        private readonly IManageTask manageTask;
        private readonly ILogger<TasksController> logger;
        public TasksController(IManageTask manageTask, ILogger<TasksController> logger)
        {
            this.manageTask = manageTask;
            this.logger = logger;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                logger.LogInformation("Executing Get All Method");
                              
                return Ok(await manageTask.ViewTasksAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);                
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");               
            }          
        }

        // GET: api/Tasks/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                logger.LogInformation($"Getting task details for {id}");
              
                return Ok(await manageTask.GetTaskAsync(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }
        
        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]TaskDetail taskDetail)
        {
            try
            {
                if (taskDetail == null)
                {
                    logger.LogInformation($"Task is null.  Provide valid task details.");
                    return BadRequest();
                }

                await manageTask.AddTaskAsync(taskDetail);

                logger.LogInformation($"Task Added Successfully and the new task id is { taskDetail.Id }");
               
               return Ok($"Task Added Successfully and the new task id is { taskDetail.Id }");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }
        
        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]TaskDetail taskDetail)
        {
            try
            { 
                logger.LogInformation($"Updating task {id}");
                if (taskDetail == null || id != taskDetail.Id)
                {
                    logger.LogInformation("Invalid Task to edit");
                    return BadRequest("Invalid task to edit.");
                }
                if (taskDetail.EndTask && !manageTask.IsTaskValidToClose(taskDetail))
                {
                    logger.LogInformation("You can not close this task as the task have child tasks");
                    return BadRequest("You can not close this task as the task have child tasks");
                }

                await manageTask.EditTaskAsync(id, taskDetail);
                logger.LogInformation($"Task Updated Successfully for the task name { taskDetail.Name } ");
                return Ok($"Task Updated Successfully for the task name { taskDetail.Name }");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            logger.LogInformation($"Deleting Task is not accessible");
            return NotFound("Deleting Task is not accessible");
        }
    }
}
