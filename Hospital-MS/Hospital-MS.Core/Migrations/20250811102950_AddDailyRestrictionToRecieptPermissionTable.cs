using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyRestrictionToRecieptPermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "DailyRestrictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissions_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "DailyRestrictionId",
                principalSchema: "finance",
                principalTable: "DailyRestrictions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptPermissions_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptPermissions_DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions");

            migrationBuilder.DropColumn(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions");
        }
    }
}
