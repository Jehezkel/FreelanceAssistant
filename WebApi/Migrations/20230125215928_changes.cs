using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSearches_AspNetUsers_UserId",
                table: "ProjectSearches");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36e50a2f-7f0c-4d85-bb7c-095a48d3468f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90e4ecb6-ae12-47de-9f3b-4d2d968d8d21");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectSearches",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3565a9f0-04cb-49d0-a3d7-721a54993eee", "65511062-3bf1-4967-a1c4-2055002848eb", "Admin", "ADMIN" },
                    { "6eb9983e-1b7d-4039-95c1-3f12716d4965", "9a799af4-aa2d-43f8-a30b-6ed21b910f6c", "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSearches_AspNetUsers_UserId",
                table: "ProjectSearches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSearches_AspNetUsers_UserId",
                table: "ProjectSearches");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3565a9f0-04cb-49d0-a3d7-721a54993eee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6eb9983e-1b7d-4039-95c1-3f12716d4965");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectSearches",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "36e50a2f-7f0c-4d85-bb7c-095a48d3468f", "4230eb1a-ee3a-48db-b27c-d8adf4cbfc6b", "Admin", "ADMIN" },
                    { "90e4ecb6-ae12-47de-9f3b-4d2d968d8d21", "5a2ee6d4-0762-4373-af2c-b9d5fade3e7a", "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSearches_AspNetUsers_UserId",
                table: "ProjectSearches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
