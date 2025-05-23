using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionImmo.Migrations
{
    /// <inheritdoc />
    public partial class ConvertRoleToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ab35ede-7489-49f6-af05-f7043cd74093"),
                column: "Role",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("95bd3141-36a9-4dd6-adf1-291875bb8c83"),
                column: "Role",
                value: "AGENT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ab35ede-7489-49f6-af05-f7043cd74093"),
                column: "Role",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("95bd3141-36a9-4dd6-adf1-291875bb8c83"),
                column: "Role",
                value: 1);
        }
    }
}
