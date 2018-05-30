using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DA.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AircraftTypeDetailAircraftTypeID",
                table: "AircraftType",
                newName: "DetailAircraftTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DetailAircraftTypeID",
                table: "AircraftType",
                newName: "AircraftTypeDetailAircraftTypeID");
        }
    }
}
