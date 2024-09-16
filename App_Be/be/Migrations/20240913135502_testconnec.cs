using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class testconnec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2d5888ea-d639-4616-a8b1-b794a76b44e8");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "be312bec-9ad0-45ae-878c-6319a2df56df");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f81d05f-deeb-432b-a335-cc00d3a98945", null, "User", "USER" },
                    { "965be3b1-ef8c-4ca6-99dc-7506a97c7352", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1f81d05f-deeb-432b-a335-cc00d3a98945");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "965be3b1-ef8c-4ca6-99dc-7506a97c7352");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2d5888ea-d639-4616-a8b1-b794a76b44e8", null, "Admin", "ADMIN" },
                    { "be312bec-9ad0-45ae-878c-6319a2df56df", null, "User", "USER" }
                });
        }
    }
}
