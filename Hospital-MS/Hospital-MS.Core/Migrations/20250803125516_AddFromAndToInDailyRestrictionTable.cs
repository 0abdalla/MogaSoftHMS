using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFromAndToInDailyRestrictionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "From",
                schema: "finance",
                table: "DailyRestrictionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                schema: "finance",
                table: "DailyRestrictionDetails",
                type: "nvarchar(max)",
                nullable: true);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "From",
                schema: "finance",
                table: "DailyRestrictionDetails");

            migrationBuilder.DropColumn(
                name: "To",
                schema: "finance",
                table: "DailyRestrictionDetails");


        }
    }
}
