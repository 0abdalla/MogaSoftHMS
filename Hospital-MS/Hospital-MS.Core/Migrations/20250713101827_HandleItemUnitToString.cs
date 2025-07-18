using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class HandleItemUnitToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemUnits_UnitId",
                schema: "finance",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UnitId",
                schema: "finance",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "finance",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "finance",
                table: "Items",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "finance",
                table: "Items");


            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "finance",
                table: "Items",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UnitId",
                schema: "finance",
                table: "Items",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemUnits_UnitId",
                schema: "finance",
                table: "Items",
                column: "UnitId",
                principalSchema: "finance",
                principalTable: "ItemUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
