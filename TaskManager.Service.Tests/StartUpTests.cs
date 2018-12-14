using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.BusinessLayer;
using Xunit;

namespace TaskManager.Service.Tests
{
    public class StartUpTests
    {
        [Fact]
        public void TestBuild()
        {
            var configuration = new Mock<IConfiguration>();
            var serviceCollection = new ServiceCollection();
            configuration.Setup(config => config.GetSection("Database").GetSection("Connection").Value).Returns("DummyConnection");
            var startUp = new Startup(configuration.Object);
        
            startUp.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result = serviceProvider.GetService<ITaskHandler>();
            Assert.NotNull(result);           
        }
       
    }

}
