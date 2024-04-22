using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class VehiclevehicleMaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleModels_VehicleMakes_VehicleMakeId",
                table: "VehicleModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleMaintenances_VehicleMaintenanceId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "MaintenanceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleMaintenanceId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleMakes",
                table: "VehicleMakes");

            migrationBuilder.DropColumn(
                name: "VehicleMaintenanceId",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "VehicleMakes",
                newName: "Vehicle");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "VehicleMaintenances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "VehicleMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaintenances_VehicleId",
                table: "VehicleMaintenances",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleMaintenances_Vehicles_VehicleId",
                table: "VehicleMaintenances",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleModels_Vehicle_VehicleMakeId",
                table: "VehicleModels",
                column: "VehicleMakeId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleMaintenances_Vehicles_VehicleId",
                table: "VehicleMaintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleModels_Vehicle_VehicleMakeId",
                table: "VehicleModels");

            migrationBuilder.DropIndex(
                name: "IX_VehicleMaintenances_VehicleId",
                table: "VehicleMaintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "VehicleMaintenances");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "VehicleMaintenances");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "VehicleMakes");

            migrationBuilder.AddColumn<int>(
                name: "VehicleMaintenanceId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleMakes",
                table: "VehicleMakes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MaintenanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleMaintenanceId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceTypes_VehicleMaintenances_VehicleMaintenanceId",
                        column: x => x.VehicleMaintenanceId,
                        principalTable: "VehicleMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleMaintenanceId",
                table: "Vehicles",
                column: "VehicleMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTypes_VehicleMaintenanceId",
                table: "MaintenanceTypes",
                column: "VehicleMaintenanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleModels_VehicleMakes_VehicleMakeId",
                table: "VehicleModels",
                column: "VehicleMakeId",
                principalTable: "VehicleMakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleMaintenances_VehicleMaintenanceId",
                table: "Vehicles",
                column: "VehicleMaintenanceId",
                principalTable: "VehicleMaintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
