using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0196ce06-a35d-764c-8e7f-5296e1c570a9", "0196ce08-5df8-732f-9f84-7024b78b0f4d", "ApplicationRole", false, "SystemAdmin", "SYSTEMADMIN" },
                    { "0196ce06-a360-71e0-b597-893f262003e5", "0196ce08-5df8-732f-9f84-702596f53eb7", "ApplicationRole", false, "TopManagement", "TOPMANAGEMENT" },
                    { "0196ce06-a360-71e0-b597-89401c1a99a6", "0196ce08-5df8-732f-9f84-7026d200cb76", "ApplicationRole", false, "FinanceManager", "FINANCEMANAGER" },
                    { "0196ce06-a360-71e0-b597-8941805b5a7e", "0196ce08-5df8-732f-9f84-70271213664d", "ApplicationRole", false, "TechnicalManager", "TECHNICALMANAGER" },
                    { "0196ce06-a360-71e0-b597-89425e3c917f", "0196ce08-5df8-732f-9f84-7028b1911846", "ApplicationRole", false, "HRManager", "HRMANAGER" },
                    { "0196ce06-a360-71e0-b597-89431c9fe61b", "0196ce08-5df8-732f-9f84-702986ab7a2b", "ApplicationRole", false, "Accountant", "ACCOUNTANT" },
                    { "0196ce06-a360-71e0-b597-89441848d7b0", "0196ce08-5df8-732f-9f84-702a00ae8e82", "ApplicationRole", false, "StoreKeeper", "STOREKEEPER" },
                    { "0196ce06-a360-71e0-b597-89455dd69d96", "0196ce08-5df8-732f-9f84-702b6aab3540", "ApplicationRole", false, "ReservationEmployee", "RESERVATIONEMPLOYEE" },
                    { "0196ce06-a360-71e0-b597-894637b7ca29", "0196ce08-5df8-732f-9f84-702cb2a84a7b", "ApplicationRole", false, "Cashier", "CASHIER" },
                    { "0196ce0b-5aab-766e-9cb1-4e205c0013c7", "0196ce08-5df8-732f-9f84-702d6b892289", "ApplicationRole", false, "Auditor", "AUDITOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a35d-764c-8e7f-5296e1c570a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-893f262003e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-89401c1a99a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-8941805b5a7e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-89425e3c917f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-89431c9fe61b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-89441848d7b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-89455dd69d96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce06-a360-71e0-b597-894637b7ca29");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196ce0b-5aab-766e-9cb1-4e205c0013c7");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");
        }
    }
}
