using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DA.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Flight",
                maxLength: 50,
                nullable: true,
                defaultValue: "(not set)",
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "(offen)");

            migrationBuilder.AlterColumn<string>(
                name: "Departure",
                table: "Flight",
                maxLength: 50,
                nullable: true,
                defaultValue: "(not set)",
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "(offen)");

            migrationBuilder.CreateTable(
                name: "DepartureGrouping",
                columns: table => new
                {
                    Departure = table.Column<string>(nullable: false),
                    FlightCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartureGrouping", x => x.Departure);
                });

            migrationBuilder.CreateTable(
                name: "V_DepartureStatistics",
                columns: table => new
                {
                    Departure = table.Column<string>(nullable: false),
                    FlightCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_V_DepartureStatistics", x => x.Departure);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartureGrouping");

            migrationBuilder.DropTable(
                name: "V_DepartureStatistics");

            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Flight",
                maxLength: 50,
                nullable: true,
                defaultValue: "(offen)",
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "(not set)");

            migrationBuilder.AlterColumn<string>(
                name: "Departure",
                table: "Flight",
                maxLength: 50,
                nullable: true,
                defaultValue: "(offen)",
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "(not set)");
        }
    }
}
