using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ArabicName", "EnglishName", "IsDeleted", "Price" },
                values: new object[,]
                {
                    { 1, "وردة الياسمين", "Jasmine Flower", false, 3m },
                    { 2, "وردة جورية", "Joriya Rose", false, 22m },
                    { 3, "وردة حمراء", "Red Flower", false, 6m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@flowers.com", "Rahaf Mohammad", new byte[] { 50, 142, 147, 41, 239, 239, 255, 181, 13, 238, 107, 32, 228, 52, 250, 40, 84, 80, 104, 198, 168, 87, 191, 89, 85, 139, 35, 31, 34, 120, 250, 221, 226, 255, 216, 134, 88, 188, 139, 204, 171, 222, 80, 237, 60, 187, 134, 243, 3, 127, 32, 19, 228, 181, 140, 85, 191, 68, 107, 54, 223, 201, 198, 122 }, new byte[] { 175, 119, 165, 20, 236, 154, 188, 183, 234, 70, 18, 121, 128, 125, 122, 237, 253, 129, 62, 250, 15, 138, 131, 51, 86, 247, 112, 16, 179, 192, 167, 131, 201, 65, 89, 30, 117, 13, 7, 16, 80, 202, 163, 95, 155, 168, 235, 142, 90, 225, 97, 168, 5, 232, 39, 34, 37, 187, 191, 74, 214, 250, 76, 223, 108, 91, 58, 47, 234, 205, 117, 14, 126, 129, 110, 100, 58, 129, 92, 123, 98, 24, 125, 102, 232, 60, 23, 32, 23, 125, 126, 27, 40, 135, 245, 186, 47, 176, 72, 43, 72, 174, 157, 254, 73, 58, 98, 207, 45, 137, 242, 254, 79, 171, 181, 156, 24, 0, 41, 113, 16, 208, 99, 133, 147, 149, 1, 39 }, "Admin", "RahafAdmin" },
                    { 2, "momen@gmail.com", "Momen Mohammad", new byte[] { 59, 183, 203, 20, 15, 236, 71, 204, 28, 162, 29, 53, 48, 206, 233, 31, 109, 49, 195, 175, 170, 113, 150, 189, 130, 129, 183, 42, 4, 45, 3, 106, 134, 168, 238, 190, 166, 38, 67, 248, 126, 14, 1, 109, 198, 226, 3, 163, 82, 45, 10, 83, 225, 213, 17, 190, 85, 220, 144, 180, 60, 162, 28, 121 }, new byte[] { 233, 86, 105, 71, 165, 140, 104, 107, 74, 48, 213, 110, 222, 152, 153, 126, 91, 153, 221, 94, 66, 216, 14, 195, 179, 115, 188, 118, 63, 165, 29, 0, 88, 153, 75, 74, 101, 169, 141, 205, 106, 157, 42, 62, 84, 89, 11, 238, 1, 8, 56, 97, 244, 64, 171, 213, 2, 203, 208, 165, 223, 119, 169, 32, 224, 91, 130, 110, 25, 254, 70, 137, 9, 127, 99, 141, 83, 240, 171, 123, 4, 207, 223, 15, 194, 228, 104, 16, 3, 71, 181, 86, 146, 70, 186, 126, 154, 236, 204, 92, 118, 73, 150, 141, 110, 0, 15, 88, 8, 3, 47, 97, 151, 1, 193, 141, 30, 69, 0, 109, 76, 127, 8, 138, 194, 51, 203, 55 }, "User", "Momen123" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_ProductId",
                table: "InvoiceDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
