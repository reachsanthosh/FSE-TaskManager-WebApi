using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskManager.DataAccessLayer.Migrations
{
    public partial class parenttaskchangemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Task_ParentId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_ParentId",
                table: "Task");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
