using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneyManagerBE.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Categories",
                newName: "IconSet");

            migrationBuilder.AddColumn<string>(
                name: "IconName",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconName",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "IconSet",
                table: "Categories",
                newName: "Icon");
        }
    }
}
