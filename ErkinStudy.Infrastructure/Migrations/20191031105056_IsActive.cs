using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class IsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserLessons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Payments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Paragraphs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Lessons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Degrees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Paragraphs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Degrees");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Contents");
        }
    }
}
