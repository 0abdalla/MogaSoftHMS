using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyRestrictionRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyRestrictionId",
                table: "AdditionNotices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_DailyRestrictionId",
                table: "AdditionNotices",
                column: "DailyRestrictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionNotices_DailyRestrictions_DailyRestrictionId",
                table: "AdditionNotices",
                column: "DailyRestrictionId",
                principalSchema: "finance",
                principalTable: "DailyRestrictions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotices_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices",
                column: "DailyRestrictionId",
                principalSchema: "finance",
                principalTable: "DailyRestrictions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions",
                column: "DailyRestrictionId",
                principalSchema: "finance",
                principalTable: "DailyRestrictions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyReceipts_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "DailyRestrictionId",
                principalSchema: "finance",
                principalTable: "DailyRestrictions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionNotices_DailyRestrictions_DailyRestrictionId",
                table: "AdditionNotices");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNotices_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyReceipts_DailyRestrictions_DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.DropIndex(
                name: "IX_SupplyReceipts_DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.DropIndex(
                name: "IX_DispensePermissions_DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DebitNotices_DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices");

            migrationBuilder.DropIndex(
                name: "IX_AdditionNotices_DailyRestrictionId",
                table: "AdditionNotices");

            migrationBuilder.DropColumn(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts");

            migrationBuilder.DropColumn(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropColumn(
                name: "DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices");

            migrationBuilder.DropColumn(
                name: "DailyRestrictionId",
                table: "AdditionNotices");
        }
    }
}
