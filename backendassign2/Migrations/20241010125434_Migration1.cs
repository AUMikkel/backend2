using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendassign2.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPassedFoodSafetyCourse",
                table: "Cooks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPassedFoodSafetyCourse",
                table: "Cooks");
        }
    }
}
