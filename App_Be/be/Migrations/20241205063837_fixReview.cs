using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class fixReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2410b58a-211f-446c-96d7-f148c5caeac9");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "907c4479-09a5-48b0-92d1-587ff3f4e9bf");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11007836-87a8-4379-9216-af42dde3f3e4", null, "Admin", "ADMIN" },
                    { "aee04090-8ae2-437e-b2dd-7dc775a97e5b", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "11007836-87a8-4379-9216-af42dde3f3e4");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "aee04090-8ae2-437e-b2dd-7dc775a97e5b");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Review");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2410b58a-211f-446c-96d7-f148c5caeac9", null, "User", "USER" },
                    { "907c4479-09a5-48b0-92d1-587ff3f4e9bf", null, "Admin", "ADMIN" }
                });
        }
    }
}
