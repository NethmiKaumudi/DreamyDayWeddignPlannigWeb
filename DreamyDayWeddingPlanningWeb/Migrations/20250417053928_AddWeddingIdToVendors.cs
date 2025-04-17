using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamyDayWeddingPlanningWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingIdToVendors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeddingId",
                table: "Vendors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_WeddingId",
                table: "Vendors",
                column: "WeddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Weddings_WeddingId",
                table: "Vendors",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Weddings_WeddingId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_WeddingId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "WeddingId",
                table: "Vendors");
        }
    }
}
