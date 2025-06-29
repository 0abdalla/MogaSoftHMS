using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPurchaseOrderAndPurchaseOrderItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_CostCenters_CostCenterId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Stores_StoreId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_Date",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_OrderNumber",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "OrderDate");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                schema: "finance",
                table: "PurchaseOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CostCenterId",
                schema: "finance",
                table: "PurchaseOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                schema: "finance",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "finance",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RequestedQuantity",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_CostCenters_CostCenterId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "CostCenterId",
                principalSchema: "finance",
                principalTable: "CostCenters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Stores_StoreId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "StoreId",
                principalSchema: "finance",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_CostCenters_CostCenterId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Stores_StoreId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RequestedQuantity",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "Notes");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                schema: "finance",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CostCenterId",
                schema: "finance",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "finance",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                schema: "finance",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitCost",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Date",
                schema: "finance",
                table: "PurchaseOrders",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderNumber",
                schema: "finance",
                table: "PurchaseOrders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_CostCenters_CostCenterId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "CostCenterId",
                principalSchema: "finance",
                principalTable: "CostCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Stores_StoreId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "StoreId",
                principalSchema: "finance",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
