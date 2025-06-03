using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDispensePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispensePermissions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    FromStoreId = table.Column<int>(type: "int", nullable: false),
                    ToStoreId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispensePermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensePermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispensePermissions_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensePermissions_Stores_FromStoreId",
                        column: x => x.FromStoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensePermissions_Stores_ToStoreId",
                        column: x => x.ToStoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_CreatedById",
                schema: "finance",
                table: "DispensePermissions",
                column: "CreatedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_UpdatedById",
                schema: "finance",
                table: "DispensePermissions",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispensePermissions",
                schema: "finance");
        }
    }
}
