using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class OrderOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderOperations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    TraceTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOperations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderOperations");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Orders");
        }
    }
}
