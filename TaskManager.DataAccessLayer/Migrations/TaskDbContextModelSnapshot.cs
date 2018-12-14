﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TaskManager.DataAccessLayer;

namespace TaskManager.DataAccessLayer.Migrations
{
    [DbContext(typeof(TaskDbContext))]
    partial class TaskDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TaskManager.Model.TaskDetails", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TaskId");

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("End_Date");

                    b.Property<bool>("EndTask")
                        .HasColumnName("End_Task");

                    b.Property<int?>("ParentId")
                        .HasColumnName("ParentId");

                    b.Property<int>("Priority");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("Start_Date");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnName("Task_Name")
                        .HasMaxLength(100);

                    b.HasKey("TaskId");

                    b.ToTable("Task_Information");
                });
#pragma warning restore 612, 618
        }
    }
}
