using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamyDayWeddingPlanningWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedToWedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTasks_AspNetUsers_AssignedToUserId",
                table: "WeddingTasks");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "WeddingTasks",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Weddings",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Weddings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WeddingVendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WeddingId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    AssignedByPlannerId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeddingVendors_AspNetUsers_AssignedByPlannerId",
                        column: x => x.AssignedByPlannerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeddingVendors_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeddingVendors_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_UserId",
                table: "Weddings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingVendors_AssignedByPlannerId",
                table: "WeddingVendors",
                column: "AssignedByPlannerId");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingVendors_VendorId",
                table: "WeddingVendors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingVendors_WeddingId",
                table: "WeddingVendors",
                column: "WeddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_UserId",
                table: "Weddings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTasks_AspNetUsers_AssignedToUserId",
                table: "WeddingTasks",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_UserId",
                table: "Weddings");

            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTasks_AspNetUsers_AssignedToUserId",
                table: "WeddingTasks");

            migrationBuilder.DropTable(
                name: "WeddingVendors");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_UserId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Weddings");

            migrationBuilder.UpdateData(
                table: "WeddingTasks",
                keyColumn: "AssignedToUserId",
                keyValue: null,
                column: "AssignedToUserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "WeddingTasks",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Weddings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTasks_AspNetUsers_AssignedToUserId",
                table: "WeddingTasks",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
