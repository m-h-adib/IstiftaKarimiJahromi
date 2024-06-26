using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReasonText",
                table: "Researchs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 6, 26, 10, 28, 57, 68, DateTimeKind.Local).AddTicks(7442), "ADLZ39MPHhaw/Q6uzaRAqP5p20V4XFrNHlMbFh+Bcor7JtgX4dikI0kV++6ACgulBg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonText",
                table: "Researchs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 5, 19, 20, 2, 34, 115, DateTimeKind.Local).AddTicks(8845), "APyumCE2234ABB5YQjLXkz/FQKK3RbFqI2aDLQEmAFQfcJ5m+rHBiNdawXakEDAD0Q==" });
        }
    }
}
