using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerFile2",
                table: "Researchs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonFile2",
                table: "Researchs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 9, 30, 214, DateTimeKind.Local).AddTicks(7536), "AIDHIWyfx8GpFBmEaXjM9Luc29oMIczA3dqZkcIKsKrqOGYTWoIFerSls73dTxpHJw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerFile2",
                table: "Researchs");

            migrationBuilder.DropColumn(
                name: "ReasonFile2",
                table: "Researchs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 11, 11, 14, 2, 49, 989, DateTimeKind.Local).AddTicks(5979), "ACb9pGqtoerp+7ECniIH2M9Yrdrc1Gm4acnPNXf1WP8pG+1LfMLHUCjgtRAwc0ugjw==" });
        }
    }
}
