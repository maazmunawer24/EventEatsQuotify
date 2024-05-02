using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class Updatefooditemmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1e9cc6d-dab3-4792-bb4d-c1de2930e326");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf269dfa-aaff-495c-901c-8363e6e3e1c8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "174cb68d-1a49-4965-bb41-be3ca93efb79", "f0cec4ba-402a-41a6-b069-77ee0b268d68", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5b312db-e422-479f-a1fe-06e6ac6f9687", "67d52a43-5e1f-4f2e-a987-728ef52ae26f", "Vendor", "VENDOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "174cb68d-1a49-4965-bb41-be3ca93efb79");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5b312db-e422-479f-a1fe-06e6ac6f9687");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b1e9cc6d-dab3-4792-bb4d-c1de2930e326", "c51a08f3-ce40-4649-8cd5-3f847ee5a25f", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bf269dfa-aaff-495c-901c-8363e6e3e1c8", "20a167df-76f7-46fe-a1d4-98af6153ee2a", "Vendor", "VENDOR" });
        }
    }
}
