using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddMatrialIssuePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialIssuePermissions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssuePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "finance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialIssueItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialIssuePermissionId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_MaterialIssuePermissions_MaterialIssuePermissionId",
                        column: x => x.MaterialIssuePermissionId,
                        principalSchema: "finance",
                        principalTable: "MaterialIssuePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_CreatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_ItemId",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_MaterialIssuePermissionId",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "MaterialIssuePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_UpdatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_StoreId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_UpdatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialIssueItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "MaterialIssuePermissions",
                schema: "finance");
        }
    }
}
