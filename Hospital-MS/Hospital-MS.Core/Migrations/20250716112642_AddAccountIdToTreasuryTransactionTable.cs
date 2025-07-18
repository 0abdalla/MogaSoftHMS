using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToTreasuryTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                schema: "finance",
                table: "TreasuryTransactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "finance",
                table: "TreasuryTransactions");
        }
    }
}
