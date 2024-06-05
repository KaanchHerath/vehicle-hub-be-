using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class customerreservationchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerReservations_CustomerId",
                table: "CustomerReservations");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReservations_VehicleId",
                table: "CustomerReservations");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReservations_CustomerId",
                table: "CustomerReservations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReservations_VehicleId",
                table: "CustomerReservations",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerReservations_CustomerId",
                table: "CustomerReservations");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReservations_VehicleId",
                table: "CustomerReservations");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReservations_CustomerId",
                table: "CustomerReservations",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReservations_VehicleId",
                table: "CustomerReservations",
                column: "VehicleId",
                unique: true);
        }
    }
}
