﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceTask.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndProductSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 3.7m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 2.2m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 6.4m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordHash", "PasswordSalt" },
                values: new object[] { "1234", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordHash", "PasswordSalt" },
                values: new object[] { "1234", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 3m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 22m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 6m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 50, 142, 147, 41, 239, 239, 255, 181, 13, 238, 107, 32, 228, 52, 250, 40, 84, 80, 104, 198, 168, 87, 191, 89, 85, 139, 35, 31, 34, 120, 250, 221, 226, 255, 216, 134, 88, 188, 139, 204, 171, 222, 80, 237, 60, 187, 134, 243, 3, 127, 32, 19, 228, 181, 140, 85, 191, 68, 107, 54, 223, 201, 198, 122 }, new byte[] { 175, 119, 165, 20, 236, 154, 188, 183, 234, 70, 18, 121, 128, 125, 122, 237, 253, 129, 62, 250, 15, 138, 131, 51, 86, 247, 112, 16, 179, 192, 167, 131, 201, 65, 89, 30, 117, 13, 7, 16, 80, 202, 163, 95, 155, 168, 235, 142, 90, 225, 97, 168, 5, 232, 39, 34, 37, 187, 191, 74, 214, 250, 76, 223, 108, 91, 58, 47, 234, 205, 117, 14, 126, 129, 110, 100, 58, 129, 92, 123, 98, 24, 125, 102, 232, 60, 23, 32, 23, 125, 126, 27, 40, 135, 245, 186, 47, 176, 72, 43, 72, 174, 157, 254, 73, 58, 98, 207, 45, 137, 242, 254, 79, 171, 181, 156, 24, 0, 41, 113, 16, 208, 99, 133, 147, 149, 1, 39 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 59, 183, 203, 20, 15, 236, 71, 204, 28, 162, 29, 53, 48, 206, 233, 31, 109, 49, 195, 175, 170, 113, 150, 189, 130, 129, 183, 42, 4, 45, 3, 106, 134, 168, 238, 190, 166, 38, 67, 248, 126, 14, 1, 109, 198, 226, 3, 163, 82, 45, 10, 83, 225, 213, 17, 190, 85, 220, 144, 180, 60, 162, 28, 121 }, new byte[] { 233, 86, 105, 71, 165, 140, 104, 107, 74, 48, 213, 110, 222, 152, 153, 126, 91, 153, 221, 94, 66, 216, 14, 195, 179, 115, 188, 118, 63, 165, 29, 0, 88, 153, 75, 74, 101, 169, 141, 205, 106, 157, 42, 62, 84, 89, 11, 238, 1, 8, 56, 97, 244, 64, 171, 213, 2, 203, 208, 165, 223, 119, 169, 32, 224, 91, 130, 110, 25, 254, 70, 137, 9, 127, 99, 141, 83, 240, 171, 123, 4, 207, 223, 15, 194, 228, 104, 16, 3, 71, 181, 86, 146, 70, 186, 126, 154, 236, 204, 92, 118, 73, 150, 141, 110, 0, 15, 88, 8, 3, 47, 97, 151, 1, 193, 141, 30, 69, 0, 109, 76, 127, 8, 138, 194, 51, 203, 55 } });
        }
    }
}
