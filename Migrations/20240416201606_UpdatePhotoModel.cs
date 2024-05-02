using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEatsQuotify.Migrations
{
    public partial class UpdatePhotoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31d71bc8-b0ab-41c9-bae8-1ee4969800af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a16c9e3d-0766-4949-80f9-76af7f2b4dc0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b1e9cc6d-dab3-4792-bb4d-c1de2930e326", "c51a08f3-ce40-4649-8cd5-3f847ee5a25f", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bf269dfa-aaff-495c-901c-8363e6e3e1c8", "20a167df-76f7-46fe-a1d4-98af6153ee2a", "Vendor", "VENDOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "31d71bc8-b0ab-41c9-bae8-1ee4969800af", "5cb26ecb-2aea-4971-bb6a-db6fe51d0365", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a16c9e3d-0766-4949-80f9-76af7f2b4dc0", "b898e786-6377-4149-8a34-fc842863c597", "Vendor", "VENDOR" });
        }
    }
}
