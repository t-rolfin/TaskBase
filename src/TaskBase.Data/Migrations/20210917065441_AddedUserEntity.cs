using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskBase.Data.Migrations
{
    public partial class AddedUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignToId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignToId",
                table: "Tasks",
                column: "AssignToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_users_AssignToId",
                table: "Tasks",
                column: "AssignToId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_users_AssignToId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignToId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssignToId",
                table: "Tasks");
        }
    }
}
