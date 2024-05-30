using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class Vehicle_statusvehiclelog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraDays",
                table: "VehicleLogs");

            migrationBuilder.RenameColumn(
                name: "ExtraCostPerKM",
                table: "Vehicles",
                newName: "CostPerExtraKM");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "CostPerExtraKM",
                table: "Vehicles",
                newName: "ExtraCostPerKM");

            migrationBuilder.AddColumn<int>(
                name: "ExtraDays",
                table: "VehicleLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
