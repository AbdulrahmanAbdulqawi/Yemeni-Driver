using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yemeni_Driver.Migrations
{
    /// <inheritdoc />
    public partial class addFieldsForLiveLocationInApplicationUesr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LiveLocationLatitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LiveLocationLongitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiveLocationLatitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LiveLocationLongitude",
                table: "AspNetUsers");
        }
    }
}
