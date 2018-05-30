using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFC_DA.Migrations
{
 public partial class Kardinaliaet11wird1N : Migration
 {
  protected override void Up(MigrationBuilder migrationBuilder)
  {
   // First create a new column on the N-side
   migrationBuilder.AddColumn<int>(
       name: "PassengerPersonID",
       table: "Persondetail",
       nullable: true);

   // Now copy the values from the 1-side
   migrationBuilder.Sql("update Persondetail set PassengerPersonID = Passenger.PersonID FROM Passenger INNER JOIN Persondetail ON Passenger.DetailID = Persondetail.ID");

   // Then delete the column on the 1-side first
   migrationBuilder.DropForeignKey(
       name: "FK_Passenger_Persondetail_DetailID",
       table: "Passenger");

   migrationBuilder.DropIndex(
       name: "IX_Passenger_DetailID",
       table: "Passenger");

   migrationBuilder.DropColumn(
       name: "DetailID",
       table: "Passenger");

   // Then create index and FK for new column
   migrationBuilder.CreateIndex(
       name: "IX_Persondetail_PassengerPersonID",
       table: "Persondetail",
       column: "PassengerPersonID");

   migrationBuilder.AddForeignKey(
       name: "FK_Persondetail_Passenger_PassengerPersonID",
       table: "Persondetail",
       column: "PassengerPersonID",
       principalTable: "Passenger",
       principalColumn: "PersonID",
       onDelete: ReferentialAction.Restrict);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
   migrationBuilder.DropForeignKey(
       name: "FK_Persondetail_Passagier_PassagierPersonID",
       table: "Persondetail");

   migrationBuilder.DropForeignKey(
       name: "FK_Persondetail_Passenger_PassengerPersonID",
       table: "Persondetail");

   migrationBuilder.DropIndex(
       name: "IX_Persondetail_PassagierPersonID",
       table: "Persondetail");

   migrationBuilder.DropIndex(
       name: "IX_Persondetail_PassengerPersonID",
       table: "Persondetail");

   migrationBuilder.DropColumn(
       name: "PassagierPersonID",
       table: "Persondetail");

   migrationBuilder.DropColumn(
       name: "PassengerPersonID",
       table: "Persondetail");

   migrationBuilder.DropColumn(
       name: "Stadt",
       table: "Persondetail");

   migrationBuilder.AddColumn<int>(
       name: "DetailID",
       table: "Passenger",
       nullable: true);

   migrationBuilder.AddColumn<string>(
       name: "Ort",
       table: "Persondetail",
       maxLength: 30,
       nullable: true);

   migrationBuilder.AddColumn<int>(
       name: "DetailID",
       table: "Passagier",
       nullable: true);

   migrationBuilder.CreateIndex(
       name: "IX_Passenger_DetailID",
       table: "Passenger",
       column: "DetailID");

   migrationBuilder.AlterColumn<string>(
       name: "Plz",
       table: "Persondetail",
       maxLength: 8,
       nullable: true);

   migrationBuilder.CreateIndex(
       name: "IX_Passagier_DetailID",
       table: "Passagier",
       column: "DetailID");

   migrationBuilder.AddForeignKey(
       name: "FK_Passagier_Persondetail_DetailID",
       table: "Passagier",
       column: "DetailID",
       principalTable: "Persondetail",
       principalColumn: "ID",
       onDelete: ReferentialAction.Restrict);

   migrationBuilder.AddForeignKey(
       name: "FK_Passenger_Persondetail_DetailID",
       table: "Passenger",
       column: "DetailID",
       principalTable: "Persondetail",
       principalColumn: "ID",
       onDelete: ReferentialAction.Restrict);
  }
 }
}
