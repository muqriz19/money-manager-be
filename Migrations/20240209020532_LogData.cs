using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneyManagerBE.Migrations
{
    /// <inheritdoc />
    public partial class LogData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Records_RecordId",
                table: "Log");

            migrationBuilder.AlterColumn<int>(
                name: "RecordId",
                table: "Log",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Log",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Log",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Log",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Log",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Records_RecordId",
                table: "Log",
                column: "RecordId",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Records_RecordId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Log");

            migrationBuilder.AlterColumn<int>(
                name: "RecordId",
                table: "Log",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Records_RecordId",
                table: "Log",
                column: "RecordId",
                principalTable: "Records",
                principalColumn: "Id");
        }
    }
}
