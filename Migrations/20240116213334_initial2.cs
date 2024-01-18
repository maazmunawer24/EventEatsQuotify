using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "352637dc-ff3d-41b4-8f68-f92a9098617f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9064fe5e-4487-49ce-b4f6-852cfbda0fe2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3b2d8f6-fab6-4553-86ff-e59feede12f8", "a4d859a8-1525-4716-b192-f782ef13e37e", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b8d8c09e-3404-4fa4-95a5-abdeb2c71e3c", "87d2fb1e-ed2d-40c4-a372-928be3ce446e", "Vendor", "VENDOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3b2d8f6-fab6-4553-86ff-e59feede12f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8d8c09e-3404-4fa4-95a5-abdeb2c71e3c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "352637dc-ff3d-41b4-8f68-f92a9098617f", "9a7c3af7-5dd0-486e-a024-bf6d4074db22", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9064fe5e-4487-49ce-b4f6-852cfbda0fe2", "65ccc8d9-26be-43ea-828f-1c7f6449ebd3", "Customer", "CUSTOMER" });
        }
    }
}
