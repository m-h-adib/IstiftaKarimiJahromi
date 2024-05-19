using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreProccesses_QuestionCycles_QuestionCycleId",
                table: "PreProccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Researchs_QuestionCycles_QuestionCycleId",
                table: "Researchs");

            migrationBuilder.DropIndex(
                name: "IX_Researchs_QuestionCycleId",
                table: "Researchs");

            migrationBuilder.DropIndex(
                name: "IX_PreProccesses_QuestionCycleId",
                table: "PreProccesses");

            migrationBuilder.AlterColumn<long>(
                name: "QuestionCycleId",
                table: "Researchs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "QuestionCycleId",
                table: "PreProccesses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 5, 19, 18, 49, 2, 166, DateTimeKind.Local).AddTicks(2261), "AA4Gj19M97WPiNxXAGXL8rX1KMBwAPrwlv69+5aZar0J4p/Yg/DNXk4ukkWRWGVqvA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Researchs_QuestionCycleId",
                table: "Researchs",
                column: "QuestionCycleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreProccesses_QuestionCycleId",
                table: "PreProccesses",
                column: "QuestionCycleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PreProccesses_QuestionCycles_QuestionCycleId",
                table: "PreProccesses",
                column: "QuestionCycleId",
                principalTable: "QuestionCycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Researchs_QuestionCycles_QuestionCycleId",
                table: "Researchs",
                column: "QuestionCycleId",
                principalTable: "QuestionCycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreProccesses_QuestionCycles_QuestionCycleId",
                table: "PreProccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Researchs_QuestionCycles_QuestionCycleId",
                table: "Researchs");

            migrationBuilder.DropIndex(
                name: "IX_Researchs_QuestionCycleId",
                table: "Researchs");

            migrationBuilder.DropIndex(
                name: "IX_PreProccesses_QuestionCycleId",
                table: "PreProccesses");

            migrationBuilder.AlterColumn<long>(
                name: "QuestionCycleId",
                table: "Researchs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "QuestionCycleId",
                table: "PreProccesses",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 5, 19, 18, 24, 47, 183, DateTimeKind.Local).AddTicks(1404), "AGAoOfuIaQF0Ng2Q652vmTraShVCVQU5NQpO6sJfbyOGaZw6n+if20qLxRrYLbuCsQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Researchs_QuestionCycleId",
                table: "Researchs",
                column: "QuestionCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreProccesses_QuestionCycleId",
                table: "PreProccesses",
                column: "QuestionCycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreProccesses_QuestionCycles_QuestionCycleId",
                table: "PreProccesses",
                column: "QuestionCycleId",
                principalTable: "QuestionCycles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Researchs_QuestionCycles_QuestionCycleId",
                table: "Researchs",
                column: "QuestionCycleId",
                principalTable: "QuestionCycles",
                principalColumn: "Id");
        }
    }
}
