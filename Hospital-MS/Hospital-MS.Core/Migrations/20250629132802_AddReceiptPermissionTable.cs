using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiptPermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptPermissions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "finance",
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "finance",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptPermissionItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ReceiptPermissionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptPermissionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_ReceiptPermissions_ReceiptPermissionId",
                        column: x => x.ReceiptPermissionId,
                        principalSchema: "finance",
                        principalTable: "ReceiptPermissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_ItemId",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_ReceiptPermissionId",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "ReceiptPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_UpdatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_CreatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_PurchaseRequestId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_StoreId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_SupplierId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_UpdatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptPermissionItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "ReceiptPermissions",
                schema: "finance");
        }
    }
}
