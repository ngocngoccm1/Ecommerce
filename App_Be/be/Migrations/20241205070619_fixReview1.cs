using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class fixReview1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "11007836-87a8-4379-9216-af42dde3f3e4");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "aee04090-8ae2-437e-b2dd-7dc775a97e5b");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Review",
                newName: "CreateDate");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "667183d7-389f-4ef0-a4b7-42e639f9ef95", null, "User", "USER" },
                    { "e4327488-e7bb-4298-8724-b1e6d7ec9b45", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "667183d7-389f-4ef0-a4b7-42e639f9ef95");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e4327488-e7bb-4298-8724-b1e6d7ec9b45");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Review",
                newName: "OrderDate");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11007836-87a8-4379-9216-af42dde3f3e4", null, "Admin", "ADMIN" },
                    { "aee04090-8ae2-437e-b2dd-7dc775a97e5b", null, "User", "USER" }
                });
        }
    }
}
