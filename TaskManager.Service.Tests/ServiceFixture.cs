using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Service.Controllers;

namespace TaskManager.Service.Tests
{
    public class ServiceFixture : IDisposable
    {
        public ServiceFixture()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<TasksController>();
        }

        public ILogger<TasksController> Logger { get; private set; }
        public void Dispose()
        {

        }
    }
}
