using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yemeni_Driver.Migrations
{
    /// <inheritdoc />
    public partial class intia2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassengerId",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassengerId",
                table: "DriversAndRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicenseNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TripId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PassengerId",
                table: "Vehicles",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DriverId",
                table: "Requests",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriversAndRequests_PassengerId",
                table: "DriversAndRequests",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriversAndRequests_AspNetUsers_PassengerId",
                table: "DriversAndRequests",
                column: "PassengerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_DriverId",
                table: "Requests",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_PassengerId",
                table: "Vehicles",
                column: "PassengerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trips_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DriversAndRequests_AspNetUsers_PassengerId",
                table: "DriversAndRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_DriverId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_PassengerId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_PassengerId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Requests_DriverId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_DriversAndRequests_PassengerId",
                table: "DriversAndRequests");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TripId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "DriversAndRequests");

            migrationBuilder.DropColumn(
                name: "DrivingLicenseNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "AspNetUsers");
        }
    }
}
