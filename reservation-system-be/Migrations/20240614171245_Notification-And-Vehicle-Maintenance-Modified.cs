using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class NotificationAndVehicleMaintenanceModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_CustomerReservations_CustomerReservationId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "VehiclePhoto");

            migrationBuilder.AddColumn<int>(
                name: "CurrentMileage",
                table: "VehicleMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerReservationId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VehicleInsuranceID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleMaintenanceId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_VehicleInsuranceID",
                table: "Notifications",
                column: "VehicleInsuranceID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_VehicleMaintenanceId",
                table: "Notifications",
                column: "VehicleMaintenanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_CustomerReservations_CustomerReservationId",
                table: "Notifications",
                column: "CustomerReservationId",
                principalTable: "CustomerReservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_VehicleInsurances_VehicleInsuranceID",
                table: "Notifications",
                column: "VehicleInsuranceID",
                principalTable: "VehicleInsurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_VehicleMaintenances_VehicleMaintenanceId",
                table: "Notifications",
                column: "VehicleMaintenanceId",
                principalTable: "VehicleMaintenances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_CustomerReservations_CustomerReservationId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_VehicleInsurances_VehicleInsuranceID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_VehicleMaintenances_VehicleMaintenanceId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_VehicleInsuranceID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_VehicleMaintenanceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CurrentMileage",
                table: "VehicleMaintenances");

            migrationBuilder.DropColumn(
                name: "VehicleInsuranceID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "VehicleMaintenanceId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerReservationId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "VehiclePhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePhoto_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePhoto_VehicleId",
                table: "VehiclePhoto",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_CustomerReservations_CustomerReservationId",
                table: "Notifications",
                column: "CustomerReservationId",
                principalTable: "CustomerReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
