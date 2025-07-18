using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTreasuryMovementTable00 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TreasuryMovements",
                newName: "TreasuryMovements",
                newSchema: "finance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TreasuryMovements",
                schema: "finance",
                newName: "TreasuryMovements");
        }
    }
}
