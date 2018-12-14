using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.BusinessLayer.Tests
{
    public class BusinessFixture : IDisposable
    {
        public BusinessFixture()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<ManageTask>();
        }

        public ILogger<ManageTask> Logger { get; private set; }
        public void Dispose()
        {

        }
    }
}
