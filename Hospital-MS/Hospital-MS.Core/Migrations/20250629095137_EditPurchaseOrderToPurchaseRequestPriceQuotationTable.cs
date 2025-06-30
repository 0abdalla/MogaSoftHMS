using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditPurchaseOrderToPurchaseRequestPriceQuotationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotations_PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations",
                column: "PurchaseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations",
                column: "PurchaseRequestId",
                principalSchema: "finance",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropIndex(
                name: "IX_PriceQuotations_PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations");
        }
    }
}
