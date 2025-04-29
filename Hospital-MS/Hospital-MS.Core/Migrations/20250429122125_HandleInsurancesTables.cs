using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class HandleInsurancesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "InsuranceCompany",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ContractEndDate",
                table: "InsuranceCompany",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ContractStartDate",
                table: "InsuranceCompany",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "InsuranceCompany",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "InsuranceCompany",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceCompanyId",
                table: "InsuranceCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "InsuranceCategory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCategory_InsuranceCompanyId",
                table: "InsuranceCategory",
                column: "InsuranceCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceCategory_InsuranceCompany_InsuranceCompanyId",
                table: "InsuranceCategory",
                column: "InsuranceCompanyId",
                principalTable: "InsuranceCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceCategory_InsuranceCompany_InsuranceCompanyId",
                table: "InsuranceCategory");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCategory_InsuranceCompanyId",
                table: "InsuranceCategory");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "InsuranceCompany");

            migrationBuilder.DropColumn(
                name: "ContractEndDate",
                table: "InsuranceCompany");

            migrationBuilder.DropColumn(
                name: "ContractStartDate",
                table: "InsuranceCompany");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "InsuranceCompany");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "InsuranceCompany");

            migrationBuilder.DropColumn(
                name: "InsuranceCompanyId",
                table: "InsuranceCategory");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "InsuranceCategory");
        }
    }
}
