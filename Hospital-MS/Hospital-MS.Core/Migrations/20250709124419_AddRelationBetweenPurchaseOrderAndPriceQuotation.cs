using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenPurchaseOrderAndPriceQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "finance",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "PriceQuotationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "PriceQuotationId",
                principalSchema: "finance",
                principalTable: "PriceQuotations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "finance",
                table: "PurchaseOrders");
        }
    }
}
