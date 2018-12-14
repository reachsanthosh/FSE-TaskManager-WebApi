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
        public virtual DbSet<TaskDetail> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server = DOTNET; Database = TaskDb; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDetail>().HasKey("Id");           
            modelBuilder.Entity<TaskDetail>().ToTable("Task");            
            modelBuilder.Entity<TaskDetail>().Property(t => t.Name).HasColumnName("Task").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<TaskDetail>().Property(t => t.StartDate).HasColumnName("Start_Date").IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.EndDate).HasColumnName("End_Date").IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.ParentId).HasColumnName("ParentId");
            modelBuilder.Entity<TaskDetail>().Property(t => t.Priority).IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.EndTask).HasColumnName("End_Task").IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.Id).ValueGeneratedOnAdd().HasColumnName("Task_Id").IsRequired();
        }
    }
}
