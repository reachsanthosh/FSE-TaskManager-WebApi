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
    public class ManageTaskTests : IClassFixture<BusinessFixture>
    {
        private BusinessFixture fixture;
        public ManageTaskTests(BusinessFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestAddTaskAsync_VerifyInsertAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetail();
            var result = await manageTask.AddTaskAsync(taskDetail);

            mockRepository.Verify(r => r.InsertAsync(taskDetail), Times.Once);
        }

        [Fact]
        public async Task TestEditTaskAsync_VerifyUpdateAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetail();
            var result = await manageTask.EditTaskAsync(10, taskDetail);

            mockRepository.Verify(r => r.UpdateAsync(10,taskDetail), Times.Once);
        }

        [Fact]
        public async Task TestViewTasksAsync_VerifyGetAllAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            
            var result = await manageTask.ViewTasksAsync();

            mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task TestGetTaskAsync_VerifyGetAsyncCalledOnce()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            
            var result = await manageTask.GetTaskAsync(10);

            mockRepository.Verify(r => r.GetAsync(10), Times.Once);
        }

        [Fact]
        public void TestIsTaskValidToClose_ReturnFalseWhenTaskContainsChildTask()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20 };
         
            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20, ParentId = 1},
            };

           mockRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult<IEnumerable<TaskDetail>>(taskDetailsList));

            var result = manageTask.IsTaskValidToClose(taskDetail);

            Assert.False(result);
        }

        [Fact]
        public void TestIsTaskValidToClose_ReturnTrueWhenTaskContainsChildTaskWhichIsNOtActive()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20 };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20, ParentId = 1, EndTask = true},
            };

            mockRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult<IEnumerable<TaskDetail>>(taskDetailsList));

            var result = manageTask.IsTaskValidToClose(taskDetail);

            Assert.True(result);
        }

        [Fact]
        public void TestIsTaskValidToClose_ReturnTrueWhenTaskDoesNotContainsChildTas()
        {
            var mockRepository = new Mock<ITaskCollection>();
            var manageTask = new TaskHandler(mockRepository.Object, fixture.Logger);
            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20 };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            mockRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult<IEnumerable<TaskDetail>>(taskDetailsList));

            var result = manageTask.IsTaskValidToClose(taskDetail);

            Assert.True(result);
        }
    }
}
