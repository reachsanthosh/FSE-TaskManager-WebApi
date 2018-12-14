using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TaskManager.BusinessLayer;
using TaskManager.Model;
using TaskManager.Service.Controllers;
using Xunit;

namespace TaskManager.Service.Tests
{
    public class TasksControllerTests : IClassFixture<ServiceFixture>
    {
        private ServiceFixture fixture;
        public TasksControllerTests(ServiceFixture serviceFixture)
        {
            this.fixture = serviceFixture;
        }

        [Fact]
        public async Task TestGetAllAsync_VerifyServiceReturnOkStatus()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var taskDetailsList = new List<TaskDetails>()
            {
                new TaskDetails() {TaskId = 1, TaskName ="Addition", Priority = 10},
                new TaskDetails() {TaskId = 2, TaskName ="Multiplication", Priority = 20},
            };

            mockHandleTask.Setup(manage => manage.ViewTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskDetails>>(taskDetailsList));
            var statusResult = await taskCollection.GetAllTasksAsync();

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as List<TaskDetails>;
            Assert.Equal(2, taskDetailsResult.Count);
        }

        [Fact]
        public async Task TestGetAllAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            mockHandleTask.Setup(manage => manage.ViewTasksAsync()).Throws(new Exception());

            var statusResult = await taskCollection.GetAllTasksAsync();

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }


        [Fact]
        public async Task TestGetAsync_VerifyServiceReturnOkStatusAndCheckTaskDetailss()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 1, TaskName = "Addition", Priority = 10 };

            mockHandleTask.Setup(manage => manage.GetTaskAsync(1)).Returns(Task.FromResult<TaskDetails>(taskDetail));

            var statusResult = await taskCollection.GetTaskAsync(1);

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as TaskDetails;
            Assert.Equal("Addition", taskDetailsResult.TaskName);
            Assert.Equal(10, taskDetailsResult.Priority);
        }


        [Fact]
        public async Task TestGetAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            mockHandleTask.Setup(manage => manage.GetTaskAsync(1)).Throws(new Exception());

            var statusResult = await taskCollection.GetTaskAsync(1);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPostAsync_VerifyServiceReturnOkStatusAndCheckTaskId()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10 };

            mockHandleTask.Setup(manage => manage.AddTaskAsync(taskDetail)).Returns(Task.FromResult<int>(101));

            var statusResult = await taskCollection.PostTaskAsync(taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task is added successfully and the new task id is 101 and task name Addition", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPostAsync_PassNullAndVerifyServiceReturnBadRequest()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var statusResult = await taskCollection.PostTaskAsync(null);

            Assert.NotNull(statusResult as BadRequestResult);
        }

        [Fact]
        public async Task TestPostAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10 };
            mockHandleTask.Setup(manage => manage.AddTaskAsync(taskDetail)).Throws(new Exception());

            var statusResult = await taskCollection.PostTaskAsync(taskDetail);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusAndCheckServiceResponse()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10 };

            mockHandleTask.Setup(manage => manage.EditTaskAsync(101, taskDetail)).Returns(Task.FromResult<int>(101));

            var statusResult = await taskCollection.PutTaskAsync(101, taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task is updated successfully and the task id is 101 and task name Addition", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailsNull()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var statusResult = await taskCollection.PutTaskAsync(101, null);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Task is null or invalid, please provide valid task details.", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailsIdIsInvalid()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Multiplication", Priority = 10 };
            var statusResult = await taskCollection.PutTaskAsync(102, taskDetail);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Task is null or invalid, please provide valid task details.", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailsIsNotValidToClose()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10, EndTask = true };
            mockHandleTask.Setup(manage => manage.IsTaskValidToClose(taskDetail)).Returns(false);
            var statusResult = await taskCollection.PutTaskAsync(101, taskDetail);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("You can not close this task as the task have child tasks", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusWhenTaskDetailsIsValidToClose()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10, EndTask = true };

            mockHandleTask.Setup(manage => manage.IsTaskValidToClose(taskDetail)).Returns(true);
            mockHandleTask.Setup(manage => manage.EditTaskAsync(101, taskDetail)).Returns(Task.FromResult<int>(101));

            var statusResult = await taskCollection.PutTaskAsync(101, taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);
            Assert.Equal("Task is updated successfully and the task id is 101 and task name Addition", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10 };
            mockHandleTask.Setup(manage => manage.EditTaskAsync(101, taskDetail)).Throws(new Exception());

            var statusResult = await taskCollection.PutTaskAsync(101, taskDetail);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestDeleteAsync_VerifyServiceReturnNotFoundStatus()
        {
            var mockHandleTask = new Mock<ITaskHandler>();
            var taskCollection = new TasksController(mockHandleTask.Object, fixture.Logger);

            var taskDetail = new TaskDetails() { TaskId = 101, TaskName = "Addition", Priority = 10, EndTask = true };

            mockHandleTask.Setup(manage => manage.IsTaskValidToClose(taskDetail)).Returns(true);
            mockHandleTask.Setup(manage => manage.EditTaskAsync(101, taskDetail)).Returns(Task.FromResult<int>(101));

            var statusResult = await taskCollection.DeleteTaskAsync(taskDetail);

            Assert.Equal("Task is deleted successfully for the task id is 101 and task name Addition", (statusResult as OkObjectResult).Value);
        }
    }
}
