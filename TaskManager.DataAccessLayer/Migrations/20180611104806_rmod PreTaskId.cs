using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskManager.DataAccessLayer.Migrations
{
    public partial class rmodPreTaskId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Task_ParentTaskId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_ParentTaskId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "ParentTaskId",
                table: "Task");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ParentId",
                table: "Task",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Task_ParentId",
                table: "Task",
                column: "ParentId",
                principalTable: "Task",
                principalColumn: "Task_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Task_ParentId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_ParentId",
                table: "Task");

            migrationBuilder.AddColumn<int>(
                name: "ParentTaskId",
                table: "Task",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_ParentTaskId",
                table: "Task",
                column: "ParentTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Task_ParentTaskId",
                table: "Task",
                column: "ParentTaskId",
                principalTable: "Task",
                principalColumn: "Task_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
