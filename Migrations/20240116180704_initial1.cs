using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07ce83de-b1ae-4d24-8081-cba450888f78");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a17325f2-70c4-4371-8357-584fe56d16dd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "352637dc-ff3d-41b4-8f68-f92a9098617f", "9a7c3af7-5dd0-486e-a024-bf6d4074db22", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9064fe5e-4487-49ce-b4f6-852cfbda0fe2", "65ccc8d9-26be-43ea-828f-1c7f6449ebd3", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "07ce83de-b1ae-4d24-8081-cba450888f78", "61f3a259-99a4-4e0a-8a62-e95267c97c30", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a17325f2-70c4-4371-8357-584fe56d16dd", "f3f0b8c6-abc6-46aa-8645-d51338ec987b", "Customer", "CUSTOMER" });
        }
    }
}
