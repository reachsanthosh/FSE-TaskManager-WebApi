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
                var taskModel = new TaskDetail();
                var taskDbContextStub = new TaskDbContextStub(contextOptions);
              
                var modelBuilder = new ModelBuilder(new ConventionSet());
                // modelBuilder.Entity
                var model = new Microsoft.EntityFrameworkCore.Metadata.Internal.Model();
                var configSource = new ConfigurationSource();
                var entity = new EntityType("TaskModel", model, configSource);
                var internalModelBuilder = new InternalModelBuilder(model);
                var internalEntityTypeBuilder = new InternalEntityTypeBuilder(entity, internalModelBuilder);
                var entityTypeBuilder = new EntityTypeBuilder<TaskDetail>(internalEntityTypeBuilder);
                //var entityTypeBuilder = new Mock<EntityTypeBuilder<TaskDetail>>(internalEntityTypeBuilder);
                //modelBuilder.Entity<TaskDetail>()
                mockModel.Setup(m => m.Entity<TaskDetail>()).Returns(entityTypeBuilder);

                var property = new Property("Name", taskModel.GetType(), taskModel.GetType().GetProperty("Name"), taskModel.GetType().GetField("Name"), entity, configSource, null);
                var internalPropertyBuilder = new InternalPropertyBuilder(property, internalModelBuilder);
                var propertyBuilder = new PropertyBuilder<string>(internalPropertyBuilder);
                //mockModel.Setup(m => m.Entity<TaskDetail>()

                // mockModel.Setup(m => m.Entity<TaskDetail>().Property("Name")).Returns(propertyBuilder);

                taskDbContextStub.TestModelCreation(modelBuilder);

                //mockModel.Verify(m => m.Entity<TaskDetail>().HasKey("Id"), Times.Once);
            }
            catch (Exception ex)
            {
                mockModel.Verify(m => m.Entity<TaskDetail>().HasKey("Id"), Times.Once);
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
