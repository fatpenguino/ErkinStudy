using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class Payment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
