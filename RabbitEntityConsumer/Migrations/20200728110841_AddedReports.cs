using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RabbitEntityConsumer.Migrations
{
    public partial class AddedReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XMLwithOpenXML");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportData = table.Column<string>(type: "xml", nullable: true),
                    RequestedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    AddedToDatabase = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.CreateTable(
                name: "XMLwithOpenXML",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    XMLData = table.Column<string>(type: "xml", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XMLwithOpenXML", x => x.Id);
                });
        }
    }
}
