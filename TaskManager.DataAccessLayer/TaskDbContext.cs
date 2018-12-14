using Microsoft.EntityFrameworkCore;
using System;
using TaskManager.Model;

namespace TaskManager.DataAccessLayer
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions options):base(options)
        {
            
        }
        public virtual DbSet<TaskDetails> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DOTNET; Database = TaskManager; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDetails>().HasKey("TaskId");           
            modelBuilder.Entity<TaskDetails>().ToTable("Task_Information");         
            modelBuilder.Entity<TaskDetails>().Property(t => t.TaskName).HasColumnName("Task_Name").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<TaskDetails>().Property(t => t.StartDate).HasColumnName("Start_Date").IsRequired();
            modelBuilder.Entity<TaskDetails>().Property(t => t.EndDate).HasColumnName("End_Date").IsRequired();
            modelBuilder.Entity<TaskDetails>().Property(t => t.ParentId).HasColumnName("ParentId");
            modelBuilder.Entity<TaskDetails>().Property(t => t.Priority).IsRequired();
            modelBuilder.Entity<TaskDetails>().Property(t => t.EndTask).HasColumnName("End_Task").IsRequired();
            modelBuilder.Entity<TaskDetails>().Property(t => t.TaskId).ValueGeneratedOnAdd().HasColumnName("TaskId").IsRequired();
        }
    }
}
