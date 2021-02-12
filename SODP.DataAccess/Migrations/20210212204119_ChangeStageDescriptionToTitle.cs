using Microsoft.EntityFrameworkCore.Migrations;

namespace SODP.DataAccess.Migrations
{
    public partial class ChangeStageDescriptionToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stages");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Stages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Stages");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stages",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
