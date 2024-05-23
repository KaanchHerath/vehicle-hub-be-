using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_system_be.Migrations
{
    /// <inheritdoc />
    public partial class additionalfeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Features",
                table: "AdditionalFeatures");

            migrationBuilder.AddColumn<bool>(
                name: "ABS",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AcFront",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AirbagDriver",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AirbagPassenger",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AirbagSide",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AlloyWheels",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutomaticHeadlights",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bluetooth",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ElectricMirrors",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FogLights",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "KeylessEntry",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NavigationSystem",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ParkingSensor",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PowerWindow",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RearWindowWiper",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SecuritySystem",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sunroof",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TintedGlass",
                table: "AdditionalFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ABS",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "AcFront",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "AirbagDriver",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "AirbagPassenger",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "AirbagSide",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "AlloyWheels",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "AutomaticHeadlights",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "Bluetooth",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "ElectricMirrors",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "FogLights",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "KeylessEntry",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "NavigationSystem",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "ParkingSensor",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "PowerWindow",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "RearWindowWiper",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "SecuritySystem",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "Sunroof",
                table: "AdditionalFeatures");

            migrationBuilder.DropColumn(
                name: "TintedGlass",
                table: "AdditionalFeatures");

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "AdditionalFeatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
