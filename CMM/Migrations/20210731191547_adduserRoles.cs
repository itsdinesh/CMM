using Microsoft.EntityFrameworkCore.Migrations;

namespace CMM.Migrations
{
    public partial class adduserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userRoles",
                schema: "Identity",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userRoles",
                schema: "Identity",
                table: "AspNetUsers");
        }
    }
}
