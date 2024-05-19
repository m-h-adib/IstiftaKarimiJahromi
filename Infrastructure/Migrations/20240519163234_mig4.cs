using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DuplicateId",
                table: "PreProccesses",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 5, 19, 20, 2, 34, 115, DateTimeKind.Local).AddTicks(8845), "APyumCE2234ABB5YQjLXkz/FQKK3RbFqI2aDLQEmAFQfcJ5m+rHBiNdawXakEDAD0Q==" });

            migrationBuilder.CreateIndex(
                name: "IX_PreProccesses_DuplicateId",
                table: "PreProccesses",
                column: "DuplicateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreProccesses_Questions_DuplicateId",
                table: "PreProccesses",
                column: "DuplicateId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreProccesses_Questions_DuplicateId",
                table: "PreProccesses");

            migrationBuilder.DropIndex(
                name: "IX_PreProccesses_DuplicateId",
                table: "PreProccesses");

            migrationBuilder.DropColumn(
                name: "DuplicateId",
                table: "PreProccesses");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 5, 19, 18, 49, 2, 166, DateTimeKind.Local).AddTicks(2261), "AA4Gj19M97WPiNxXAGXL8rX1KMBwAPrwlv69+5aZar0J4p/Yg/DNXk4ukkWRWGVqvA==" });
        }
    }
}
