using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskManager.DataAccessLayer.Migrations
{
    public partial class CreateTaskDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task_Details",
                columns: table => new
                {
                    Task_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    End_Date = table.Column<DateTime>(nullable: false),
                    Task_Name = table.Column<string>(maxLength: 100, nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task_Details", x => x.Task_Id);                   
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task");
        }
    }
}
