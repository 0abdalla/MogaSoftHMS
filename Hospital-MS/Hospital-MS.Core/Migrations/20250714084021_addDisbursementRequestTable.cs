using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class addDisbursementRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisbursementRequests",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementRequests_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementRequests_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementRequests_JobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "JobDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DisbursementRequestItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DisbursementRequestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementRequestItems_DisbursementRequests_DisbursementRequestId",
                        column: x => x.DisbursementRequestId,
                        principalSchema: "finance",
                        principalTable: "DisbursementRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementRequestItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequestItems_DisbursementRequestId",
                schema: "finance",
                table: "DisbursementRequestItems",
                column: "DisbursementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequestItems_ItemId",
                schema: "finance",
                table: "DisbursementRequestItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequests_CreatedById",
                schema: "finance",
                table: "DisbursementRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequests_JobDepartmentId",
                schema: "finance",
                table: "DisbursementRequests",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequests_UpdatedById",
                schema: "finance",
                table: "DisbursementRequests",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisbursementRequestItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DisbursementRequests",
                schema: "finance");
        }
    }
}
