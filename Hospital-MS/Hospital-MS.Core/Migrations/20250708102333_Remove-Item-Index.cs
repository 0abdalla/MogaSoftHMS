using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveItemIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_NameAr_GroupId",
                schema: "finance",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_NameEn",
                schema: "finance",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Items_NameAr_GroupId",
                schema: "finance",
                table: "Items",
                columns: new[] { "NameAr", "GroupId" },
                unique: true,
                filter: "[GroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Items_NameEn",
                schema: "finance",
                table: "Items",
                column: "NameEn");
        }
    }
}
