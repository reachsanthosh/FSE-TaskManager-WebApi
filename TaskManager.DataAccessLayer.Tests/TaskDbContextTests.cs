using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Model;
using Xunit;

namespace TaskManager.DataAccessLayer.Tests
{
    public class TaskDbContextTests
    {
        [Fact]
        public void OnModelCreating_VerifyModelCreation()
        {
            var mockModel = new Mock<ModelBuilder>(new ConventionSet());
            try
            {
                var contextOptions = new DbContextOptions<TaskDbContext>();
                var taskModel = new TaskDetails();
                var taskDbContextStub = new TaskDbContextStub(contextOptions);
              
                var modelBuilder = new ModelBuilder(new ConventionSet());
                var model = new Microsoft.EntityFrameworkCore.Metadata.Internal.Model();
                var configSource = new ConfigurationSource();
                var entity = new EntityType("TaskModel", model, configSource);
                var internalModelBuilder = new InternalModelBuilder(model);
                var internalEntityTypeBuilder = new InternalEntityTypeBuilder(entity, internalModelBuilder);
                var entityTypeBuilder = new EntityTypeBuilder<TaskDetails>(internalEntityTypeBuilder);

                mockModel.Setup(m => m.Entity<TaskDetails>()).Returns(entityTypeBuilder);

                var property = new Property("TaskName", taskModel.GetType(), taskModel.GetType().GetProperty("TaskName"), taskModel.GetType().GetField("TaskName"), entity, configSource, null);
                var internalPropertyBuilder = new InternalPropertyBuilder(property, internalModelBuilder);
                var propertyBuilder = new PropertyBuilder<string>(internalPropertyBuilder);
                taskDbContextStub.TestModelCreation(modelBuilder);

            }
            catch (Exception ex)
            {
                mockModel.Verify(m => m.Entity<TaskDetails>().HasKey("TaskId"), Times.Once);
                Assert.NotNull(ex);
            }
        }
    }

    public class TaskDbContextStub : TaskDbContext
    {
        public TaskDbContextStub(DbContextOptions options):base(options)
        {

        }
        public void TestModelCreation(ModelBuilder model)
        {
            OnModelCreating(model);           
        }
    }
}
