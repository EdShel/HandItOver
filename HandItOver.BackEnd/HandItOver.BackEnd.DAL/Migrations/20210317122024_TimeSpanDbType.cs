using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HandItOver.BackEnd.DAL.Migrations
{
    public partial class TimeSpanDbType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxRentTime",
                table: "MailboxGroup"
            );

            migrationBuilder.AddColumn<long>(
                name: "MaxRentTime",
                table: "MailboxGroup",
                type: "bigint",
                nullable: true
            );

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxRentTime",
                table: "MailboxGroup"
            );

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxRentTime",
                table: "MailboxGroup",
                type: "time",
                nullable: true
            );

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

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CF87335D-19AB-4413-86C1-30BC80A741FA",
                column: "ConcurrencyStamp",
                value: "0f7e27c6-78ff-4528-bffa-7fecb230569d");
        }
    }
}
