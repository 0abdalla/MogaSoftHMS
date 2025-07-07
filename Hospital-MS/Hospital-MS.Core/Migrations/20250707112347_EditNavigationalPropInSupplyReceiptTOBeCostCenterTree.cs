using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditNavigationalPropInSupplyReceiptTOBeCostCenterTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyReceipts_CostCenters_CostCenterId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyReceipts_CostCenterTree_CostCenterId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "CostCenterId",
                principalSchema: "Finance",
                principalTable: "CostCenterTree",
                principalColumn: "CostCenterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyReceipts_CostCenterTree_CostCenterId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyReceipts_CostCenters_CostCenterId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "CostCenterId",
                principalSchema: "finance",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }
    }
}
