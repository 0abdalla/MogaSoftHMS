using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddOopendInAndClosedInInTreasuryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosedIn",
                schema: "finance",
                table: "Treasuries",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "OpenedIn",
                schema: "finance",
                table: "Treasuries",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedIn",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropColumn(
                name: "OpenedIn",
                schema: "finance",
                table: "Treasuries");
        }
    }
}
