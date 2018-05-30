using DA;
using BO;
using System;
using System.Linq;

namespace EFC_Console
{
 class SampleClientForward
 {
  public static void Run()
  {
   Console.WriteLine("Start...");
   using (var ctx = new WWWingsContext())
   {
    // Datenbank anlegen, wenn nicht vorhanden!
    ctx.Database.EnsureCreated();
    // Passagierobjekt erzeugen
    var newPassenger = new Passenger();
    newPassenger.GivenName = "Holger";
    newPassenger.Surname = "Schwichtenberg";
    // Passenger an EF-Kontext anfügen
    ctx.PassengerSet.Add(newPassenger);
    // Objekt speichern
    var anz = ctx.SaveChanges();
    Console.WriteLine("Number of changes: " + anz);
    // All Passagiere einlesen aus Datenbank
    var passengerSet = ctx.PassengerSet.ToList();
    Console.WriteLine("Number of passengers: " + passengerSet.Count);
    // Filtern mit LINQ-to-Objects 
    foreach (var p in passengerSet.Where(x => x.Surname == "Schwichtenberg").ToList())
    {
     Console.WriteLine(p.PersonID + ": " + p.GivenName + " " + p.Surname);
    }
   }
   Console.WriteLine("Ende!");
   Console.ReadLine();
  }

 }
}
