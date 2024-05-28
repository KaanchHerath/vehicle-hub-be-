using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class Payment_related_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLogs_Reservations_ReservationId",
                table: "VehicleLogs");

            migrationBuilder.DropIndex(
                name: "IX_VehicleLogs_ReservationId",
                table: "VehicleLogs");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "VehicleLogs",
                newName: "ExtraKM");

            migrationBuilder.AddColumn<float>(
                name: "ExtraCostPerKM",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<int>(
                name: "Penalty",
                table: "VehicleLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExtraDays",
                table: "VehicleLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VehicleLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerReservationId",
                table: "VehicleLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLogs_CustomerReservationId",
                table: "VehicleLogs",
                column: "CustomerReservationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLogs_CustomerReservations_CustomerReservationId",
                table: "VehicleLogs",
                column: "CustomerReservationId",
                principalTable: "CustomerReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLogs_CustomerReservations_CustomerReservationId",
                table: "VehicleLogs");

            migrationBuilder.DropIndex(
                name: "IX_VehicleLogs_CustomerReservationId",
                table: "VehicleLogs");

            migrationBuilder.DropColumn(
                name: "ExtraCostPerKM",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CustomerReservationId",
                table: "VehicleLogs");

            migrationBuilder.RenameColumn(
                name: "ExtraKM",
                table: "VehicleLogs",
                newName: "ReservationId");

            migrationBuilder.AlterColumn<int>(
                name: "Penalty",
                table: "VehicleLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ExtraDays",
                table: "VehicleLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VehicleLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLogs_ReservationId",
                table: "VehicleLogs",
                column: "ReservationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLogs_Reservations_ReservationId",
                table: "VehicleLogs",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
