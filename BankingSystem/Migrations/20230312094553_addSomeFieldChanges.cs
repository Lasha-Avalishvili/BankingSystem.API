using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations
{
    /// <inheritdoc />
    public partial class addSomeFieldChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
