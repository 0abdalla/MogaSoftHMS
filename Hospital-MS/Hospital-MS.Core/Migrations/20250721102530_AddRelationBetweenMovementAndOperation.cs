using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenMovementAndOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryOperations_TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations",
                column: "TreasuryMovementId");


            migrationBuilder.AddForeignKey(
                name: "FK_TreasuryOperations_TreasuryMovements_TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations",
                column: "TreasuryMovementId",
                principalSchema: "finance",
                principalTable: "TreasuryMovements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropForeignKey(
                name: "FK_TreasuryOperations_TreasuryMovements_TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations");

            migrationBuilder.DropIndex(
                name: "IX_TreasuryOperations_TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations");


            migrationBuilder.DropColumn(
                name: "TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations");

        }
    }
}
