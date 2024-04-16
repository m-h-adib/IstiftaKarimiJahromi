using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "IsActive", "IsRemoved", "Name", "Password", "RemovedAt", "Role", "UpdatedAt", "Username" },
                values: new object[] { 1L, new DateTime(2024, 4, 16, 11, 25, 26, 708, DateTimeKind.Local).AddTicks(2633), true, false, "محمدحسین ادیب", "AJDT6ooMoJVHDjzF204I3NBaoMqx3t4yFKTo6dYKvU9syHnqEnBe28+Uqqs0W6Y3hA==", null, 2, null, "m-h-adib" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
