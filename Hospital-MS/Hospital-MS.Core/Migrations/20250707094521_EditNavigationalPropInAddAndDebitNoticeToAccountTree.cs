using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditNavigationalPropInAddAndDebitNoticeToAccountTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebitNotices_Accounts_AccountId",
                schema: "finance",
                table: "DebitNotices");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotices_AccountTrees_AccountId",
                schema: "finance",
                table: "DebitNotices",
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
                name: "FK_DebitNotices_AccountTrees_AccountId",
                schema: "finance",
                table: "DebitNotices");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotices_Accounts_AccountId",
                schema: "finance",
                table: "DebitNotices",
                column: "AccountId",
                principalSchema: "finance",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
