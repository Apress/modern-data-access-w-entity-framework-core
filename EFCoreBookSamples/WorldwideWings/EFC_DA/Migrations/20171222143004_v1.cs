using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DA.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AircraftType",
                columns: table => new
                {
                    TypeID = table.Column<byte>(nullable: false),
                    FlugzeugtypdetailID = table.Column<int>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftType", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Persondetail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(maxLength: 30, nullable: true),
                    Country = table.Column<string>(maxLength: 3, nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    Street = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persondetail", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AircraftTypeDetail",
                columns: table => new
                {
                    AircraftTypeID = table.Column<byte>(nullable: false),
                    Length = table.Column<float>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Tare = table.Column<short>(nullable: true),
                    TurbineCount = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftTypeDetail", x => x.AircraftTypeID);
                    table.ForeignKey(
                        name: "FK_AircraftTypeDetail_AircraftType_AircraftTypeID",
                        column: x => x.AircraftTypeID,
                        principalTable: "AircraftType",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthday = table.Column<DateTime>(nullable: true),
                    DetailID = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    EMail = table.Column<string>(nullable: true),
                    GivenName = table.Column<string>(nullable: true),
                    PassportNumber = table.Column<string>(nullable: true),
                    Salary = table.Column<float>(nullable: false),
                    SupervisorPersonID = table.Column<int>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    FlightHours = table.Column<int>(nullable: true),
                    FlightSchool = table.Column<string>(maxLength: 50, nullable: true),
                    LicenseDate = table.Column<DateTime>(nullable: true),
                    PilotLicenseType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_Employee_Persondetail_DetailID",
                        column: x => x.DetailID,
                        principalTable: "Persondetail",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Employee_SupervisorPersonID",
                        column: x => x.SupervisorPersonID,
                        principalTable: "Employee",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthday = table.Column<DateTime>(nullable: true),
                    CustomerSince = table.Column<DateTime>(nullable: true),
                    DetailID = table.Column<int>(nullable: true),
                    EMail = table.Column<string>(nullable: true),
                    GivenName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 1, nullable: true),
                    Surname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_Passenger_Persondetail_DetailID",
                        column: x => x.DetailID,
                        principalTable: "Persondetail",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightNo = table.Column<int>(nullable: false),
                    AircraftTypeID = table.Column<byte>(nullable: true),
                    AirlineCode = table.Column<string>(nullable: true),
                    CopilotId = table.Column<int>(nullable: true),
                    FlightDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Departure = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "(offen)"),
                    Destination = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "(offen)"),
                    FreeSeats = table.Column<short>(nullable: true),
                    LastChange = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(maxLength: 5000, nullable: true),
                    NonSmokingFlight = table.Column<bool>(nullable: true),
                    PilotId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: true, defaultValue: 123.45m),
                    Seats = table.Column<short>(nullable: false),
                    Strikebound = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Utilization = table.Column<decimal>(nullable: true, computedColumnSql: "100.0-(([FreeSeats]*1.0)/[Seats])*100.0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightNo);
                    table.ForeignKey(
                        name: "FK_Flight_AircraftType_AircraftTypeID",
                        column: x => x.AircraftTypeID,
                        principalTable: "AircraftType",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flight_Airline_AirlineCode",
                        column: x => x.AirlineCode,
                        principalTable: "Airline",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flight_Employee_CopilotId",
                        column: x => x.CopilotId,
                        principalTable: "Employee",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flight_Employee_PilotId",
                        column: x => x.PilotId,
                        principalTable: "Employee",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    FlightNo = table.Column<int>(nullable: false),
                    PassengerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => new { x.FlightNo, x.PassengerID });
                    table.UniqueConstraint("AK_Booking_FlightNo_PassengerID", x => new { x.FlightNo, x.PassengerID });
                    table.ForeignKey(
                        name: "FK_Booking_Flight_FlightNo",
                        column: x => x.FlightNo,
                        principalTable: "Flight",
                        principalColumn: "FlightNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Passenger_PassengerID",
                        column: x => x.PassengerID,
                        principalTable: "Passenger",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PassengerID",
                table: "Booking",
                column: "PassengerID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DetailID",
                table: "Employee",
                column: "DetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SupervisorPersonID",
                table: "Employee",
                column: "SupervisorPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AircraftTypeID",
                table: "Flight",
                column: "AircraftTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AirlineCode",
                table: "Flight",
                column: "AirlineCode");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_CopilotId",
                table: "Flight",
                column: "CopilotId");

            migrationBuilder.CreateIndex(
                name: "Index_FreeSeats",
                table: "Flight",
                column: "FreeSeats");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_PilotId",
                table: "Flight",
                column: "PilotId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Departure_Destination",
                table: "Flight",
                columns: new[] { "Departure", "Destination" });

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_DetailID",
                table: "Passenger",
                column: "DetailID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftTypeDetail");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Passenger");

            migrationBuilder.DropTable(
                name: "AircraftType");

            migrationBuilder.DropTable(
                name: "Airline");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Persondetail");
        }
    }
}
