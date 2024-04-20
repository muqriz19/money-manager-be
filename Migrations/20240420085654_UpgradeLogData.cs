using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneyManagerBE.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeLogData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Logs_CategoryId",
                table: "Logs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Categories_CategoryId",
                table: "Logs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Categories_CategoryId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_CategoryId",
                table: "Logs");
        }
    }
}
