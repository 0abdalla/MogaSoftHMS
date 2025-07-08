using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddaccountingguidanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LedgerNumber",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.AddColumn<int>(
                name: "AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountingGuidance",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingGuidance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingGuidance_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountingGuidance_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions",
                column: "AccountingGuidanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingGuidance_CreatedById",
                schema: "finance",
                table: "AccountingGuidance",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingGuidance_UpdatedById",
                schema: "finance",
                table: "AccountingGuidance",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRestrictions_AccountingGuidance_AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions",
                column: "AccountingGuidanceId",
                principalSchema: "finance",
                principalTable: "AccountingGuidance",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRestrictions_AccountingGuidance_AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.DropTable(
                name: "AccountingGuidance",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "IX_DailyRestrictions_AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.DropColumn(
                name: "AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions");

            migrationBuilder.AddColumn<string>(
                name: "LedgerNumber",
                schema: "finance",
                table: "DailyRestrictions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
