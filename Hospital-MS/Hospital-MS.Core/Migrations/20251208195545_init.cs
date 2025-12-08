using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "config");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                schema: "config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PenaltyTypes",
                schema: "dbo",
                columns: table => new
                {
                    PenaltyTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyTypes", x => x.PenaltyTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyBodyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyBodyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClosedBy = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountingGuidance",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingGuidance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingGuidance_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountingGuidance_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountTrees",
                schema: "Finance",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentAccountId = table.Column<int>(type: "int", nullable: true),
                    AccountLevel = table.Column<int>(type: "int", nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: true),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    AccountNature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    IsDisToCostCenter = table.Column<bool>(type: "bit", nullable: true),
                    IsPost = table.Column<bool>(type: "bit", nullable: true),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepreciationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepreciationYears = table.Column<int>(type: "int", nullable: true),
                    DepreciationId = table.Column<int>(type: "int", nullable: true),
                    AccumulatedDepreciationId = table.Column<int>(type: "int", nullable: true),
                    PreCredit = table.Column<double>(type: "float", nullable: true),
                    PreDebit = table.Column<double>(type: "float", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTrees", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_AccountTrees_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountTrees_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvanceTypes",
                schema: "dbo",
                columns: table => new
                {
                    AdvanceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvanceTypes", x => x.AdvanceTypeId);
                    table.ForeignKey(
                        name: "FK_AdvanceTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvanceTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitialBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banks_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clinics_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clinics_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContractDetails",
                schema: "dbo",
                columns: table => new
                {
                    ContractDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    BasicSalary = table.Column<double>(type: "float", nullable: false),
                    ExtraSalary = table.Column<double>(type: "float", nullable: true),
                    Transportation = table.Column<double>(type: "float", nullable: true),
                    HousingAllowance = table.Column<double>(type: "float", nullable: true),
                    MobileAllowance = table.Column<double>(type: "float", nullable: true),
                    WorkNature = table.Column<double>(type: "float", nullable: true),
                    MealAllowance = table.Column<double>(type: "float", nullable: true),
                    Other = table.Column<double>(type: "float", nullable: true),
                    GrossSalary = table.Column<double>(type: "float", nullable: true),
                    TotalSalary = table.Column<double>(type: "float", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDetails", x => x.ContractDetailId);
                    table.ForeignKey(
                        name: "FK_ContractDetails_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContractDetails_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCenters_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CostCenters_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostCenterTree",
                schema: "Finance",
                columns: table => new
                {
                    CostCenterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCenterNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CostLevel = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: true),
                    IsPost = table.Column<bool>(type: "bit", nullable: true),
                    IsExpences = table.Column<int>(type: "int", nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenterTree", x => x.CostCenterId);
                    table.ForeignKey(
                        name: "FK_CostCenterTree_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CostCenterTree_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResponsibleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResponsibleName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Job = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Phone2 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Telephone2 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentResponsible = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(755)", maxLength: 755, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaries",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    EmployeeCode = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicSalary = table.Column<double>(type: "float", nullable: false),
                    DailyRate = table.Column<double>(type: "float", nullable: false),
                    AttendanceDays = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Insurance = table.Column<double>(type: "float", nullable: false),
                    NetSalary = table.Column<double>(type: "float", nullable: false),
                    Taxes = table.Column<double>(type: "float", nullable: false),
                    Advances = table.Column<double>(type: "float", nullable: false),
                    AmountDue = table.Column<double>(type: "float", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeSalaries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiscalYears_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FiscalYears_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ContractEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceCompany_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InsuranceCompany_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemUnits",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemUnits_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemUnits_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDepartment_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobDepartment_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLevels_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobLevels_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MainGroups",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainGroups_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MainGroups_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Penalties",
                schema: "dbo",
                columns: table => new
                {
                    PenaltyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    PenaltyTypeId = table.Column<int>(type: "int", nullable: false),
                    PenaltyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeductionByDays = table.Column<double>(type: "float", nullable: false),
                    DeductionAmount = table.Column<double>(type: "float", nullable: true),
                    TotalDeduction = table.Column<double>(type: "float", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkflowStatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.PenaltyId);
                    table.ForeignKey(
                        name: "FK_Penalties_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Penalties_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestrictionTypes",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
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
                name: "StoreTypes",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResponsibleName1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResponsibleName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Phone2 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Job = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Fax1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suppliers_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vacations",
                schema: "dbo",
                columns: table => new
                {
                    VacationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    VacationTypeId = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastDayWork = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: true),
                    VacationMonth = table.Column<int>(type: "int", nullable: true),
                    WorkflowStatusId = table.Column<int>(type: "int", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastJoinDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacations", x => x.VacationId);
                    table.ForeignKey(
                        name: "FK_Vacations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VacationTypes",
                schema: "dbo",
                columns: table => new
                {
                    VacationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationTypes", x => x.VacationTypeId);
                    table.ForeignKey(
                        name: "FK_VacationTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VacationTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wards_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShiftMedicalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: true),
                    MedicalServiceName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftMedicalServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftMedicalServices_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Treasuries",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treasuries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treasuries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Treasuries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Treasuries_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "finance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    SpecialtyId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Doctors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Doctors_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalServices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalServices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalServices_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsuranceCompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceCategory_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InsuranceCategory_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InsuranceCategory_InsuranceCompany_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "InsuranceCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementRequests",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
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
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitles_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobTitles_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobTitles_JobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "JobDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemGroups",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MainGroupId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemGroups_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemGroups_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemGroups_MainGroups_MainGroupId",
                        column: x => x.MainGroupId,
                        principalSchema: "finance",
                        principalTable: "MainGroups",
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
                    RestrictionTypeId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountingGuidanceId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRestrictions_AccountingGuidance_AccountingGuidanceId",
                        column: x => x.AccountingGuidanceId,
                        principalSchema: "finance",
                        principalTable: "AccountingGuidance",
                        principalColumn: "Id");
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_StoreTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "finance",
                        principalTable: "StoreTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    DailyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryMovements",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasuryNumber = table.Column<int>(type: "int", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenedIn = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedIn = table.Column<DateOnly>(type: "date", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    TotalCredits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDebits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    IsReEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasuryMovements_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreasuryMovements_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreasuryMovements_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorPerformances_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorPerformances_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorPerformances_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CurrentAppointments = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSchedules_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorSchedules_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorSchedules_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorMedicalService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMedicalService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalService_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorMedicalService_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorMedicalService_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalService_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalServiceSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekDay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServiceSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalServiceSchedules_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalServiceSchedules_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalServiceSchedules_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyPhone01 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmergencyContact01 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EmergencyPhone02 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmergencyContact02 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true),
                    InsuranceCompanyId = table.Column<int>(type: "int", nullable: true),
                    InsuranceCategoryId = table.Column<int>(type: "int", nullable: true),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_InsuranceCategory_InsuranceCategoryId",
                        column: x => x.InsuranceCategoryId,
                        principalTable: "InsuranceCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_InsuranceCompany_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "InsuranceCompany",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1055)", maxLength: 1055, nullable: true),
                    JobTitleId = table.Column<int>(type: "int", nullable: true),
                    JobTypeId = table.Column<int>(type: "int", nullable: true),
                    JobLevelId = table.Column<int>(type: "int", nullable: true),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    BasicSalary = table.Column<double>(type: "float", nullable: true),
                    Tax = table.Column<int>(type: "int", nullable: true),
                    Insurance = table.Column<int>(type: "int", nullable: true),
                    VacationDays = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    VariableSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VisaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Allowances = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rewards = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClinicId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "finance",
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "JobDepartment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobLevels_JobLevelId",
                        column: x => x.JobLevelId,
                        principalTable: "JobLevels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staff_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    OrderLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    SalesTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    PriceAfterTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    HasBarcode = table.Column<bool>(type: "bit", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    IsGroupHead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_ItemGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "finance",
                        principalTable: "ItemGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "finance",
                        principalTable: "ItemTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_ItemUnits_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "finance",
                        principalTable: "ItemUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdditionNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Finance",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdditionNotices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdditionNotices_Banks_BankId",
                        column: x => x.BankId,
                        principalSchema: "finance",
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionNotices_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
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
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRestrictionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Finance",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_CostCenterTree_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Finance",
                        principalTable: "CostCenterTree",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_DailyRestrictionDetails_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DebitNotices",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebitNotices_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Finance",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNotices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DebitNotices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DebitNotices_Banks_BankId",
                        column: x => x.BankId,
                        principalSchema: "finance",
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNotices_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DispensePermissions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DispenseTo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TreasuryId = table.Column<int>(type: "int", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispensePermissions_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Finance",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_DispensePermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispensePermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispensePermissions_CostCenterTree_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Finance",
                        principalTable: "CostCenterTree",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_DispensePermissions_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispensePermissions_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplyReceipts",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_AccountTrees_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Finance",
                        principalTable: "AccountTrees",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_CostCenterTree_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Finance",
                        principalTable: "CostCenterTree",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplyReceipts_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialIssuePermissions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: true),
                    DisbursementRequestId = table.Column<int>(type: "int", nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssuePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_DisbursementRequests_DisbursementRequestId",
                        column: x => x.DisbursementRequestId,
                        principalSchema: "finance",
                        principalTable: "DisbursementRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_JobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "JobDepartment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssuePermissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreCounts",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCounts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreCounts_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreCounts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beds_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Beds_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Beds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryOperations",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    TreasuryMovementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasuryOperations_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreasuryOperations_TreasuryMovements_TreasuryMovementId",
                        column: x => x.TreasuryMovementId,
                        principalSchema: "finance",
                        principalTable: "TreasuryMovements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    ClinicId = table.Column<int>(type: "int", nullable: true),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentNumber = table.Column<int>(type: "int", nullable: false),
                    EmergencyLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanionNationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanionPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "EGP"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAttachments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientAttachments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientAttachments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientMedicalHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1055)", maxLength: 1055, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedicalHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientMedicalHistories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientMedicalHistories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientMedicalHistories_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendaceSalaries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkHours = table.Column<int>(type: "int", nullable: true),
                    WorkDays = table.Column<double>(type: "float", nullable: true),
                    RequiredHours = table.Column<int>(type: "int", nullable: true),
                    TotalFingerprintHours = table.Column<double>(type: "float", nullable: true),
                    SickDays = table.Column<double>(type: "float", nullable: true),
                    OtherDays = table.Column<double>(type: "float", nullable: true),
                    Fridays = table.Column<int>(type: "int", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Overtime = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendaceSalaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendaceSalaries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendaceSalaries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendaceSalaries_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    InTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    OutTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attendances_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAdvances",
                schema: "dbo",
                columns: table => new
                {
                    StaffAdvanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvanceNumber = table.Column<int>(type: "int", nullable: false),
                    AdvanceTypeId = table.Column<int>(type: "int", nullable: false),
                    AdvanceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvanceAmount = table.Column<double>(type: "float", nullable: false),
                    PaymentAmount = table.Column<double>(type: "float", nullable: false),
                    Benefit = table.Column<double>(type: "float", nullable: true),
                    PaymentFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkflowStatusId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAdvances", x => x.StaffAdvanceId);
                    table.ForeignKey(
                        name: "FK_EmployeeAdvances_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeAdvances_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeAdvances_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NurseActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NurseActivities_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseActivities_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseActivities_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NurseActivities_Staff_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NurseShifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseId = table.Column<int>(type: "int", nullable: false),
                    ShiftDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NurseShifts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseShifts_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseShifts_Staff_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NurseShifts_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffAttachments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffAttachments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffAttachments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffTreasuries",
                schema: "finance",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffTreasuries", x => new { x.StaffId, x.TreasuryId });
                    table.ForeignKey(
                        name: "FK_StaffTreasuries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffTreasuries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffTreasuries_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffTreasuries_Treasuries_TreasuryId",
                        column: x => x.TreasuryId,
                        principalSchema: "finance",
                        principalTable: "Treasuries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementRequestItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementRequestItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "MaterialIssueItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialIssuePermissionId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialIssueItems_MaterialIssuePermissions_MaterialIssuePermissionId",
                        column: x => x.MaterialIssuePermissionId,
                        principalSchema: "finance",
                        principalTable: "MaterialIssuePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HealthStatus = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InitialDiagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    HasCompanion = table.Column<bool>(type: "bit", nullable: false),
                    CompanionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanionNationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CompanionPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissions_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalServiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false),
                    RadiologyBodyTypeId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalServiceDetails_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalServiceDetails_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalServiceDetails_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalServiceDetails_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceQuotationItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceQuotationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceQuotationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceQuotationItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PriceQuotationItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PriceQuotationItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceQuotations",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuotationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PriceQuotations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PriceQuotations_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "finance",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PriceQuotationId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_PriceQuotations_PriceQuotationId",
                        column: x => x.PriceQuotationId,
                        principalSchema: "finance",
                        principalTable: "PriceQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "finance",
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "finance",
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "finance",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "finance",
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequestedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "finance",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptPermissions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DailyRestrictionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_DailyRestrictions_DailyRestrictionId",
                        column: x => x.DailyRestrictionId,
                        principalSchema: "finance",
                        principalTable: "DailyRestrictions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "finance",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "finance",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "finance",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptPermissionItems",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiptPermissionId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptPermissionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "finance",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptPermissionItems_ReceiptPermissions_ReceiptPermissionId",
                        column: x => x.ReceiptPermissionId,
                        principalSchema: "finance",
                        principalTable: "ReceiptPermissions",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AccountingGuidance_CreatedById",
                schema: "finance",
                table: "AccountingGuidance",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingGuidance_UpdatedById",
                schema: "finance",
                table: "AccountingGuidance",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTrees_CreatedById",
                schema: "Finance",
                table: "AccountTrees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTrees_UpdatedById",
                schema: "Finance",
                table: "AccountTrees",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_AccountId",
                table: "AdditionNotices",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_BankId",
                table: "AdditionNotices",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_CreatedById",
                table: "AdditionNotices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_DailyRestrictionId",
                table: "AdditionNotices",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionNotices_UpdatedById",
                table: "AdditionNotices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_BedId",
                table: "Admissions",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_CreatedById",
                table: "Admissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DepartmentId",
                table: "Admissions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_MedicalServiceId",
                table: "Admissions",
                column: "MedicalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_RoomId",
                table: "Admissions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_UpdatedById",
                table: "Admissions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdvanceTypes_CreatedById",
                schema: "dbo",
                table: "AdvanceTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdvanceTypes_UpdatedById",
                schema: "dbo",
                table: "AdvanceTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CreatedById",
                table: "Appointments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalServiceId",
                table: "Appointments",
                column: "MedicalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UpdatedById",
                table: "Appointments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AttendaceSalaries_CreatedById",
                schema: "dbo",
                table: "AttendaceSalaries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AttendaceSalaries_StaffId",
                schema: "dbo",
                table: "AttendaceSalaries",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendaceSalaries_UpdatedById",
                schema: "dbo",
                table: "AttendaceSalaries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CreatedById",
                table: "Attendances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StaffId",
                table: "Attendances",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UpdatedById",
                table: "Attendances",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CreatedById",
                schema: "finance",
                table: "Banks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_UpdatedById",
                schema: "finance",
                table: "Banks",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_CreatedById",
                table: "Beds",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_RoomId",
                table: "Beds",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_UpdatedById",
                table: "Beds",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingNumber",
                table: "Bookings",
                column: "BookingNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedById",
                table: "Bookings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PatientId",
                table: "Bookings",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UpdatedById",
                table: "Bookings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_WardId",
                table: "Bookings",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedById",
                schema: "finance",
                table: "Branches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UpdatedById",
                schema: "finance",
                table: "Branches",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_CreatedById",
                table: "Clinics",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_UpdatedById",
                table: "Clinics",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_CreatedById",
                schema: "dbo",
                table: "ContractDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_UpdatedById",
                schema: "dbo",
                table: "ContractDetails",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_CreatedById",
                schema: "finance",
                table: "CostCenters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_UpdatedById",
                schema: "finance",
                table: "CostCenters",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterTree_CreatedById",
                schema: "Finance",
                table: "CostCenterTree",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterTree_UpdatedById",
                schema: "Finance",
                table: "CostCenterTree",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Code",
                schema: "finance",
                table: "Customers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedById",
                schema: "finance",
                table: "Customers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                schema: "finance",
                table: "Customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Phone",
                schema: "finance",
                table: "Customers",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UpdatedById",
                schema: "finance",
                table: "Customers",
                column: "UpdatedById");

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
                name: "IX_DailyRestrictions_AccountingGuidanceId",
                schema: "finance",
                table: "DailyRestrictions",
                column: "AccountingGuidanceId");

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
                name: "IX_DebitNotices_AccountId",
                schema: "finance",
                table: "DebitNotices",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_BankId",
                schema: "finance",
                table: "DebitNotices",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_CreatedById",
                schema: "finance",
                table: "DebitNotices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_DailyRestrictionId",
                schema: "finance",
                table: "DebitNotices",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotices_UpdatedById",
                schema: "finance",
                table: "DebitNotices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedById",
                table: "Departments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UpdatedById",
                table: "Departments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementRequestItems_CreatedById",
                schema: "finance",
                table: "DisbursementRequestItems",
                column: "CreatedById");

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
                name: "IX_DisbursementRequestItems_UpdatedById",
                schema: "finance",
                table: "DisbursementRequestItems",
                column: "UpdatedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_AccountId",
                schema: "finance",
                table: "DispensePermissions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_CostCenterId",
                schema: "finance",
                table: "DispensePermissions",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_CreatedById",
                schema: "finance",
                table: "DispensePermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_DailyRestrictionId",
                schema: "finance",
                table: "DispensePermissions",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_TreasuryId",
                schema: "finance",
                table: "DispensePermissions",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensePermissions_UpdatedById",
                schema: "finance",
                table: "DispensePermissions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalService_CreatedById",
                table: "DoctorMedicalService",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalService_DoctorId",
                table: "DoctorMedicalService",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalService_MedicalServiceId",
                table: "DoctorMedicalService",
                column: "MedicalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalService_UpdatedById",
                table: "DoctorMedicalService",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPerformances_CreatedById",
                table: "DoctorPerformances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPerformances_DoctorId",
                table: "DoctorPerformances",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPerformances_UpdatedById",
                table: "DoctorPerformances",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_CreatedById",
                table: "Doctors",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SpecialtyId",
                table: "Doctors",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UpdatedById",
                table: "Doctors",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_CreatedById",
                table: "DoctorSchedules",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedules",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_UpdatedById",
                table: "DoctorSchedules",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_CreatedById",
                schema: "dbo",
                table: "EmployeeAdvances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_StaffId",
                schema: "dbo",
                table: "EmployeeAdvances",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdvances_UpdatedById",
                schema: "dbo",
                table: "EmployeeAdvances",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_CreatedById",
                schema: "finance",
                table: "EmployeeSalaries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_UpdatedById",
                schema: "finance",
                table: "EmployeeSalaries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYears_CreatedById",
                schema: "finance",
                table: "FiscalYears",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYears_UpdatedById",
                schema: "finance",
                table: "FiscalYears",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCategory_CreatedById",
                table: "InsuranceCategory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCategory_InsuranceCompanyId",
                table: "InsuranceCategory",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCategory_UpdatedById",
                table: "InsuranceCategory",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompany_CreatedById",
                table: "InsuranceCompany",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompany_UpdatedById",
                table: "InsuranceCompany",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_CreatedById",
                schema: "finance",
                table: "ItemGroups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_MainGroupId",
                schema: "finance",
                table: "ItemGroups",
                column: "MainGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_UpdatedById",
                schema: "finance",
                table: "ItemGroups",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatedById",
                schema: "finance",
                table: "Items",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GroupId",
                schema: "finance",
                table: "Items",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TypeId",
                schema: "finance",
                table: "Items",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UnitId",
                schema: "finance",
                table: "Items",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UpdatedById",
                schema: "finance",
                table: "Items",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_CreatedById",
                schema: "finance",
                table: "ItemTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_NameAr",
                schema: "finance",
                table: "ItemTypes",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_NameEn",
                schema: "finance",
                table: "ItemTypes",
                column: "NameEn");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_UpdatedById",
                schema: "finance",
                table: "ItemTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_CreatedById",
                schema: "finance",
                table: "ItemUnits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_UpdatedById",
                schema: "finance",
                table: "ItemUnits",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartment_CreatedById",
                table: "JobDepartment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartment_UpdatedById",
                table: "JobDepartment",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_CreatedById",
                table: "JobLevels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_UpdatedById",
                table: "JobLevels",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_CreatedById",
                table: "JobTitles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_JobDepartmentId",
                table: "JobTitles",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_UpdatedById",
                table: "JobTitles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTypes_CreatedById",
                table: "JobTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTypes_UpdatedById",
                table: "JobTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MainGroups_CreatedById",
                schema: "finance",
                table: "MainGroups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MainGroups_UpdatedById",
                schema: "finance",
                table: "MainGroups",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_CreatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_ItemId",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_MaterialIssuePermissionId",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "MaterialIssuePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueItems_UpdatedById",
                schema: "finance",
                table: "MaterialIssueItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_CreatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_DailyRestrictionId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_DisbursementRequestId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "DisbursementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_JobDepartmentId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_StoreId",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssuePermissions_UpdatedById",
                schema: "finance",
                table: "MaterialIssuePermissions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceDetails_AppointmentId",
                table: "MedicalServiceDetails",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceDetails_CreatedById",
                table: "MedicalServiceDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceDetails_MedicalServiceId",
                table: "MedicalServiceDetails",
                column: "MedicalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceDetails_UpdatedById",
                table: "MedicalServiceDetails",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServices_CreatedById",
                table: "MedicalServices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServices_DepartmentId",
                table: "MedicalServices",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServices_UpdatedById",
                table: "MedicalServices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceSchedules_CreatedById",
                table: "MedicalServiceSchedules",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceSchedules_MedicalServiceId",
                table: "MedicalServiceSchedules",
                column: "MedicalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServiceSchedules_UpdatedById",
                table: "MedicalServiceSchedules",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NurseActivities_CreatedById",
                table: "NurseActivities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NurseActivities_NurseId",
                table: "NurseActivities",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseActivities_PatientId",
                table: "NurseActivities",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseActivities_UpdatedById",
                table: "NurseActivities",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NurseShifts_CreatedById",
                table: "NurseShifts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NurseShifts_NurseId",
                table: "NurseShifts",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseShifts_UpdatedById",
                table: "NurseShifts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NurseShifts_WardId",
                table: "NurseShifts",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAttachments_CreatedById",
                table: "PatientAttachments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAttachments_PatientId",
                table: "PatientAttachments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAttachments_UpdatedById",
                table: "PatientAttachments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalHistories_CreatedById",
                table: "PatientMedicalHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalHistories_PatientId",
                table: "PatientMedicalHistories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalHistories_UpdatedById",
                table: "PatientMedicalHistories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedById",
                table: "Patients",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceCategoryId",
                table: "Patients",
                column: "InsuranceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceCompanyId",
                table: "Patients",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UpdatedById",
                table: "Patients",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_CreatedById",
                schema: "dbo",
                table: "Penalties",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_UpdatedById",
                schema: "dbo",
                table: "Penalties",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotationItems_CreatedById",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotationItems_ItemId",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotationItems_PriceQuotationId",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "PriceQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotationItems_UpdatedById",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotations_CreatedById",
                schema: "finance",
                table: "PriceQuotations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotations_PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotations_SupplierId",
                schema: "finance",
                table: "PriceQuotations",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuotations_UpdatedById",
                schema: "finance",
                table: "PriceQuotations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_CreatedById",
                schema: "finance",
                table: "PurchaseOrderItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_ItemId",
                schema: "finance",
                table: "PurchaseOrderItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                schema: "finance",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_UpdatedById",
                schema: "finance",
                table: "PurchaseOrderItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CostCenterId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CreatedById",
                schema: "finance",
                table: "PurchaseOrders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseRequestId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_StoreId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                schema: "finance",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_UpdatedById",
                schema: "finance",
                table: "PurchaseOrders",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_CreatedById",
                schema: "finance",
                table: "PurchaseRequestItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_ItemId",
                schema: "finance",
                table: "PurchaseRequestItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_PurchaseRequestId",
                schema: "finance",
                table: "PurchaseRequestItems",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_UpdatedById",
                schema: "finance",
                table: "PurchaseRequestItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CreatedById",
                schema: "finance",
                table: "PurchaseRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests",
                column: "PriceQuotationId",
                unique: true,
                filter: "[PriceQuotationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_StoreId",
                schema: "finance",
                table: "PurchaseRequests",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_UpdatedById",
                schema: "finance",
                table: "PurchaseRequests",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_CreatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_ItemId",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_ReceiptPermissionId",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "ReceiptPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissionItems_UpdatedById",
                schema: "finance",
                table: "ReceiptPermissionItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_CreatedById",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_DailyRestrictionId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_PurchaseOrderId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_StoreId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_SupplierId",
                schema: "finance",
                table: "ReceiptPermissions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptPermissions_UpdatedById",
                schema: "finance",
                table: "ReceiptPermissions",
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

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CreatedById",
                table: "Rooms",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_UpdatedById",
                table: "Rooms",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_WardId",
                table: "Rooms",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftMedicalServices_ShiftId",
                table: "ShiftMedicalServices",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_BranchId",
                table: "Staff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_ClinicId",
                table: "Staff",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_CreatedById",
                table: "Staff",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobDepartmentId",
                table: "Staff",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobLevelId",
                table: "Staff",
                column: "JobLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobTitleId",
                table: "Staff",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobTypeId",
                table: "Staff",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UpdatedById",
                table: "Staff",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttachments_CreatedById",
                table: "StaffAttachments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttachments_StaffId",
                table: "StaffAttachments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttachments_UpdatedById",
                table: "StaffAttachments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTreasuries_CreatedById",
                schema: "finance",
                table: "StaffTreasuries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTreasuries_TreasuryId",
                schema: "finance",
                table: "StaffTreasuries",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTreasuries_UpdatedById",
                schema: "finance",
                table: "StaffTreasuries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCounts_CreatedById",
                schema: "finance",
                table: "StoreCounts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCounts_StoreId",
                schema: "finance",
                table: "StoreCounts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCounts_UpdatedById",
                schema: "finance",
                table: "StoreCounts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CreatedById",
                schema: "finance",
                table: "Stores",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_TypeId",
                schema: "finance",
                table: "Stores",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_UpdatedById",
                schema: "finance",
                table: "Stores",
                column: "UpdatedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_AccountCode",
                schema: "finance",
                table: "Suppliers",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatedById",
                schema: "finance",
                table: "Suppliers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Email",
                schema: "finance",
                table: "Suppliers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Phone1",
                schema: "finance",
                table: "Suppliers",
                column: "Phone1");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TaxNumber",
                schema: "finance",
                table: "Suppliers",
                column: "TaxNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UpdatedById",
                schema: "finance",
                table: "Suppliers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_AccountId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_CostCenterId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_CreatedById",
                schema: "finance",
                table: "SupplyReceipts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_DailyRestrictionId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "DailyRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_TreasuryId",
                schema: "finance",
                table: "SupplyReceipts",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyReceipts_UpdatedById",
                schema: "finance",
                table: "SupplyReceipts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Treasuries_BranchId",
                schema: "finance",
                table: "Treasuries",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Treasuries_CreatedById",
                schema: "finance",
                table: "Treasuries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Treasuries_UpdatedById",
                schema: "finance",
                table: "Treasuries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryMovements_CreatedById",
                schema: "finance",
                table: "TreasuryMovements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryMovements_TreasuryId",
                schema: "finance",
                table: "TreasuryMovements",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryMovements_UpdatedById",
                schema: "finance",
                table: "TreasuryMovements",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryOperations_TreasuryId",
                schema: "finance",
                table: "TreasuryOperations",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryOperations_TreasuryMovementId",
                schema: "finance",
                table: "TreasuryOperations",
                column: "TreasuryMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_CreatedById",
                schema: "dbo",
                table: "Vacations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_UpdatedById",
                schema: "dbo",
                table: "Vacations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_CreatedById",
                schema: "dbo",
                table: "VacationTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_UpdatedById",
                schema: "dbo",
                table: "VacationTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_CreatedById",
                table: "Wards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_UpdatedById",
                table: "Wards",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotationItems_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PriceQuotationItems",
                column: "PriceQuotationId",
                principalSchema: "finance",
                principalTable: "PriceQuotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceQuotations_PurchaseRequests_PurchaseRequestId",
                schema: "finance",
                table: "PriceQuotations",
                column: "PurchaseRequestId",
                principalSchema: "finance",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceQuotations_AspNetUsers_UpdatedById",
                schema: "finance",
                table: "PriceQuotations");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_AspNetUsers_CreatedById",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_AspNetUsers_UpdatedById",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_AspNetUsers_UpdatedById",
                schema: "finance",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTypes_AspNetUsers_CreatedById",
                schema: "finance",
                table: "StoreTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTypes_AspNetUsers_UpdatedById",
                schema: "finance",
                table: "StoreTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedById",
                schema: "finance",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_UpdatedById",
                schema: "finance",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Stores_StoreId",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_PriceQuotations_PriceQuotationId",
                schema: "finance",
                table: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "AdditionNotices");

            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.DropTable(
                name: "AdvanceTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AttendaceSalaries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "ContractDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DailyRestrictionDetails",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DebitNotices",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DisbursementRequestItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DispensePermissions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DoctorMedicalService");

            migrationBuilder.DropTable(
                name: "DoctorPerformances");

            migrationBuilder.DropTable(
                name: "DoctorSchedules");

            migrationBuilder.DropTable(
                name: "EmployeeAdvances",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeSalaries",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "FiscalYears",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "MaterialIssueItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "MedicalServiceDetails");

            migrationBuilder.DropTable(
                name: "MedicalServiceSchedules");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NurseActivities");

            migrationBuilder.DropTable(
                name: "NurseShifts");

            migrationBuilder.DropTable(
                name: "Pages",
                schema: "config");

            migrationBuilder.DropTable(
                name: "PatientAttachments");

            migrationBuilder.DropTable(
                name: "PatientMedicalHistories");

            migrationBuilder.DropTable(
                name: "Penalties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PenaltyTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PriceQuotationItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PurchaseRequestItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "RadiologyBodyTypes");

            migrationBuilder.DropTable(
                name: "ReceiptPermissionItems",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "config");

            migrationBuilder.DropTable(
                name: "ShiftMedicalServices");

            migrationBuilder.DropTable(
                name: "StaffAttachments");

            migrationBuilder.DropTable(
                name: "StaffTreasuries",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "StoreCounts",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "SupplyReceipts",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "TreasuryOperations",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Vacations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VacationTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Banks",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "MaterialIssuePermissions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "ReceiptPermissions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "AccountTrees",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "CostCenterTree",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "TreasuryMovements",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "DisbursementRequests",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "MedicalServices");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "ItemGroups",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "ItemTypes",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "ItemUnits",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "DailyRestrictions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "JobLevels");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.DropTable(
                name: "Treasuries",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "InsuranceCategory");

            migrationBuilder.DropTable(
                name: "MainGroups",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "AccountingGuidance",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "RestrictionTypes",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "CostCenters",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "JobDepartment");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "InsuranceCompany");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "StoreTypes",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PriceQuotations",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PurchaseRequests",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "finance");
        }
    }
}
