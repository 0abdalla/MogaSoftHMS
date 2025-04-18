using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomToBed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Wards_WardId",
                table: "Beds");

            migrationBuilder.DropIndex(
                name: "IX_Beds_WardId",
                table: "Beds");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Beds");

            migrationBuilder.RenameColumn(
                name: "BedNumber",
                table: "Beds",
                newName: "Number");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Beds",
                type: "nvarchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Beds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Beds",
                newName: "BedNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Beds",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(55)",
                oldMaxLength: 55);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Beds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WardId",
                table: "Beds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Beds_WardId",
                table: "Beds",
                column: "WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Wards_WardId",
                table: "Beds",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
