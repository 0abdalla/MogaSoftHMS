using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditTotalPriceinPurchaseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "finance",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
