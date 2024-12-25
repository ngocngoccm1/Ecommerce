using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class fixReview3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "7d8e748c-adf6-465a-bfad-f16bd78124f6");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c3062cf8-644e-44f8-af76-32f6ff5ab842");

            migrationBuilder.AddColumn<string>(
                name: "user_liked",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12b99a3d-e4c9-4537-be59-84b0546ddda6", null, "Admin", "ADMIN" },
                    { "8d48a1fd-8980-4c5b-a673-b253ee6c86c6", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "12b99a3d-e4c9-4537-be59-84b0546ddda6");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "8d48a1fd-8980-4c5b-a673-b253ee6c86c6");

            migrationBuilder.DropColumn(
                name: "user_liked",
                table: "Review");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d8e748c-adf6-465a-bfad-f16bd78124f6", null, "Admin", "ADMIN" },
                    { "c3062cf8-644e-44f8-af76-32f6ff5ab842", null, "User", "USER" }
                });
        }
    }
}
