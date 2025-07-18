using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTrasuryTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreasuryTransactions",
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
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_TreasuryTransactions_TreasuryId",
                schema: "finance",
                table: "TreasuryTransactions",
                column: "TreasuryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreasuryTransactions",
                schema: "finance");
        }
    }
}
