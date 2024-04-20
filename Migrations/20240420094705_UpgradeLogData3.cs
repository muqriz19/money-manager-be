using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace moneyManagerBE.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeLogData3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Categories_CategoryId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Records_RecordId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Logs_LogId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Logs_CategoryId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_RecordId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Logs",
                type: "json",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RecordId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Log_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_CategoryId",
                table: "Log",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_RecordId",
                table: "Log",
                column: "RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Log_LogId",
                table: "Transactions",
                column: "LogId",
                principalTable: "Log",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Log_LogId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Logs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CategoryId",
                table: "Logs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RecordId",
                table: "Logs",
                column: "RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Categories_CategoryId",
                table: "Logs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Records_RecordId",
                table: "Logs",
                column: "RecordId",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Logs_LogId",
                table: "Transactions",
                column: "LogId",
                principalTable: "Logs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
