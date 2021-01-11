using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class CityUniver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CityId",
                table: "Universities",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.CreateIndex(
                name: "IX_Universities_CityId",
                table: "Universities",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Cities_CityId",
                table: "Universities",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Cities_CityId",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_CityId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Universities");
        }
    }
}
