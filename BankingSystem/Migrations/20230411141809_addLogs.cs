using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations
{
    /// <inheritdoc />
    public partial class addLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionRequiredTime = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisteredAt" },
                values: new object[] { "a2ad0cca-250d-461d-8e85-ba23e9ac53fd", "AQAAAAIAAYagAAAAEHOViNBEe29Vdix1w1yHNuE2nnZuyy9Dz0Z/qBGWpwnPM8R0SVyIvp8Dty8TBrtKvQ==", new DateTime(2023, 4, 11, 18, 18, 8, 871, DateTimeKind.Local).AddTicks(9918) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisteredAt" },
                values: new object[] { "1a1b8df5-597f-41aa-a280-0dcc68b21d14", "AQAAAAIAAYagAAAAEO3lbSPmgF/tDz3eUcMhIb1U8/jVdlS0w4VcJx9PHGMjx9Vc6m+IKJr5VVatg0aDPw==", new DateTime(2023, 4, 10, 21, 33, 32, 732, DateTimeKind.Local).AddTicks(1330) });
        }
    }
}
