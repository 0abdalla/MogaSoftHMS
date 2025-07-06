using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRestrictionDailyAndDetailsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "RestrictionTypes",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestrictionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestrictionTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RestrictionTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyRestrictions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestrictionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RestrictionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RestrictionTypeId = table.Column<int>(type: "int", nullable: false),
                    LedgerNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_RestrictionTypes_RestrictionTypeId",
                        column: x => x.RestrictionTypeId,
                        principalSchema: "finance",
                        principalTable: "RestrictionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailyRestrictionDetails",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRestrictionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "finance",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "finance",
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictionDetails_AccountId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictionDetails_CostCenterId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictionDetails_DailyRestrictionId",
                schema: "finance",
                table: "DailyRestrictionDetails",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_CreatedById",
                schema: "finance",
                table: "DailyRestrictions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_RestrictionTypeId",
                schema: "finance",
                table: "DailyRestrictions",
                column: "RestrictionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRestrictions_UpdatedById",
                schema: "finance",
                table: "DailyRestrictions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RestrictionTypes_CreatedById",
                schema: "finance",
                table: "RestrictionTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RestrictionTypes_UpdatedById",
                schema: "finance",
                table: "RestrictionTypes",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyRestrictionDetails",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DailyRestrictions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "RestrictionTypes",
                schema: "finance");
        }
    }
}
