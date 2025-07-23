using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnsInStaffTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Allowances",
                table: "Staff",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);


            migrationBuilder.AddColumn<decimal>(
                name: "Rewards",
                table: "Staff",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VariableSalary",
                table: "Staff",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "VisaCode",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allowances",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Rewards",
                table: "Staff");


            migrationBuilder.DropColumn(
                name: "VacationDays",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "VariableSalary",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "VisaCode",
                table: "Staff");
        }
    }
}
