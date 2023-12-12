using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemeniDriver.Migrations
{
    /// <inheritdoc />
    public partial class makeApplicationUserIdNotUniqueInTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_ApplicationUserId",
                table: "Trips");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ApplicationUserId",
                table: "Trips",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_ApplicationUserId",
                table: "Trips");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ApplicationUserId",
                table: "Trips",
                column: "ApplicationUserId",
                unique: true);
        }
    }
}
