using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditDailyRestrictionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.AlterColumn<int>(
                name: "RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                schema: "finance",
                table: "DailyRestrictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions",
                column: "RestrictionTypeId",
                principalSchema: "finance",
                principalTable: "RestrictionTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.AlterColumn<int>(
                name: "RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions",
                column: "RestrictionTypeId",
                principalSchema: "finance",
                principalTable: "RestrictionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
