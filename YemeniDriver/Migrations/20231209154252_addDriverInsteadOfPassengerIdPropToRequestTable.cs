using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemeniDriver.Migrations
{
    /// <inheritdoc />
    public partial class addDriverInsteadOfPassengerIdPropToRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassengerID",
                table: "Requests",
                newName: "DriverID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriverID",
                table: "Requests",
                newName: "PassengerID");
        }
    }
}
