using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNavPropInDailyRestrictionToAccountTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_Accounts_AccountId",
                schema: "finance",
                table: "DailyRestrictionDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "AccountId",
                principalSchema: "Finance",
                principalTable: "AccountTrees",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                schema: "finance",
                table: "DailyRestrictionDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictionDetails_Accounts_AccountId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "AccountId",
                principalSchema: "finance",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
