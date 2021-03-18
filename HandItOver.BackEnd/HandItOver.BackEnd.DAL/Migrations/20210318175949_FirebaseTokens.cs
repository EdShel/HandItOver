using Microsoft.EntityFrameworkCore.Migrations;

namespace HandItOver.BackEnd.DAL.Migrations
{
    public partial class FirebaseTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirebaseToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirebaseToken", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_FirebaseToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2AEFE1C5-C5F0-4399-8FB8-420813567554",
                column: "ConcurrencyStamp",
                value: "69e01990-fa37-4b0e-9d46-f5534a748e20");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99DA7670-5471-414F-834E-9B3A6B6C8F6F",
                column: "ConcurrencyStamp",
                value: "0ea8df9a-405b-4778-b2c6-35953bb2348d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CF87335D-19AB-4413-86C1-30BC80A741FA",
                column: "ConcurrencyStamp",
                value: "08b2fb75-d541-4cd9-b3d2-3c3cd44b4795");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirebaseToken");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2AEFE1C5-C5F0-4399-8FB8-420813567554",
                column: "ConcurrencyStamp",
                value: "c6fc1dfd-dfcb-40e9-bbe1-420a4ad4ef55");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99DA7670-5471-414F-834E-9B3A6B6C8F6F",
                column: "ConcurrencyStamp",
                value: "208a9dbe-cb2b-4ccf-a08b-74f356a6a07a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CF87335D-19AB-4413-86C1-30BC80A741FA",
                column: "ConcurrencyStamp",
                value: "867d5be9-560b-449d-98c8-e930f93a37d6");
        }
    }
}
