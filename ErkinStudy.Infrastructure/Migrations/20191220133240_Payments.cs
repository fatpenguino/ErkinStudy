using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class Payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Lessons_LessonId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_LessonId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Payments");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedTime",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "Payments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Payments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserOnlineCourses",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    OnlineCourseId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ApproverId",
                table: "Payments",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOnlineCourses_UserId",
                table: "UserOnlineCourses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_ApproverId",
                table: "Payments",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_ApproverId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "UserOnlineCourses");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ApproverId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApprovedTime",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "LessonId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_LessonId",
                table: "Payments",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Lessons_LessonId",
                table: "Payments",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
