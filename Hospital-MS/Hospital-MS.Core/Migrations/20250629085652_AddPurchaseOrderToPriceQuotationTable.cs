using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderToPriceQuotationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                schema: "finance",
                table: "PriceQuotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_PurchaseOrders_PurchaseOrderId",
                schema: "finance",
                table: "PriceQuotations",
                column: "PurchaseOrderId",
                principalSchema: "finance",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_PurchaseOrders_PurchaseOrderId",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropIndex(
                name: "IX_PriceQuotations_PurchaseOrderId",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "finance",
                table: "PriceQuotations");
        }
    }
}
