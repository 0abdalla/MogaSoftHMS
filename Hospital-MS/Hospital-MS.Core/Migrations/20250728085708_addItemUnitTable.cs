using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class addItemUnitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemUnits_NameAr",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropIndex(
                name: "IX_ItemUnits_NameEn",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "NameEn",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "finance",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "finance",
                table: "ItemUnits",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "finance",
                table: "Items",
                type: "int",
                nullable: true);

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Name",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "finance",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "finance",
                table: "ItemUnits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                schema: "finance",
                table: "ItemUnits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "finance",
                table: "Items",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_NameAr",
                schema: "finance",
                table: "ItemUnits",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_NameEn",
                schema: "finance",
                table: "ItemUnits",
                column: "NameEn");
        }
    }
}
