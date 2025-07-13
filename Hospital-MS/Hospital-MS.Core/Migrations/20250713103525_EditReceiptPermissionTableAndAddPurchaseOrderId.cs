using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditReceiptPermissionTableAndAddPurchaseOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptPermissions_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "ReceiptPermissions");

            migrationBuilder.RenameColumn(
                name: "PurchaseRequestId",
                schema: "finance",
                table: "ReceiptPermissions",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptPermissions_PurchaseRequestId",
                schema: "finance",
                table: "ReceiptPermissions",
                newName: "IX_ReceiptPermissions_PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissions_PurchaseOrders_PurchaseOrderId",
                schema: "finance",
                table: "ReceiptPermissions",
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
                name: "FK_ReceiptPermissions_PurchaseOrders_PurchaseOrderId",
                schema: "finance",
                table: "ReceiptPermissions");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                schema: "finance",
                table: "ReceiptPermissions",
                newName: "PurchaseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptPermissions_PurchaseOrderId",
                schema: "finance",
                table: "ReceiptPermissions",
                newName: "IX_ReceiptPermissions_PurchaseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissions_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "PurchaseRequestId",
                principalSchema: "finance",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
