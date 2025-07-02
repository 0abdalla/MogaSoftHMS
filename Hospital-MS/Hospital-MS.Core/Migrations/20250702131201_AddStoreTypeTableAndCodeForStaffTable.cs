using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreTypeTableAndCodeForStaffTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_AspNetUsers_CreatedById",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_CreatedById",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_CreatedById",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_AspNetUsers_CreatedById",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_CreatedById",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_AspNetUsers_CreatedById",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_AspNetUsers_CreatedById",
                schema: "finance",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPerformances_AspNetUsers_CreatedById",
                table: "DoctorPerformances");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_CreatedById",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedules_AspNetUsers_CreatedById",
                table: "DoctorSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceCategory_AspNetUsers_CreatedById",
                table: "InsuranceCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceCompany_AspNetUsers_CreatedById",
                table: "InsuranceCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemGroups_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemTypes_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemUnits_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_JobDepartment_AspNetUsers_CreatedById",
                table: "JobDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLevels_AspNetUsers_CreatedById",
                table: "JobLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_AspNetUsers_CreatedById",
                table: "JobTitles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTypes_AspNetUsers_CreatedById",
                table: "JobTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssueItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_NurseActivities_AspNetUsers_CreatedById",
                table: "NurseActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_NurseShifts_AspNetUsers_CreatedById",
                table: "NurseShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAttachments_AspNetUsers_CreatedById",
                table: "PatientAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicalHistories_AspNetUsers_CreatedById",
                table: "PatientMedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotationItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptPermissionItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_CreatedById",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_AspNetUsers_CreatedById",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffTreasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StaffTreasuries");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCounts_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StoreCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Treasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_AspNetUsers_CreatedById",
                table: "Wards");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Wards",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Treasuries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");


            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                schema: "finance",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "StoreCounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "StaffTreasuries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Staff",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PriceQuotations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PriceQuotationItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "PatientMedicalHistories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "PatientAttachments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "NurseShifts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "NurseActivities",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobTitles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobLevels",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobDepartment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ItemUnits",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ItemTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ItemGroups",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "InsuranceCompany",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "InsuranceCategory",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "DoctorSchedules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "DoctorPerformances",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "DispensePermissions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "CostCenters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Clinics",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Branches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Beds",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Admissions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            
            migrationBuilder.CreateTable(
                name: "StoreTypes",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_TypeId",
                schema: "finance",
                table: "Stores",
                column: "TypeId");

           
            migrationBuilder.CreateIndex(
                name: "IX_StoreTypes_CreatedById",
                schema: "finance",
                table: "StoreTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTypes_UpdatedById",
                schema: "finance",
                table: "StoreTypes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_AspNetUsers_CreatedById",
                table: "Admissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_CreatedById",
                table: "Appointments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_CreatedById",
                table: "Attendances",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_AspNetUsers_CreatedById",
                table: "Beds",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_CreatedById",
                table: "Bookings",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Branches",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_AspNetUsers_CreatedById",
                table: "Clinics",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_AspNetUsers_CreatedById",
                schema: "finance",
                table: "CostCenters",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Customers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "DispensePermissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPerformances_AspNetUsers_CreatedById",
                table: "DoctorPerformances",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_CreatedById",
                table: "Doctors",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedules_AspNetUsers_CreatedById",
                table: "DoctorSchedules",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceCategory_AspNetUsers_CreatedById",
                table: "InsuranceCategory",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceCompany_AspNetUsers_CreatedById",
                table: "InsuranceCompany",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGroups_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemGroups",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Items",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTypes_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemTypes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemUnits_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemUnits",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobDepartment_AspNetUsers_CreatedById",
                table: "JobDepartment",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobLevels_AspNetUsers_CreatedById",
                table: "JobLevels",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobTitles_AspNetUsers_CreatedById",
                table: "JobTitles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobTypes_AspNetUsers_CreatedById",
                table: "JobTypes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssueItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NurseActivities_AspNetUsers_CreatedById",
                table: "NurseActivities",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NurseShifts_AspNetUsers_CreatedById",
                table: "NurseShifts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAttachments_AspNetUsers_CreatedById",
                table: "PatientAttachments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicalHistories_AspNetUsers_CreatedById",
                table: "PatientMedicalHistories",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotationItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrders",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequests",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissionItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_CreatedById",
                table: "Rooms",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_AspNetUsers_CreatedById",
                table: "Staff",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffTreasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StaffTreasuries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCounts_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StoreCounts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Stores",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_StoreTypes_TypeId",
                schema: "finance",
                table: "Stores",
                column: "TypeId",
                principalSchema: "finance",
                principalTable: "StoreTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Suppliers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Treasuries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_AspNetUsers_CreatedById",
                table: "Wards",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_AspNetUsers_CreatedById",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_CreatedById",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_CreatedById",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_AspNetUsers_CreatedById",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_CreatedById",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_AspNetUsers_CreatedById",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_CostCenters_AspNetUsers_CreatedById",
                schema: "finance",
                table: "CostCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DispensePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "DispensePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPerformances_AspNetUsers_CreatedById",
                table: "DoctorPerformances");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_CreatedById",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedules_AspNetUsers_CreatedById",
                table: "DoctorSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceCategory_AspNetUsers_CreatedById",
                table: "InsuranceCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceCompany_AspNetUsers_CreatedById",
                table: "InsuranceCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemGroups_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemTypes_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemUnits_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_JobDepartment_AspNetUsers_CreatedById",
                table: "JobDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLevels_AspNetUsers_CreatedById",
                table: "JobLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_AspNetUsers_CreatedById",
                table: "JobTitles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTypes_AspNetUsers_CreatedById",
                table: "JobTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssueItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_NurseActivities_AspNetUsers_CreatedById",
                table: "NurseActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_NurseShifts_AspNetUsers_CreatedById",
                table: "NurseShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAttachments_AspNetUsers_CreatedById",
                table: "PatientAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicalHistories_AspNetUsers_CreatedById",
                table: "PatientMedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotationItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptPermissionItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_CreatedById",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_AspNetUsers_CreatedById",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffTreasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StaffTreasuries");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCounts_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StoreCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_StoreTypes_TypeId",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Treasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Treasuries");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_AspNetUsers_CreatedById",
                table: "Wards");

            
            migrationBuilder.DropTable(
                name: "StoreTypes",
                schema: "finance");


            migrationBuilder.DropIndex(
                name: "IX_Stores_TypeId",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Staff");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Wards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Treasuries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "StoreCounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "StaffTreasuries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Staff",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PriceQuotations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "PriceQuotationItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "PatientMedicalHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "PatientAttachments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "NurseShifts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "NurseActivities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobTypes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobTitles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobLevels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "JobDepartment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ItemUnits",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ItemTypes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "ItemGroups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "InsuranceCompany",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "InsuranceCategory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "DoctorSchedules",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "DoctorPerformances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "DispensePermissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "CostCenters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Clinics",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "finance",
                table: "Branches",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Beds",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Admissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_AspNetUsers_CreatedById",
                table: "Admissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_CreatedById",
                table: "Appointments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_CreatedById",
                table: "Attendances",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_AspNetUsers_CreatedById",
                table: "Beds",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_CreatedById",
                table: "Bookings",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Branches",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_AspNetUsers_CreatedById",
                table: "Clinics",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenters_AspNetUsers_CreatedById",
                schema: "finance",
                table: "CostCenters",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Customers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DispensePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "DispensePermissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPerformances_AspNetUsers_CreatedById",
                table: "DoctorPerformances",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_CreatedById",
                table: "Doctors",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedules_AspNetUsers_CreatedById",
                table: "DoctorSchedules",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceCategory_AspNetUsers_CreatedById",
                table: "InsuranceCategory",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceCompany_AspNetUsers_CreatedById",
                table: "InsuranceCompany",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGroups_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemGroups",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Items",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTypes_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemTypes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemUnits_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ItemUnits",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobDepartment_AspNetUsers_CreatedById",
                table: "JobDepartment",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLevels_AspNetUsers_CreatedById",
                table: "JobLevels",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTitles_AspNetUsers_CreatedById",
                table: "JobTitles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTypes_AspNetUsers_CreatedById",
                table: "JobTypes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssueItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NurseActivities_AspNetUsers_CreatedById",
                table: "NurseActivities",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NurseShifts_AspNetUsers_CreatedById",
                table: "NurseShifts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAttachments_AspNetUsers_CreatedById",
                table: "PatientAttachments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicalHistories_AspNetUsers_CreatedById",
                table: "PatientMedicalHistories",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotationItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseOrders",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequests",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissionItems_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_CreatedById",
                table: "Rooms",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_AspNetUsers_CreatedById",
                table: "Staff",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffTreasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StaffTreasuries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCounts_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StoreCounts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Stores",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Suppliers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Treasuries_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Treasuries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_AspNetUsers_CreatedById",
                table: "Wards",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
