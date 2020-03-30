using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class migration23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsideredId",
                table: "UserServey");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "UserServey");

            migrationBuilder.AddColumn<int>(
                name: "ConsideredUserId",
                table: "UserServey",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoterUserId",
                table: "UserServey",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsideredUserId",
                table: "UserServey");

            migrationBuilder.DropColumn(
                name: "VoterUserId",
                table: "UserServey");

            migrationBuilder.AddColumn<int>(
                name: "ConsideredId",
                table: "UserServey",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoterId",
                table: "UserServey",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
