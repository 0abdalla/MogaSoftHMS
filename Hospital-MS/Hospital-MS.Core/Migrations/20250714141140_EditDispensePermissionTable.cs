using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditDispensePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_Items_ItemId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_Stores_FromStoreId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_Stores_ToStoreId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_FromStoreId_ToStoreId_ItemId_Date",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_ItemId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_Status",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_ToStoreId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "FromStoreId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "ToStoreId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "finance",
                table: "DispensePermissions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DispenseTo",
                schema: "finance",
                table: "DispensePermissions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TreasuryId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_AccountId",
                schema: "finance",
                table: "DispensePermissions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_CostCenterId",
                schema: "finance",
                table: "DispensePermissions",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_TreasuryId",
                schema: "finance",
                table: "DispensePermissions",
                column: "TreasuryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_AccountTrees_AccountId",
                schema: "finance",
                table: "DispensePermissions",
                column: "AccountId",
                principalSchema: "Finance",
                principalTable: "AccountTrees",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_CostCenterTree_CostCenterId",
                schema: "finance",
                table: "DispensePermissions",
                column: "CostCenterId",
                principalSchema: "Finance",
                principalTable: "CostCenterTree",
                principalColumn: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_Treasuries_TreasuryId",
                schema: "finance",
                table: "DispensePermissions",
                column: "TreasuryId",
                principalSchema: "finance",
                principalTable: "Treasuries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_AccountTrees_AccountId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_CostCenterTree_CostCenterId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_Treasuries_TreasuryId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_AccountId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_CostCenterId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_TreasuryId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "DispenseTo",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "TreasuryId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "finance",
                table: "DispensePermissions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FromStoreId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                schema: "finance",
                table: "DispensePermissions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "finance",
                table: "DispensePermissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<int>(
                name: "ToStoreId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_FromStoreId_ToStoreId_ItemId_Date",
                schema: "finance",
                table: "DispensePermissions",
                columns: new[] { "FromStoreId", "ToStoreId", "ItemId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_ItemId",
                schema: "finance",
                table: "DispensePermissions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_Status",
                schema: "finance",
                table: "DispensePermissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_ToStoreId",
                schema: "finance",
                table: "DispensePermissions",
                column: "ToStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_Items_ItemId",
                schema: "finance",
                table: "DispensePermissions",
                column: "ItemId",
                principalSchema: "finance",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_Stores_FromStoreId",
                schema: "finance",
                table: "DispensePermissions",
                column: "FromStoreId",
                principalSchema: "finance",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_Stores_ToStoreId",
                schema: "finance",
                table: "DispensePermissions",
                column: "ToStoreId",
                principalSchema: "finance",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
