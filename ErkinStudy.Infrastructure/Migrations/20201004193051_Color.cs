using Microsoft.EntityFrameworkCore.Migrations;

namespace ErkinStudy.Infrastructure.Migrations
{
    public partial class Color : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Quizzes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "OnlineCourses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Folders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FolderId",
                table: "Payments",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FolderId",
                table: "Orders",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOperations_OrderId",
                table: "OrderOperations",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderOperations_Orders_OrderId",
                table: "OrderOperations",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Folders_FolderId",
                table: "Orders",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Folders_FolderId",
                table: "Payments",
                column: "FolderId",
                principalTable: "Folders",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderOperations_Orders_OrderId",
                table: "OrderOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Folders_FolderId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Folders_FolderId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_FolderId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FolderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderOperations_OrderId",
                table: "OrderOperations");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "OnlineCourses");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Folders");
        }
    }
}
