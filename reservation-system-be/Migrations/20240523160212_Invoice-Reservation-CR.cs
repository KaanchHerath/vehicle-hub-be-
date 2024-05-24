using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceReservationCR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Reservations_ReservationId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalFeatures_VehicleModelId",
                table: "AdditionalFeatures");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Invoices",
                newName: "CustomerReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ReservationId",
                table: "Invoices",
                newName: "IX_Invoices_CustomerReservationId");

            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "Invoices",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "Feedbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                unique: true,
                filter: "[ReservationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFeatures_VehicleModelId",
                table: "AdditionalFeatures",
                column: "VehicleModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CustomerReservations_CustomerReservationId",
                table: "Invoices",
                column: "CustomerReservationId",
                principalTable: "CustomerReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CustomerReservations_CustomerReservationId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalFeatures_VehicleModelId",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "CustomerReservationId",
                table: "Invoices",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CustomerReservationId",
                table: "Invoices",
                newName: "IX_Invoices_ReservationId");

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFeatures_VehicleModelId",
                table: "AdditionalFeatures",
                column: "VehicleModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Reservations_ReservationId",
                table: "Feedbacks",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Reservations_ReservationId",
                table: "Invoices",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
