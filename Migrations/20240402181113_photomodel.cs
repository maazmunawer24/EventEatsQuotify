using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class photomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "975354b4-d8c0-49a5-8c30-4e75dfb0065f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fe3828d-2151-43e1-8179-8ac42db32253");

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "50807652-a42e-4c39-9e84-0c3dbc4b4734", "209fc85f-f28c-48e4-96a4-fad0b9745171", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9fbb191d-c8bc-4190-a0e2-390e40c8484a", "a656c752-eee3-48d3-8960-5243574ee347", "Customer", "CUSTOMER" });

            migrationBuilder.CreateIndex(
                name: "IX_Photo_FoodItemId",
                table: "Photo",
                column: "FoodItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50807652-a42e-4c39-9e84-0c3dbc4b4734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fbb191d-c8bc-4190-a0e2-390e40c8484a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "975354b4-d8c0-49a5-8c30-4e75dfb0065f", "4bc79bf3-452b-4079-872a-f2d2896cfb23", "Vendor", "VENDOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9fe3828d-2151-43e1-8179-8ac42db32253", "4549d532-518c-4bcd-bf1b-18a1bd158877", "Customer", "CUSTOMER" });
        }
    }
}
