using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class roles2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46f010aa-a550-4cd1-81f4-2461a8ade2e7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11ffbcdd-43a0-4f76-abe8-e6b2ec7e9b0d", "95507757-be32-4e0e-ac76-fd55f0f20822", "User", "USER" },
                    { "ead8885b-4d9a-4bee-8230-c12b8d052ae2", "a6e7b32e-afb7-413a-bbfe-a9cfd796707d", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11ffbcdd-43a0-4f76-abe8-e6b2ec7e9b0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ead8885b-4d9a-4bee-8230-c12b8d052ae2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "46f010aa-a550-4cd1-81f4-2461a8ade2e7", "641f0cd5-a32d-4c63-a53a-cb3d143656a2", "Admin", "ADMIN" });
        }
    }
}
