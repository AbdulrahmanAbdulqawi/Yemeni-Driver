using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yemeni_Driver.Migrations
{
    /// <inheritdoc />
    public partial class updateRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RideType",
                table: "Requests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Requests",
                newName: "PickupTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Requests",
                newName: "RideType");

            migrationBuilder.RenameColumn(
                name: "PickupTime",
                table: "Requests",
                newName: "DateTime");
        }
    }
}
