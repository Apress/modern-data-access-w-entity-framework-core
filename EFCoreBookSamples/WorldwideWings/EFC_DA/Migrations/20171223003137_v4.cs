using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DA.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartureGrouping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Booking_FlightNo_PassengerID",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Flight");

            migrationBuilder.RenameColumn(
                name: "FlugzeugtypdetailID",
                table: "AircraftType",
                newName: "AircraftTypeDetailAircraftTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                columns: new[] { "FlightNo", "PassengerID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "AircraftTypeDetailAircraftTypeID",
                table: "AircraftType",
                newName: "FlugzeugtypdetailID");

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Flight",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "FlightNo");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Booking_FlightNo_PassengerID",
                table: "Booking",
                columns: new[] { "FlightNo", "PassengerID" });

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
        }
    }
}
