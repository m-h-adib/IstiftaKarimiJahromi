using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proccesses");

            migrationBuilder.AddColumn<long>(
                name: "DuplicateId",
                table: "Questions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QuestionId",
                table: "QuestionCycles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Editors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccept",
                table: "Editors",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PreProccesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionCycleId = table.Column<long>(type: "bigint", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreProccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreProccesses_QuestionCycles_QuestionCycleId",
                        column: x => x.QuestionCycleId,
                        principalTable: "QuestionCycles",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 5, 19, 18, 24, 47, 183, DateTimeKind.Local).AddTicks(1404), "AGAoOfuIaQF0Ng2Q652vmTraShVCVQU5NQpO6sJfbyOGaZw6n+if20qLxRrYLbuCsQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DuplicateId",
                table: "Questions",
                column: "DuplicateId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCycles_QuestionId",
                table: "QuestionCycles",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreProccesses_QuestionCycleId",
                table: "PreProccesses",
                column: "QuestionCycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionCycles_Questions_QuestionId",
                table: "QuestionCycles",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_DuplicateId",
                table: "Questions",
                column: "DuplicateId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionCycles_Questions_QuestionId",
                table: "QuestionCycles");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_DuplicateId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "PreProccesses");

            migrationBuilder.DropIndex(
                name: "IX_Questions_DuplicateId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionCycles_QuestionId",
                table: "QuestionCycles");

            migrationBuilder.DropColumn(
                name: "DuplicateId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "QuestionCycles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "IsAccept",
                table: "Editors");

            migrationBuilder.CreateTable(
                name: "Proccesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuplicateId = table.Column<long>(type: "bigint", nullable: true),
                    QuestionCycleId = table.Column<long>(type: "bigint", nullable: true),
                    ResearcherId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proccesses_QuestionCycles_QuestionCycleId",
                        column: x => x.QuestionCycleId,
                        principalTable: "QuestionCycles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proccesses_Questions_DuplicateId",
                        column: x => x.DuplicateId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proccesses_Users_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 4, 16, 11, 25, 26, 708, DateTimeKind.Local).AddTicks(2633), "AJDT6ooMoJVHDjzF204I3NBaoMqx3t4yFKTo6dYKvU9syHnqEnBe28+Uqqs0W6Y3hA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Proccesses_DuplicateId",
                table: "Proccesses",
                column: "DuplicateId");

            migrationBuilder.CreateIndex(
                name: "IX_Proccesses_QuestionCycleId",
                table: "Proccesses",
                column: "QuestionCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Proccesses_ResearcherId",
                table: "Proccesses",
                column: "ResearcherId");
        }
    }
}
