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
    public class TaskRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture fixture;
        public TaskRepositoryTests(DatabaseFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestGetAll_ReturnsTwoTaskDetails()
        {

            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
           
            var taskRepository = new TaskRepository(mockContext.Object, fixture.Logger);

            IQueryable<TaskDetail> taskDetailsList = new List<TaskDetail>()
            {
                new TaskDetail() {Id = 1, Name ="Task 1 ", Priority = 10},
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskDetail>>();

            mockSet.As<IAsyncEnumerable<TaskDetail>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<TaskDetail>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskDetail>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TaskDetail>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskDetail>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskDetail>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskDetail>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            // mockContext.SetupProperty(m => m.Tasks, mockSet.Object);

            var taskDetails = await taskRepository.GetAllAsync();

            Assert.Equal(2, taskDetails.Count());
        }

        [Fact]
        public async Task TestGet_VerifyTaskName()
        {

            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);

            var taskRepository = new TaskRepository(mockContext.Object, fixture.Logger);

            IQueryable<TaskDetail> taskDetailsList = new List<TaskDetail>()
            {
                new TaskDetail() {Id = 1, Name ="Task 1", Priority = 10},
                new TaskDetail() {Id = 2, Name ="Task 2", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskDetail>>();

            mockSet.As<IAsyncEnumerable<TaskDetail>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<TaskDetail>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskDetail>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TaskDetail>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskDetail>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskDetail>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskDetail>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            // mockContext.SetupProperty(m => m.Tasks, mockSet.Object);

            var taskDetails = await taskRepository.GetAsync(2);

            Assert.Equal("Task 2", taskDetails.Name);
        }

        [Fact]
        public async Task TestInsertAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);
          
            var taskRepository = new TaskRepository(mockContext.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskDetail>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskRepository.InsertAsync(taskDetail);

            mockSet.Verify(m => m.Add(taskDetail), Times.Once);
            mockContext.Verify(m => m. SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);           
        }

        [Fact]
        public async Task TestUpdateAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);

            var taskRepository = new TaskRepository(mockContext.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskDetail>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskRepository.UpdateAsync(1, taskDetail);

            mockSet.Verify(m => m.Update(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestRemoveAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskDbContext>();
            var mockContext = new Mock<TaskDbContext>(contextOptions);

            var taskRepository = new TaskRepository(mockContext.Object, fixture.Logger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskDetail>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskRepository.DeleteAsync(taskDetail);

            mockSet.Verify(m => m.Remove(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }
    }
}
