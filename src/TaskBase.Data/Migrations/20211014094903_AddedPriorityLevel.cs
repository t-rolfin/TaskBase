using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskBase.Data.Migrations
{
    public partial class AddedPriorityLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "priorityLevelId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PriorityLevels",
                columns: table => new
                {
                    Value = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityLevels", x => x.Value);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_priorityLevelId",
                table: "Tasks",
                column: "priorityLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_PriorityLevels_priorityLevelId",
                table: "Tasks",
                column: "priorityLevelId",
                principalTable: "PriorityLevels",
                principalColumn: "Value",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_PriorityLevels_priorityLevelId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "PriorityLevels");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_priorityLevelId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "priorityLevelId",
                table: "Tasks");
        }
    }
}
