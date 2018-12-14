using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    public class DIBuilderTests
    {
        [Fact]
        public void TestBuild_VerifyDependendencyObjectsAreNotNull()
        {
            var serviceCollection = new ServiceCollection();
            var configuration = new Mock<IConfiguration>();

            configuration.Setup(config => config.GetSection("Database").GetSection("Connection").Value).Returns("DummyConnection");
            DIBuilder.Build(serviceCollection, configuration.Object);
            var sp = serviceCollection.BuildServiceProvider();
            var result = sp.GetService<ITaskRepository>();
            Assert.NotNull(result);
            var dbContext = sp.GetService<TaskDbContext>();
            Assert.NotNull(dbContext);
        }
    }
}
