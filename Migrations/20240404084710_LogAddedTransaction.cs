using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneyManagerBE.Migrations
{
    /// <inheritdoc />
    public partial class LogAddedTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LogId",
                table: "Transactions",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Logs_LogId",
                table: "Transactions",
                column: "LogId",
                principalTable: "Logs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Logs_LogId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_LogId",
                table: "Transactions");
        }
    }
}
