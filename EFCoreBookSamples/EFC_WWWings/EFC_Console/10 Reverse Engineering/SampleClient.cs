using System;
using System.Linq;
using EFC_WWWings1_Reverse_NETFX_;
using Microsoft.EntityFrameworkCore;

namespace EFC_Console
{
 class SampleClientReverse
 {
  public static void Run()
  {
   Console.WriteLine("Start...");
   using (var ctx = new WWWingsV1Context())
   {
    // Create Person object
    var newPerson = new Person();
    newPerson.GivenName = "Holger";
    newPerson.Surname = "Schwichtenberg";
    // Create Passenger object
    var newPassenger = new Passenger();
    newPassenger.PassengerStatus = "A";
    newPassenger.Person = newPerson;
    // Add Passenger to Context
    ctx.Passenger.Add(newPassenger);
    // Save objects
    var count = ctx.SaveChanges();
    Console.WriteLine("Number of changes: " + count);
    // Get all passengers from the database
    var passengerSet = ctx.Passenger.Include(x => x.Person).ToList();
    Console.WriteLine("Number of passengers: " + passengerSet.Count);
    // Filter with LINQ-to-Objects 
    foreach (var p in passengerSet.Where(x=>x.Person.Surname == "Schwichtenberg").ToList())
    {
     Console.WriteLine(p.PersonId + ": " + p.Person.GivenName + " " + p.Person.Surname);
    }
   }
   Console.WriteLine("Done!");
   Console.ReadLine();
  }

  //public static void AddFlight()
  //{
  // CUI.MainHeadline("Demo_EinFlight");

  // using (var ctx = new EFC_WWingsV1_Reverse.WWWingsV1Context()())
  // {

  //  ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo = 101");

  //  var pilot = new Employee();
  //  pilot.Surname = "testpilot";
  //  ctx.Mitarbeiter.Add(pilot);


  //  EFC_Reverse.Flight fneu = new Flight();
  //  fneu.FlightNo = 101;
  //  fneu.Pilot = pilot;
  //  fneu.Copilot = pilot;
  //  fneu.FreeSeats = 100;
  //  ctx.Flight.Add(fneu);
  //  ctx.SaveChanges();

  //  // hier funktioniert das Lazy Loading (noch nicht)
  //  // var buchung = ctx.BookingSet.FirstOrDefault();

  //  // Console.WriteLine(buchung.Passenger.FullName);
  //  //Console.WriteLine(buchung.Flight.ToString());


  //  var flight101 = ctx.Flight
  //   .Include(b => b.Buchung).ThenInclude(p => p.Passagier)
  //   .Include(b => b.Pilot) //.ThenInclude(p=>p.FlightAsPilotSet)
  //   .SingleOrDefault(x => x.FlightNo == 101);
  //  // Flightdaten ausgeben
  //  Console.WriteLine("Flight #" + flight101.FlightNo + " from " + flight101.Departure + " to " + flight101.Destination);
  //  // Pilot und Copilot ausgeben
  //  if (flight101.Pilot != null) Console.WriteLine("Pilot: " + flight101.Pilot.Surname);
  //  if (flight101.Copilot != null) Console.WriteLine("Copilot: " + flight101.Copilot.Surname);
  //  // All Passagiere ausgeben
  //  foreach (var b in flight101.Buchung)
  //  {
  //   Console.WriteLine(" - " + b.Passagier.Surname);
  //  }
  //  flight101.FreeSeats--;
  //  int anz = ctx.SaveChanges();
  //  if (anz != 1) Debugger.Break();
  // }
  //}

 }
}
