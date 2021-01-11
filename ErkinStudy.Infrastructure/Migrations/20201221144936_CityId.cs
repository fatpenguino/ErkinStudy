using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class CityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Cities");

            migrationBuilder.AddColumn<short>(
                name: "Id",
                table: "Cities",
                nullable: false,
                defaultValue: (short)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Cities");

            migrationBuilder.AddColumn<short>(
                name: "CityId",
                table: "Cities",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "CityId");
        }
    }
}
