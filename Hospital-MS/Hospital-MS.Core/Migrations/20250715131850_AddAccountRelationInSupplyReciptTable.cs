using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountRelationInSupplyReciptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCode",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                schema: "finance",
                table: "SupplyReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_AccountId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyReceipts_AccountTrees_AccountId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "AccountId",
                principalSchema: "Finance",
                principalTable: "AccountTrees",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyReceipts_AccountTrees_AccountId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.DropIndex(
                name: "IX_SupplyReceipts_AccountId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                schema: "finance",
                table: "SupplyReceipts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
