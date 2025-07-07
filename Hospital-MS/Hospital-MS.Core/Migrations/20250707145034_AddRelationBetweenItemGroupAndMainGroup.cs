using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenItemGroupAndMainGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemGroups_NameAr",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.DropIndex(
                name: "IX_ItemGroups_NameEn",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                schema: "finance",
                table: "ItemGroups",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "MainGroupId",
                schema: "finance",
                table: "ItemGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_MainGroupId",
                schema: "finance",
                table: "ItemGroups",
                column: "MainGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGroups_MainGroups_MainGroupId",
                schema: "finance",
                table: "ItemGroups",
                column: "MainGroupId",
                principalSchema: "finance",
                principalTable: "MainGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemGroups_MainGroups_MainGroupId",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.DropIndex(
                name: "IX_ItemGroups_MainGroupId",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "MainGroupId",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "finance",
                table: "ItemGroups",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "finance",
                table: "ItemGroups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_NameAr",
                schema: "finance",
                table: "ItemGroups",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_NameEn",
                schema: "finance",
                table: "ItemGroups",
                column: "NameEn");
        }
    }
}
