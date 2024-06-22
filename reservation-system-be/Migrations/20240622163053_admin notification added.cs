using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class adminnotificationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "VehicleInsuranceID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "VehicleMaintenanceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Feedbacks");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AdminNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Generated_DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotifications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminNotifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

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

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_VehicleInsuranceID",
                table: "Notifications",
                column: "VehicleInsuranceID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_VehicleMaintenanceId",
                table: "Notifications",
                column: "VehicleMaintenanceId");

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
    }
}
