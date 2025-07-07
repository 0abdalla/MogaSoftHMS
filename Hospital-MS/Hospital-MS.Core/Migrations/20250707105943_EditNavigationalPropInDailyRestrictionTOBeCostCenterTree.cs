using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditNavigationalPropInDailyRestrictionTOBeCostCenterTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenters_CostCenterId",
                schema: "finance",
                table: "DailyRestrictionDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenterTree_CostCenterId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "CostCenterId",
                principalSchema: "Finance",
                principalTable: "CostCenterTree",
                principalColumn: "CostCenterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenterTree_CostCenterId",
                schema: "finance",
                table: "DailyRestrictionDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_CostCenters_CostCenterId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "CostCenterId",
                principalSchema: "finance",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }
    }
}
