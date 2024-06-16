using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class vehicleInsuranceconvertedtoOnetoManyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleInsurances_VehicleId",
                table: "VehicleInsurances");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInsurances_VehicleId",
                table: "VehicleInsurances",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleInsurances_VehicleId",
                table: "VehicleInsurances");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInsurances_VehicleId",
                table: "VehicleInsurances",
                column: "VehicleId",
                unique: true);
        }
    }
}
