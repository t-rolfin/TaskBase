using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskBase.Data.Migrations
{
    public partial class DeleteBehaviorCascadeForNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tasks_taskId",
                table: "Notes");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tasks_taskId",
                table: "Notes",
                column: "taskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tasks_taskId",
                table: "Notes");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tasks_taskId",
                table: "Notes",
                column: "taskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
