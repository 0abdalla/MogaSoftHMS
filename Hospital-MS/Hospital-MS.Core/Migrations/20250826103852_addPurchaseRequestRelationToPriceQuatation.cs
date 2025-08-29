using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class addPurchaseRequestRelationToPriceQuatation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "config",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "config",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "config",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "config",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Route",
                schema: "config",
                table: "Pages");

            migrationBuilder.RenameColumn(
                name: "PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "PurchaseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_PurchaseRequestId");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "AspNetUsers",
                newName: "BranchId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningBalance",
                schema: "finance",
                table: "Treasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorized",
                table: "Staff",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Departments",
                type: "nvarchar(755)",
                maxLength: 755,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(755)",
                oldMaxLength: 755);

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                schema: "Finance",
                table: "CostCenterTree",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyPrice",
                table: "Beds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClosedBy = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShiftMedicalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: true),
                    MedicalServiceName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftMedicalServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftMedicalServices_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests",
                column: "PriceQuotationId",
                unique: true,
                filter: "[PriceQuotationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftMedicalServices_ShiftId",
                table: "ShiftMedicalServices",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "PurchaseRequestId",
                principalSchema: "finance",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests",
                column: "PriceQuotationId",
                principalSchema: "finance",
                principalTable: "PriceQuotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "ShiftMedicalServices");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropColumn(
                name: "IsAuthorized",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "IsGroup",
                schema: "Finance",
                table: "CostCenterTree");

            migrationBuilder.DropColumn(
                name: "DailyPrice",
                table: "Beds");

            migrationBuilder.RenameColumn(
                name: "PurchaseRequestId",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "PriceQuotationId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_PurchaseRequestId",
                schema: "finance",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_PriceQuotationId");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "AspNetUsers",
                newName: "StaffId");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Wards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Wards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "config",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "config",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "config",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "config",
                table: "Pages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Route",
                schema: "config",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Departments",
                type: "nvarchar(755)",
                maxLength: 755,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(755)",
                oldMaxLength: 755,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "PriceQuotationId",
                principalSchema: "finance",
                principalTable: "PriceQuotations",
                principalColumn: "Id");
        }
    }
}
