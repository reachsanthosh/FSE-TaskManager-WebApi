using System;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskManager.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DataAccessLayer.Tests.TestHelper;

namespace TaskManager.DataAccessLayer.Tests
{
    public class TaskCollectionTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture fixture;
        public TaskCollectionTests(DatabaseFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestGetAll_ReturnsTwoTaskDetailss()
        {

            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
            var taskCollection = new TaskCollection(mockContext.Object, fixture.Logger);

            IQueryable<TaskDetails> taskDetailsList = new List<TaskDetails>()
            {
                new TaskDetails() {TaskId =  1, TaskName = "Addition ", Priority = 10},
                new TaskDetails() {TaskId =  2, TaskName = "Multiplication ", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskDetails>>();

            mockSet.As<IAsyncEnumerable<TaskDetails>>()
            .Setup(m => m.GetEnumerator())
            .Returns(new TestAsyncEnumerator<TaskDetails>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskDetails>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TaskDetails>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());
            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var taskDetails = await taskCollection.GetAllTasksAsync();

            Assert.Equal(2, taskDetails.Count());
        }

        [Fact]
        public async Task TestGet_VerifyTaskName()
        {

            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
            var taskCollection = new TaskCollection(mockContext.Object, fixture.Logger);

            IQueryable<TaskDetails> taskDetailsList = new List<TaskDetails>()
            {
                new TaskDetails() {TaskId =  1, TaskName = "Addition", Priority = 10},
                new TaskDetails() {TaskId =  2, TaskName = "Multiplication", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskDetails>>();

            mockSet.As<IAsyncEnumerable<TaskDetails>>().Setup(m => m.GetEnumerator())
            .Returns(new TestAsyncEnumerator<TaskDetails>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<TaskDetails>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskDetails>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());
            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            
            var taskDetails = await taskCollection.GetTaskAsync(2);
            Assert.Equal("Multiplication", taskDetails.TaskName);
        }

        [Fact]
        public async Task TestInsertAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
            var taskCollection = new TaskCollection(mockContext.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId =  1, TaskName =  "Addition ", Priority = 10 };
            var mockSet = new Mock<DbSet<TaskDetails>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskCollection.InsertTaskAsync(taskDetail);

            mockSet.Verify(m => m.Add(taskDetail), Times.Once);
            mockContext.Verify(m => m. SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);           
        }

        [Fact]
        public async Task TestUpdateAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
            var taskCollection = new TaskCollection(mockContext.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId =  1, TaskName =  "Addition ", Priority = 10 };
            var mockSet = new Mock<DbSet<TaskDetails>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskCollection.UpdateTaskAsync(1, taskDetail);

            mockSet.Verify(m => m.Update(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestRemoveAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
            var taskCollection = new TaskCollection(mockContext.Object, fixture.Logger);
            var taskDetail = new TaskDetails() { TaskId =  1, TaskName =  "Addition ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskDetails>>();
            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskCollection.DeleteTaskAsync(taskDetail);

            mockSet.Verify(m => m.Remove(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }
    }
}
