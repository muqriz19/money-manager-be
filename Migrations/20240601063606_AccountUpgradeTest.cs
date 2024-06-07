using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneyManagerBE.Migrations
{
    /// <inheritdoc />
    public partial class AccountUpgradeTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Records_AccountId",
                table: "Records",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Accounts_AccountId",
                table: "Records",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Accounts_AccountId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_AccountId",
                table: "Records");
        }
    }
}
