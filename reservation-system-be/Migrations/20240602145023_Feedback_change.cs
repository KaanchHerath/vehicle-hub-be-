using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class Feedback_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Feedbacks",
                newName: "Vehicle_Review");

            migrationBuilder.AddColumn<int>(
                name: "CustomerReservationId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Service_Review",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CustomerReservationId",
                table: "Feedbacks",
                column: "CustomerReservationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_CustomerReservations_CustomerReservationId",
                table: "Feedbacks",
                column: "CustomerReservationId",
                principalTable: "CustomerReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_CustomerReservations_CustomerReservationId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CustomerReservationId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CustomerReservationId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Service_Review",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Vehicle_Review",
                table: "Feedbacks",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                unique: true,
                filter: "[ReservationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
