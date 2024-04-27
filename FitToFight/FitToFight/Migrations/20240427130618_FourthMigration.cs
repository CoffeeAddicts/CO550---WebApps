using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitToFight.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "HomePageData",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "HomePageData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "HomePageData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValueString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueBool = table.Column<bool>(type: "bit", nullable: false),
                    ValueInt = table.Column<int>(type: "int", nullable: false),
                    ValueDecimal = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "HomePageData");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "HomePageData");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "HomePageData",
                newName: "Key");
        }
    }
}
