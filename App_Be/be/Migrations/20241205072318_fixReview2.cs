using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class fixReview2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Review_Review_Id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Review_ReviewID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReviewID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_Review_Id",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "667183d7-389f-4ef0-a4b7-42e639f9ef95");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e4327488-e7bb-4298-8724-b1e6d7ec9b45");

            migrationBuilder.DropColumn(
                name: "ReviewID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Review_Id",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d8e748c-adf6-465a-bfad-f16bd78124f6", null, "Admin", "ADMIN" },
                    { "c3062cf8-644e-44f8-af76-32f6ff5ab842", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProductID",
                table: "Review",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Products_ProductID",
                table: "Review",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Products_ProductID",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_ProductID",
                table: "Review");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "7d8e748c-adf6-465a-bfad-f16bd78124f6");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c3062cf8-644e-44f8-af76-32f6ff5ab842");

            migrationBuilder.AddColumn<string>(
                name: "ReviewID",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review_Id",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "667183d7-389f-4ef0-a4b7-42e639f9ef95", null, "User", "USER" },
                    { "e4327488-e7bb-4298-8724-b1e6d7ec9b45", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReviewID",
                table: "Users",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Review_Id",
                table: "Products",
                column: "Review_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Review_Review_Id",
                table: "Products",
                column: "Review_Id",
                principalTable: "Review",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Review_ReviewID",
                table: "Users",
                column: "ReviewID",
                principalTable: "Review",
                principalColumn: "Id");
        }
    }
}
