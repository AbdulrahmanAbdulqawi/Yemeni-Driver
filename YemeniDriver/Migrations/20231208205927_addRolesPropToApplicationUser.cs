using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemeniDriver.Migrations
{
    /// <inheritdoc />
    public partial class addRolesPropToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Roles",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "AspNetUsers");
        }
    }
}
