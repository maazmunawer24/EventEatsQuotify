using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_AspNetUsers_VendorId",
                table: "FoodItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3b2d8f6-fab6-4553-86ff-e59feede12f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8d8c09e-3404-4fa4-95a5-abdeb2c71e3c");

            migrationBuilder.AlterColumn<string>(
                name: "VendorId",
                table: "FoodItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "673fb540-214c-478a-be3b-6d6f7689ff02", "f3dc35d9-101d-47b7-9a32-54b0b4c6fe33", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7cb688d8-e36b-4d60-acf1-9148c9551ba5", "1c690ff1-6a1b-40e8-bcc9-6d6397125577", "Customer", "CUSTOMER" });

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_AspNetUsers_VendorId",
                table: "FoodItems",
                column: "VendorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_AspNetUsers_VendorId",
                table: "FoodItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673fb540-214c-478a-be3b-6d6f7689ff02");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cb688d8-e36b-4d60-acf1-9148c9551ba5");

            migrationBuilder.AlterColumn<string>(
                name: "VendorId",
                table: "FoodItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3b2d8f6-fab6-4553-86ff-e59feede12f8", "a4d859a8-1525-4716-b192-f782ef13e37e", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b8d8c09e-3404-4fa4-95a5-abdeb2c71e3c", "87d2fb1e-ed2d-40c4-a372-928be3ce446e", "Vendor", "VENDOR" });

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_AspNetUsers_VendorId",
                table: "FoodItems",
                column: "VendorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
