using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DA.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Passenger",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FrequentFlyer",
                table: "Passenger",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PilotLicenseType",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PilotLicenseType2",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrequentFlyer",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "PilotLicenseType2",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Passenger",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<int>(
                name: "PilotLicenseType",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
