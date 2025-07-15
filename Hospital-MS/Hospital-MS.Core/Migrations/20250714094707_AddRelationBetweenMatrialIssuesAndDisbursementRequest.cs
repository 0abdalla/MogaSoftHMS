using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenMatrialIssuesAndDisbursementRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                schema: "finance",
                table: "DisbursementRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "DisbursementRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialIssuePermissions_DisbursementRequests_DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "DisbursementRequestId",
                principalSchema: "finance",
                principalTable: "DisbursementRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialIssuePermissions_DisbursementRequests_DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropIndex(
                name: "IX_MaterialIssuePermissions_DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropColumn(
                name: "DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions");

            migrationBuilder.DropColumn(
                name: "Number",
                schema: "finance",
                table: "DisbursementRequests");
        }
    }
}
