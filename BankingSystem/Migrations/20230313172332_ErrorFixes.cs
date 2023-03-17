using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations
{
    /// <inheritdoc />
    public partial class ErrorFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountEntityId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountEntityId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountEntityId",
                table: "Transactions");

            migrationBuilder.AddColumn<decimal>(
                name: "dailyLimitFromATM",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dailyLimitFromATM",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountEntityId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountEntityId",
                table: "Transactions",
                column: "AccountEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountEntityId",
                table: "Transactions",
                column: "AccountEntityId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
