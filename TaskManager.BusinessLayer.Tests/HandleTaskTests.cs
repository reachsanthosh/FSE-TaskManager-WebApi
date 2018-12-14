using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DataAccessLayer;
using TaskManager.Model;
using Xunit;

namespace TaskManager.BusinessLayer.Tests
{
    public class HandleTaskTests : IClassFixture<BusinessFixture>
    {
        private BusinessFixture fixture;
        public HandleTaskTests(BusinessFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestAddTaskAsync_VerifyInsertAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetails();
            var result = await manageTask.AddTaskAsync(taskDetail);

            mockRepository.Verify(r => r.InsertTaskAsync(taskDetail), Times.Once);
        }

        [Fact]
        public async Task TestEditTaskAsync_VerifyUpdateAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetails();
            var result = await manageTask.EditTaskAsync(10, taskDetail);

            mockRepository.Verify(r => r.UpdateTaskAsync(10,taskDetail), Times.Once);
        }

        [Fact]
        public async Task TestViewTasksAsync_VerifyGetAllAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            
            var result = await manageTask.ViewTasksAsync();

            mockRepository.Verify(r => r.GetAllTasksAsync(), Times.Once);
        }

        [Fact]
        public async Task TestGetTaskAsync_VerifyGetAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            
            var result = await manageTask.GetTaskAsync(10);

            mockRepository.Verify(r => r.GetTaskAsync(10), Times.Once);
        }

        [Fact]
        public void TestIsTaskValidToClose_ReturnFalseWhenTaskContainsChildTask()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 1, TaskName = "Addition", Priority = 20 };
         
            var taskDetailsList = new List<TaskDetails>()
            {
                taskDetail,
                new TaskDetails() {TaskId = 2, TaskName = "Multiplication", Priority = 20, ParentId = 1},
            };

           mockRepository.Setup(r => r.GetAllTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskDetails>>(taskDetailsList));

            var result = manageTask.IsTaskValidToClose(taskDetail);

            Assert.False(result);
        }

        [Fact]
        public void TestIsTaskValidToClose_ReturnTrueWhenTaskContainsChildTaskWhichIsNOtActive()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 1, TaskName = "Addition", Priority = 20 };

            var taskDetailsList = new List<TaskDetails>()
            {
                taskDetail,
                new TaskDetails() {TaskId = 2, TaskName = "Multiplication", Priority = 20, ParentId = 1, EndTask = true},
            };

            mockRepository.Setup(r => r.GetAllTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskDetails>>(taskDetailsList));

            var result = manageTask.IsTaskValidToClose(taskDetail);

            Assert.True(result);
        }

        [Fact]
        public void TestIsTaskValidToClose_ReturnTrueWhenTaskDoesNotContainsChildTas()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId = 1, TaskName = "Addition", Priority = 20 };

            var taskDetailsList = new List<TaskDetails>()
            {
                taskDetail,
                new TaskDetails() {TaskId = 2, TaskName = "Multiplication", Priority = 20},
            };

            mockRepository.Setup(r => r.GetAllTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskDetails>>(taskDetailsList));

            var result = manageTask.IsTaskValidToClose(taskDetail);

            Assert.True(result);
        }
    }
}
