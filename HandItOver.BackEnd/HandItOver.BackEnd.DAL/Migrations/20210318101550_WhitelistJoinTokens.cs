using Microsoft.EntityFrameworkCore.Migrations;

namespace HandItOver.BackEnd.DAL.Migrations
{
    public partial class WhitelistJoinTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhitelistJoinToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhitelistJoinToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhitelistJoinToken_MailboxGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MailboxGroup",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_WhitelistJoinToken_GroupId",
                table: "WhitelistJoinToken",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhitelistJoinToken");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2AEFE1C5-C5F0-4399-8FB8-420813567554",
                column: "ConcurrencyStamp",
                value: "32a238b3-6d19-436d-a1fd-39e6d7130dd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99DA7670-5471-414F-834E-9B3A6B6C8F6F",
                column: "ConcurrencyStamp",
                value: "01a978ba-34ee-4e7a-a4be-dc99434c33e6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CF87335D-19AB-4413-86C1-30BC80A741FA",
                column: "ConcurrencyStamp",
                value: "86e89632-c463-4d11-8554-790ee219c2ef");
        }
    }
}
