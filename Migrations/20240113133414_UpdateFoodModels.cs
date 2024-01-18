using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class UpdateFoodModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d921a91-8f96-40d2-b327-ec00997131de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72640be3-3eed-4031-adcd-af91ed754fb3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "22b6f4f6-3ec6-4c9d-88b8-0c8c9eac702f", "b5148548-d102-4251-bb84-e419ebf2f229", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6e2df9dc-75a0-4d02-82b9-02cb4e6c1b40", "2afd4088-21ec-4e4a-9922-fc3c1cf04041", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22b6f4f6-3ec6-4c9d-88b8-0c8c9eac702f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e2df9dc-75a0-4d02-82b9-02cb4e6c1b40");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0d921a91-8f96-40d2-b327-ec00997131de", "71f91133-60fb-41f3-9f5f-7e7d34869144", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "72640be3-3eed-4031-adcd-af91ed754fb3", "0393a3ab-1ff0-4f5c-afc6-2ee619a6eafb", "Customer", "CUSTOMER" });
        }
    }
}
