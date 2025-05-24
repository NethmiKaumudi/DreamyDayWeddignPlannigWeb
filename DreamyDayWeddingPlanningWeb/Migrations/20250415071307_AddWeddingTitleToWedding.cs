using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamyDayWeddingPlanningWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingTitleToWedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "SpentBudget",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "TotalBudget",
                table: "Weddings");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserId",
                table: "WeddingTasks",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "WeddingId",
                table: "WeddingTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Weddings",
                keyColumn: "PlannerId",
                keyValue: null,
                column: "PlannerId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PlannerId",
                table: "Weddings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "WeddingTitle",
                table: "Weddings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingTasks_AssignedToUserId",
                table: "WeddingTasks",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingTasks_WeddingId",
                table: "WeddingTasks",
                column: "WeddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTasks_AspNetUsers_AssignedToUserId",
                table: "WeddingTasks",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTasks_Weddings_WeddingId",
                table: "WeddingTasks",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTasks_AspNetUsers_AssignedToUserId",
                table: "WeddingTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTasks_Weddings_WeddingId",
                table: "WeddingTasks");

            migrationBuilder.DropIndex(
                name: "IX_WeddingTasks_AssignedToUserId",
                table: "WeddingTasks");

            migrationBuilder.DropIndex(
                name: "IX_WeddingTasks_WeddingId",
                table: "WeddingTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "WeddingTasks");

            migrationBuilder.DropColumn(
                name: "WeddingId",
                table: "WeddingTasks");

            migrationBuilder.DropColumn(
                name: "WeddingTitle",
                table: "Weddings");

            migrationBuilder.AlterColumn<string>(
                name: "PlannerId",
                table: "Weddings",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Weddings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Progress",
                table: "Weddings",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "SpentBudget",
                table: "Weddings",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalBudget",
                table: "Weddings",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
