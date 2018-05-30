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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
