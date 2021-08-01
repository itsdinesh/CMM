using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMM.Migrations
{
    public partial class addEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    ConcertID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcertPoster = table.Column<string>(nullable: true),
                    ConcertMusician = table.Column<string>(maxLength: 250, nullable: false),
                    ConcertLink = table.Column<string>(nullable: false),
                    ConcertName = table.Column<string>(maxLength: 250, nullable: false),
                    ConcertDescription = table.Column<string>(maxLength: 1000, nullable: false),
                    ConcertDateTime = table.Column<DateTime>(nullable: false),
                    ConcertPrice = table.Column<decimal>(nullable: false),
                    TicketLimit = table.Column<int>(nullable: false),
                    ConcertStatus = table.Column<string>(nullable: false),
                    ConcertVisibility = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.ConcertID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
