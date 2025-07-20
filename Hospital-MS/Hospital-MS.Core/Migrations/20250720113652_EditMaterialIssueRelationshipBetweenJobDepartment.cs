using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class EditMaterialIssueRelationshipBetweenJobDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssuePermissions_Branches_BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropIndex(
                name: "IX_MaterialIssuePermissions_BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.AddColumn<int>(
                name: "JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                type: "int",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "JobDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssuePermissions_JobDepartment_JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "JobDepartmentId",
                principalTable: "JobDepartment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssuePermissions_JobDepartment_JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropIndex(
                name: "IX_MaterialIssuePermissions_JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropColumn(
                name: "JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssuePermissions_Branches_BranchId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "BranchId",
                principalSchema: "finance",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
