using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class CustomerNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Reservations_ReservationId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Notifications",
                newName: "CustomerReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ReservationId",
                table: "Notifications",
                newName: "IX_Notifications_CustomerReservationId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_CustomerReservations_CustomerReservationId",
                table: "Notifications",
                column: "CustomerReservationId",
                principalTable: "CustomerReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_CustomerReservations_CustomerReservationId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "CustomerReservationId",
                table: "Notifications",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CustomerReservationId",
                table: "Notifications",
                newName: "IX_Notifications_ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Reservations_ReservationId",
                table: "Notifications",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
