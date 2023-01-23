using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApi.FreelanceQueries;

#nullable disable

namespace WebApi.Migrations
{
    public partial class projectsearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescriptionTemplates");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11ffbcdd-43a0-4f76-abe8-e6b2ec7e9b0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ead8885b-4d9a-4bee-8230-c12b8d052ae2");

            migrationBuilder.CreateTable(
                name: "BidTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BidTemplates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectSearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Input = table.Column<ActiveProjectsInput>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSearches_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "36e50a2f-7f0c-4d85-bb7c-095a48d3468f", "4230eb1a-ee3a-48db-b27c-d8adf4cbfc6b", "Admin", "ADMIN" },
                    { "90e4ecb6-ae12-47de-9f3b-4d2d968d8d21", "5a2ee6d4-0762-4373-af2c-b9d5fade3e7a", "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BidTemplates_UserId",
                table: "BidTemplates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSearches_UserId",
                table: "ProjectSearches",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidTemplates");

            migrationBuilder.DropTable(
                name: "ProjectSearches");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36e50a2f-7f0c-4d85-bb7c-095a48d3468f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90e4ecb6-ae12-47de-9f3b-4d2d968d8d21");

            migrationBuilder.CreateTable(
                name: "DescriptionTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DescriptionTemplates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11ffbcdd-43a0-4f76-abe8-e6b2ec7e9b0d", "95507757-be32-4e0e-ac76-fd55f0f20822", "User", "USER" },
                    { "ead8885b-4d9a-4bee-8230-c12b8d052ae2", "a6e7b32e-afb7-413a-bbfe-a9cfd796707d", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionTemplates_UserId",
                table: "DescriptionTemplates",
                column: "UserId");
        }
    }
}
