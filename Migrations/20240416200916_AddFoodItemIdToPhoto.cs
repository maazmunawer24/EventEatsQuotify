using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class AddFoodItemIdToPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_FoodItems_FoodItemId",
                table: "Photos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f80dc57-b9b1-4cee-807e-352536257442");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a911eaf0-dddf-4d62-9c69-e9bd5bd058a8");

            migrationBuilder.AlterColumn<int>(
                name: "FoodItemId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "31d71bc8-b0ab-41c9-bae8-1ee4969800af", "5cb26ecb-2aea-4971-bb6a-db6fe51d0365", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a16c9e3d-0766-4949-80f9-76af7f2b4dc0", "b898e786-6377-4149-8a34-fc842863c597", "Vendor", "VENDOR" });

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_FoodItems_FoodItemId",
                table: "Photos",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_FoodItems_FoodItemId",
                table: "Photos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31d71bc8-b0ab-41c9-bae8-1ee4969800af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a16c9e3d-0766-4949-80f9-76af7f2b4dc0");

            migrationBuilder.AlterColumn<int>(
                name: "FoodItemId",
                table: "Photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2f80dc57-b9b1-4cee-807e-352536257442", "16e232db-7c68-42b3-9f1e-91e91a0cefdc", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a911eaf0-dddf-4d62-9c69-e9bd5bd058a8", "8c0ceecb-b8f9-4afb-9e94-1b874f9f868d", "Customer", "CUSTOMER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_FoodItems_FoodItemId",
                table: "Photos",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id");
        }
    }
}
