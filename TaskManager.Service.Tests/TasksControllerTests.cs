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
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var taskDetailsList = new List<TaskDetail>()
            {
                new TaskDetail() {Id = 1, Name ="Task 1 ", Priority = 10},
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            mockManageTask.Setup(manage => manage.ViewTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskDetail>>(taskDetailsList));

            var statusResult = await taskRepository.GetAllAsync();

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as List<TaskDetail>;
            Assert.Equal(2, taskDetailsResult.Count);
        }

        [Fact]
        public async Task TestGetAllAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            mockManageTask.Setup(manage => manage.ViewTasksAsync()).Throws(new Exception());

            var statusResult = await taskRepository.GetAllAsync();

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }


        [Fact]
        public async Task TestGetAsync_VerifyServiceReturnOkStatusAndCheckTaskDetails()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.GetTaskAsync(1)).Returns(Task.FromResult<TaskDetail>(taskDetail));

            var statusResult = await taskRepository.GetAsync(1);

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as TaskDetail;
            Assert.Equal("Task 1", taskDetailsResult.Name);
            Assert.Equal(10, taskDetailsResult.Priority);
        }


        [Fact]
        public async Task TestGetAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            mockManageTask.Setup(manage => manage.GetTaskAsync(1)).Throws(new Exception());

            var statusResult = await taskRepository.GetAsync(1);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPostAsync_VerifyServiceReturnOkStatusAndCheckTaskId()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.AddTaskAsync(taskDetail)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.PostAsync(taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task Added Successfully and the new task id is 1001", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPostAsync_PassNullAndVerifyServiceReturnBadRequest()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var statusResult = await taskRepository.PostAsync(null);

            Assert.NotNull(statusResult as BadRequestResult);
        }

        [Fact]
        public async Task TestPostAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.AddTaskAsync(taskDetail)).Throws(new Exception());

            var statusResult = await taskRepository.PostAsync(taskDetail);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusAndCheckServiceResponse()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.EditTaskAsync(1001, taskDetail)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.PutAsync(1001, taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task Updated Successfully for the task name Task 1", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailNull()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var statusResult = await taskRepository.PutAsync(1001, null);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid task to edit.", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailIdIsInvalid()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10 };
            var statusResult = await taskRepository.PutAsync(1002, taskDetail);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid task to edit.", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailIsNotValidToClose()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10, EndTask = true };
            mockManageTask.Setup(manage => manage.IsTaskValidToClose(taskDetail)).Returns(false);
            var statusResult = await taskRepository.PutAsync(1001, taskDetail);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("You can not close this task as the task have child tasks", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusWhenTaskDetailIsValidToClose()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10, EndTask = true };

            mockManageTask.Setup(manage => manage.IsTaskValidToClose(taskDetail)).Returns(true);

            mockManageTask.Setup(manage => manage.EditTaskAsync(1001, taskDetail)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.PutAsync(1001, taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task Updated Successfully for the task name Task 1", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1001, Name = "Task 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.EditTaskAsync(1001, taskDetail)).Throws(new Exception());

            var statusResult = await taskRepository.PutAsync(1001, taskDetail);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestDeleteAsync_VerifyServiceReturnNotFoundStatus()
        {
            var mockManageTask = new Mock<IManageTask>();
            var taskRepository = new TasksController(mockManageTask.Object, fixture.Logger);

            var statusResult = await taskRepository.DeleteAsync(1001);

            Assert.NotNull(statusResult as NotFoundObjectResult);
        }
    }
}
