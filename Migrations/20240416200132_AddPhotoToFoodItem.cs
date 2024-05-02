using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class AddPhotoToFoodItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_FoodItems_FoodItemId",
                table: "Photo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50807652-a42e-4c39-9e84-0c3dbc4b4734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fbb191d-c8bc-4190-a0e2-390e40c8484a");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_FoodItemId",
                table: "Photos",
                newName: "IX_Photos_FoodItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_FoodItems_FoodItemId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f80dc57-b9b1-4cee-807e-352536257442");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a911eaf0-dddf-4d62-9c69-e9bd5bd058a8");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_FoodItemId",
                table: "Photo",
                newName: "IX_Photo_FoodItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "50807652-a42e-4c39-9e84-0c3dbc4b4734", "209fc85f-f28c-48e4-96a4-fad0b9745171", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9fbb191d-c8bc-4190-a0e2-390e40c8484a", "a656c752-eee3-48d3-8960-5243574ee347", "Customer", "CUSTOMER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_FoodItems_FoodItemId",
                table: "Photo",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id");
        }
    }
}
