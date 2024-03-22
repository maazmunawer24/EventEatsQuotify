using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class fooditem_model_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c18d00f5-6e05-413e-b7df-12bb3d2ea7db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f849661d-5371-4ec4-8d2f-d7e38ed86607");

            migrationBuilder.DropColumn(
                name: "FoodPicture",
                table: "FoodItems");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FoodPicturePath",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "975354b4-d8c0-49a5-8c30-4e75dfb0065f", "4bc79bf3-452b-4079-872a-f2d2896cfb23", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9fe3828d-2151-43e1-8179-8ac42db32253", "4549d532-518c-4bcd-bf1b-18a1bd158877", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "975354b4-d8c0-49a5-8c30-4e75dfb0065f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fe3828d-2151-43e1-8179-8ac42db32253");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "FoodPicturePath",
                table: "FoodItems");

            migrationBuilder.AddColumn<byte[]>(
                name: "FoodPicture",
                table: "FoodItems",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c18d00f5-6e05-413e-b7df-12bb3d2ea7db", "f6fc819f-83c0-41cc-91fd-87e8642b97f8", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f849661d-5371-4ec4-8d2f-d7e38ed86607", "e74a0b1e-2e91-417d-86f3-91e23a291489", "Vendor", "VENDOR" });
        }
    }
}
