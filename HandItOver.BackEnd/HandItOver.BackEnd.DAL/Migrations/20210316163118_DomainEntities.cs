using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HandItOver.BackEnd.DAL.Migrations
{
    public partial class DomainEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MailboxGroup",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhitelistOnly = table.Column<bool>(type: "bit", nullable: false),
                    MaxRentTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailboxGroup", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_MailboxGroup_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppUserMailboxGroup",
                columns: table => new
                {
                    WhitelistedId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WhitelistedInGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserMailboxGroup", x => new { x.WhitelistedId, x.WhitelistedInGroupId });
                    table.ForeignKey(
                        name: "FK_AppUserMailboxGroup_AspNetUsers_WhitelistedId",
                        column: x => x.WhitelistedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserMailboxGroup_MailboxGroup_WhitelistedInGroupId",
                        column: x => x.WhitelistedInGroupId,
                        principalTable: "MailboxGroup",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mailbox",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhysicalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mailbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mailbox_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mailbox_MailboxGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MailboxGroup",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    AddresseeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MailboxId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Arrived = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Taken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PredictedTakingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminalTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_AspNetUsers_AddresseeId",
                        column: x => x.AddresseeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Delivery_Mailbox_MailboxId",
                        column: x => x.MailboxId,
                        principalTable: "Mailbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MailboxRent",
                columns: table => new
                {
                    RentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MailboxId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RenterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Until = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailboxRent", x => x.RentId);
                    table.ForeignKey(
                        name: "FK_MailboxRent_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MailboxRent_Mailbox_MailboxId",
                        column: x => x.MailboxId,
                        principalTable: "Mailbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2AEFE1C5-C5F0-4399-8FB8-420813567554",
                column: "ConcurrencyStamp",
                value: "744d1b5e-6370-4876-a859-8f1a30904a63");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99DA7670-5471-414F-834E-9B3A6B6C8F6F",
                column: "ConcurrencyStamp",
                value: "d11f263a-501b-487b-8a9e-574e4306224b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "CF87335D-19AB-4413-86C1-30BC80A741FA", "0f7e27c6-78ff-4528-bffa-7fecb230569d", "mailbox", "MAILBOX" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMailboxGroup_WhitelistedInGroupId",
                table: "AppUserMailboxGroup",
                column: "WhitelistedInGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_AddresseeId",
                table: "Delivery",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_MailboxId",
                table: "Delivery",
                column: "MailboxId");

            migrationBuilder.CreateIndex(
                name: "IX_Mailbox_GroupId",
                table: "Mailbox",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Mailbox_OwnerId",
                table: "Mailbox",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MailboxGroup_OwnerId",
                table: "MailboxGroup",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MailboxRent_MailboxId",
                table: "MailboxRent",
                column: "MailboxId");

            migrationBuilder.CreateIndex(
                name: "IX_MailboxRent_RenterId",
                table: "MailboxRent",
                column: "RenterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserMailboxGroup");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "MailboxRent");

            migrationBuilder.DropTable(
                name: "Mailbox");

            migrationBuilder.DropTable(
                name: "MailboxGroup");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CF87335D-19AB-4413-86C1-30BC80A741FA");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2AEFE1C5-C5F0-4399-8FB8-420813567554",
                column: "ConcurrencyStamp",
                value: "1965e182-f518-4758-be9c-62546b6e6a93");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99DA7670-5471-414F-834E-9B3A6B6C8F6F",
                column: "ConcurrencyStamp",
                value: "f81f7757-efa1-4e16-90bd-02b53962faf4");
        }
    }
}
