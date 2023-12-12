using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemeniDriver.Migrations
{
    /// <inheritdoc />
    public partial class addPassengerIdPropToRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassengerID",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassengerID",
                table: "Requests");
        }
    }
}
