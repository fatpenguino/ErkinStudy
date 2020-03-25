using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class UserFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLessons");

            migrationBuilder.DropTable(
                name: "UserOnlineCourses");

            migrationBuilder.DropTable(
                name: "UserQuizzes");

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Folders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "UserFolders",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    FolderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFolders", x => new { x.FolderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFolders_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFolders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFolders_UserId",
                table: "UserFolders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFolders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Folders");

            migrationBuilder.CreateTable(
                name: "UserLessons",
                columns: table => new
                {
                    LessonId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLessons", x => new { x.LessonId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLessons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOnlineCourses",
                columns: table => new
                {
                    OnlineCourseId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOnlineCourses", x => new { x.OnlineCourseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserOnlineCourses_OnlineCourses_OnlineCourseId",
                        column: x => x.OnlineCourseId,
                        principalTable: "OnlineCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOnlineCourses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizzes",
                columns: table => new
                {
                    QuizId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizzes", x => new { x.QuizId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserQuizzes_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuizzes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_UserId",
                table: "UserLessons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOnlineCourses_UserId",
                table: "UserOnlineCourses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizzes_UserId",
                table: "UserQuizzes",
                column: "UserId");
        }
    }
}
