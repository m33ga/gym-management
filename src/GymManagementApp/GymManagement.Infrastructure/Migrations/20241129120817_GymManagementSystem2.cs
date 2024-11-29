using Microsoft.EntityFrameworkCore.Migrations;

namespace GymManagement.Infrastructure.Migrations
{
    public partial class GymManagementSystem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admins");
        }
    }
}
