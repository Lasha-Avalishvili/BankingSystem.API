using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations
{
    /// <inheritdoc />
    public partial class addLogger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisteredAt" },
                values: new object[] { "57edf944-9fa8-42c3-9f6d-e5408d3e7466", "AQAAAAIAAYagAAAAEPOoGQqa390qMo1RrvvUW1h2/AHj+eYJZau5pvq9eT5DCXJ+RdlEB7rfPTD1fZPqmQ==", new DateTime(2023, 4, 11, 19, 13, 11, 119, DateTimeKind.Local).AddTicks(6267) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisteredAt" },
                values: new object[] { "a2ad0cca-250d-461d-8e85-ba23e9ac53fd", "AQAAAAIAAYagAAAAEHOViNBEe29Vdix1w1yHNuE2nnZuyy9Dz0Z/qBGWpwnPM8R0SVyIvp8Dty8TBrtKvQ==", new DateTime(2023, 4, 11, 18, 18, 8, 871, DateTimeKind.Local).AddTicks(9918) });
        }
    }
}
