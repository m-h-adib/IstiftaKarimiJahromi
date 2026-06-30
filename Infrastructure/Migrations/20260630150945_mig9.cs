using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendAnswerDate",
                table: "Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2026, 6, 30, 18, 39, 45, 692, DateTimeKind.Local).AddTicks(6280), "ADH35/RmfEpeJqmv+U6aTPOB/zD76uIzKb22BFMf+JJzcar2OzgirYhGuxSVHjWP+g==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendAnswerDate",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 9, 30, 214, DateTimeKind.Local).AddTicks(7536), "AIDHIWyfx8GpFBmEaXjM9Luc29oMIczA3dqZkcIKsKrqOGYTWoIFerSls73dTxpHJw==" });
        }
    }
}
