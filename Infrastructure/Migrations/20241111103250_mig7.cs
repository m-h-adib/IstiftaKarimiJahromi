using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EitaaId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "EitaaId", "Password" },
                values: new object[] { new DateTime(2024, 11, 11, 14, 2, 49, 989, DateTimeKind.Local).AddTicks(5979), null, "ACb9pGqtoerp+7ECniIH2M9Yrdrc1Gm4acnPNXf1WP8pG+1LfMLHUCjgtRAwc0ugjw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EitaaId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 21, 23, 12, 36, 917, DateTimeKind.Local).AddTicks(9673), "AImDve37HUGPb9nfvXfcIgmdcgC9hBmFkgNgtgL8bWpw19OMkulnq2H9KltALKpIww==" });
        }
    }
}
