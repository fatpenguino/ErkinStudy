using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class FolderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FolderId",
                table: "Quizzes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FolderId",
                table: "OnlineCourses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "OnlineCourses");
        }
    }
}
