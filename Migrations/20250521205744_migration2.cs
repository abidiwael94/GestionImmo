using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionImmo.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "Role", "address", "email", "password", "phone" },
                values: new object[,]
                {
                    { new Guid("3ab35ede-7489-49f6-af05-f7043cd74093"), "admin S", 0, "Tunis", "se@admin.com", "password123", "20202020" },
                    { new Guid("95bd3141-36a9-4dd6-adf1-291875bb8c83"), "agent S", 1, "Ariana", "s@eagent.com", "password456", "30303030" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ab35ede-7489-49f6-af05-f7043cd74093"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("95bd3141-36a9-4dd6-adf1-291875bb8c83"));
        }
    }
}
