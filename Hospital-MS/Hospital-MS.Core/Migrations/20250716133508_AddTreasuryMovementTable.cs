using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTreasuryMovementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreasuryTransactions",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "IX_Treasuries_AccountCode",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropIndex(
                name: "IX_Treasuries_Currency",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropIndex(
                name: "IX_Treasuries_Name_BranchId",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropColumn(
                name: "ClosedIn",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropColumn(
                name: "OpenedIn",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.RenameColumn(
                name: "AccountCode",
                schema: "finance",
                table: "Treasuries",
                newName: "Code");

            migrationBuilder.CreateTable(
                name: "TreasuryOperations",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasuryOperations_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryOperations_TreasuryId",
                schema: "finance",
                table: "TreasuryOperations",
                column: "TreasuryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreasuryOperations",
                schema: "finance");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "finance",
                table: "Treasuries",
                newName: "AccountCode");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosedIn",
                schema: "finance",
                table: "Treasuries",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                schema: "finance",
                table: "Treasuries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "OpenedIn",
                schema: "finance",
                table: "Treasuries",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningBalance",
                schema: "finance",
                table: "Treasuries",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TreasuryTransactions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasuryTransactions_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treasuries_AccountCode",
                schema: "finance",
                table: "Treasuries",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Treasuries_Currency",
                schema: "finance",
                table: "Treasuries",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_Treasuries_Name_BranchId",
                schema: "finance",
                table: "Treasuries",
                columns: new[] { "Name", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryTransactions_TreasuryId",
                schema: "finance",
                table: "TreasuryTransactions",
                column: "TreasuryId");
        }
    }
}
