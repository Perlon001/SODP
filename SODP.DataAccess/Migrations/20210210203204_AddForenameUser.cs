using Microsoft.EntityFrameworkCore.Migrations;

namespace SODP.DataAccess.Migrations
{
    public partial class AddForenameUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Forename",
                table: "Users",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                maxLength: 256,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ac945167-2f99-4027-82f0-dc3888d50453");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c6693227-6222-494b-b7b6-26f6a2b02953");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "50454fe8-20e1-45eb-ad2e-4286d2677254");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2868ba68-6fa9-4059-bade-1f2dc65435e4", "AQAAAAEAACcQAAAAEEQolqs0q/lKmREOv5Ufd7F9+erLRBUW3X87pv79q+g9bHS1Wsgrf/rhPARzHTPHEw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Forename",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fa340de8-3547-4387-a485-73905ff02698");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "57c5b71d-1b76-4621-abb2-8341a6df2334");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6375be1f-9336-472c-9efc-ce6ed1c62fd2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5270d229-23c3-4fe5-aa90-d4beccb3060e", "AQAAAAEAACcQAAAAENnbiLN8yU9C0WilhlZYo8hOLZU/dVpUQDHrSx6hgyx0ozViGEKLrcbChhHuNqzp/g==" });
        }
    }
}
