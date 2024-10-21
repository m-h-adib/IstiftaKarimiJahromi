using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 21, 23, 12, 36, 917, DateTimeKind.Local).AddTicks(9673), "AImDve37HUGPb9nfvXfcIgmdcgC9hBmFkgNgtgL8bWpw19OMkulnq2H9KltALKpIww==" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCycles_FromUserId",
                table: "QuestionCycles",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCycles_ToUserId",
                table: "QuestionCycles",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCycles_Users_FromUserId",
                table: "QuestionCycles",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCycles_Users_ToUserId",
                table: "QuestionCycles",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCycles_Users_FromUserId",
                table: "QuestionCycles");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCycles_Users_ToUserId",
                table: "QuestionCycles");

            migrationBuilder.DropIndex(
                name: "IX_QuestionCycles_FromUserId",
                table: "QuestionCycles");

            migrationBuilder.DropIndex(
                name: "IX_QuestionCycles_ToUserId",
                table: "QuestionCycles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sessions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 6, 26, 10, 28, 57, 68, DateTimeKind.Local).AddTicks(7442), "ADLZ39MPHhaw/Q6uzaRAqP5p20V4XFrNHlMbFh+Bcor7JtgX4dikI0kV++6ACgulBg==" });
        }
    }
}
