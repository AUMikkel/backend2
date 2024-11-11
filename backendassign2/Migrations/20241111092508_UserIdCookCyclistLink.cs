using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendassign2.Migrations
{
    /// <inheritdoc />
    public partial class UserIdCookCyclistLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DeliveryDrivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Cooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DeliveryDrivers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Cooks");
        }
    }
}
