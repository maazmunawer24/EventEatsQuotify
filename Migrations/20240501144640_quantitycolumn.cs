using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class quantitycolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "174cb68d-1a49-4965-bb41-be3ca93efb79");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5b312db-e422-479f-a1fe-06e6ac6f9687");

            migrationBuilder.AddColumn<int>(
                name: "QuantityOrPersons",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QuantityType",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5f0d4f78-cdf9-45aa-a73c-b0e9733838f7", "2a12df01-c868-4d2b-87d6-74d8656e647b", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a57d40f1-30f1-4703-b6e4-dd3e7681d6c7", "1fc820cb-0d05-434e-ab10-0c931742f448", "Vendor", "VENDOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f0d4f78-cdf9-45aa-a73c-b0e9733838f7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a57d40f1-30f1-4703-b6e4-dd3e7681d6c7");

            migrationBuilder.DropColumn(
                name: "QuantityOrPersons",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "QuantityType",
                table: "FoodItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "174cb68d-1a49-4965-bb41-be3ca93efb79", "f0cec4ba-402a-41a6-b069-77ee0b268d68", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5b312db-e422-479f-a1fe-06e6ac6f9687", "67d52a43-5e1f-4f2e-a987-728ef52ae26f", "Vendor", "VENDOR" });
        }
    }
}
