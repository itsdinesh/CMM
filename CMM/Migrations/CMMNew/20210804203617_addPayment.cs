using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMM.Migrations.CMMNew
{
    public partial class addPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<string>(nullable: true),
                    PaymentPrice = table.Column<decimal>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    ConcertID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");
        }
    }
}
