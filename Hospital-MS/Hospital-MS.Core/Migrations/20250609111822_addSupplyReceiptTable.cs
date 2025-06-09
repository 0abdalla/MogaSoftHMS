using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class addSupplyReceiptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplyReceipts",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "finance",
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_CostCenterId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_CreatedById",
                schema: "finance",
                table: "SupplyReceipts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_TreasuryId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_UpdatedById",
                schema: "finance",
                table: "SupplyReceipts",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplyReceipts",
                schema: "finance");
        }
    }
}
