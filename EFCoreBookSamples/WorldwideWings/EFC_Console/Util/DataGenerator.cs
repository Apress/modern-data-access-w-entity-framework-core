using System;
using System.IO;
using System.Linq;
using BO;
using DA;
using ITVisions;
using Microsoft.EntityFrameworkCore;

namespace EFC_Console
{
 /// <summary>
 /// Generates random database test data from common first and last names, and selected cities
 /// </summary>
 public class DataGenerator
 {
  static string[] Flighthaefen =
  {
   "Berlin", "Frankfurt", "Munich", "Hamburg", "Chicago", "Rome", "London", "Paris", "Milan", "Prague", "Moscow",
   "New York/JFC", "Seattle", "Berlin", "Kapstadt", "Madrid", "Oslo", "Dallas", "Vienna"
  };

  // Common German names
  // Source: http://de.wikipedia.org/wiki/Liste_der_h%C3%A4ufigsten_Familiennamen_in_Deutschland
  static string[] Nachnamen =
  {
   "Müller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Schulz", "Hoffmann", "Schäfer",
   "Koch", "Bauer", "Richter", "Klein", "Wolf", "Schröder", "Neumann", "Schwarz", "Zimmermann"
  };

  // Source: http://www.beliebte-vornamen.de/3467-alle-spitzenreiter.htm
  static string[] Vornamen =
   {"Leon", "Hannah", "Lukas", "Anna", "Leonie", "Marie", "Niklas", "Sarah", "Jan", "Laura", "Julia", "Lisa", "Kevin"};

  // Names of german politians :-)
  static string[] PilotsNachnamen =
  {
   "Gysi", "Seehofer", "Wagenknecht", "Steinmeyer", "Schulz", "Merkel", "Lindner", "Gabriel", "Lafontaine", "Özdemir",
   "Roth"
  };

  static string[] PilotsVornamen =
  {
   "Horst", "Sahra", "Martin", "Angela", "Joschka", "Christian", "Gregor", "Sigmar", "Frank-Walter", "Oskar", "Cem",
   "Claudia"
  };

  const int NO_OF_FLIGHTS = 1000;
  const int NO_OF_PASSENGERS = 2000;
  const int NO_OF_PILOTS = 50;

  static Random rnd = new Random(DateTime.Now.Millisecond);

  public static void Run(bool delete = false)
  {

   CUI.Print("Data Generator", ConsoleColor.Blue, ConsoleColor.White);

   WWWingsContext ctx = new WWWingsContext();

   Console.WriteLine("Number of Flights: " + ctx.FlightSet.Count());
   Console.WriteLine("Number of Pilots: " + ctx.PilotSet.Count());
   Console.WriteLine("Number of Passengers: " + ctx.PassengerSet.Count());
   Console.WriteLine("Number of Bookings: " + ctx.BookingSet.Count());

   // TODO: this does not work yet :-(
   //ctx.Database.ExecuteSqlCommand(File.ReadAllText(@"H:\TFS\Demos\EFC\EFC_Samples\SQL\WWWings70_CreateSP.sql").Replace("GO",""));
   //ctx.Database.ExecuteSqlCommand(File.ReadAllText(@"H:\TFS\Demos\EFC\EFC_Samples\SQL\WWWings70_CreateTVF.sql").Replace("GO", ""));
   //ctx.Database.ExecuteSqlCommand(File.ReadAllText(@"H:\TFS\Demos\EFC\EFC_Samples\SQL\WWWings70_CreateView.sql").Replace("GO", ""));

   if (ctx.FlightSet.Any() || ctx.PilotSet.Any() || ctx.PassengerSet.Any() || ctx.BookingSet.Any())
   {
    if (!delete) return;
    CUI.PrintWarning(
     "Database is not empty. Delete all data and create new records? y = yes, other key = no and exit program");
    var key = Console.ReadKey();
    if (key.Key != ConsoleKey.Y) Environment.Exit(1);
   }


   if (delete)
   {
    CUI.PrintSuccess("Deleting...");
    ctx.Database.ExecuteSqlCommand("Delete from Airline");
    ctx.Database.ExecuteSqlCommand("Delete from Booking");
    ctx.Database.ExecuteSqlCommand("Delete from Flight");
    ctx.Database.ExecuteSqlCommand("Delete from Passenger");
    ctx.Database.ExecuteSqlCommand("Delete from Employee");
    ctx.Database.ExecuteSqlCommand("Delete from Persondetail");
    ctx.Database.ExecuteSqlCommand("Delete from AircraftType");
    ctx.Database.ExecuteSqlCommand("Delete from AircraftTypeDetail");
   }
   Console.WriteLine("Number of Flights: " + ctx.FlightSet.Count());
   Console.WriteLine("Number of Pilots: " + ctx.PilotSet.Count());
   Console.WriteLine("Number of Passengers: " + ctx.PassengerSet.Count());
   Console.WriteLine("Number of Bookings: " + ctx.BookingSet.Count());

   var act = new AircraftType();
   act.TypeID = 1;
   act.Manufacturer = "Airbus";
   act.Name = "A380-800";
   ctx.AircraftTypeSet.Add(act);

   Init_AirlineSet(ctx);
   Init_PilotSet(ctx);
   Init_FlightSet(ctx);
   Init_PassengerSet(ctx);

 
   Init_BookingSet();



   CUI.PrintSuccess("=== End of data generation!");
  }


  /// <summary>
  /// 
  /// </summary>
  private static void Init_AirlineSet(WWWingsContext ctx)
  {
   Console.WriteLine("Create 2 airlines...");

   var a1 = new Airline() { Code = "WWW", Name = "World Wide Wings" };
   var a2 = new Airline() { Code = "NCB", Name = "Never Come Back Airline" };
   ctx.AirlineSet.Add(a1);
   ctx.AirlineSet.Add(a2);
   ctx.SaveChanges();
   CUI.PrintSuccess("Number of airlines: " + ctx.FlightSet.Count());
  }


  /// <summary>
  /// Create random flights
  /// </summary>
  private static Random Init_FlightSet(WWWingsContext ctx)
  {
   Console.WriteLine("Create Flights...");

   var flightSet = from f in ctx.FlightSet select f;
   int Start = 100;
   Pilot[] PilotArray = ctx.PilotSet.ToArray();

   for (int i = Start; i < (Start + NO_OF_FLIGHTS); i++)
   {
    if (i % 100 == 0)
    {
     Console.WriteLine("Flight #" + i);
     try
     {
      ctx.SaveChanges();
     }
     catch (Exception ex)
     {
      Console.WriteLine("ERROR: " + ex.Message.ToString());
     }
    }
    Flight flightNew = new Flight();
    flightNew.FlightNo = i;
    flightNew.Departure = Flighthaefen[rnd.Next(0, Flighthaefen.Length - 1)];
    flightNew.Destination = Flighthaefen[rnd.Next(0, Flighthaefen.Length - 1)];
    ;
    if (flightNew.Departure == flightNew.Destination)
    {
     i--;
     continue;
    } // Keine Rundflüge!
    flightNew.FreeSeats = Convert.ToInt16(new Random(i).Next(250));
    flightNew.Seats = 250;
    flightNew.AircraftTypeID = 1;
    flightNew.Date = DateTime.Now.AddDays((double)flightNew.FreeSeats).AddMinutes((double)flightNew.FreeSeats * 7);
    flightNew.PilotId = PilotArray[rnd.Next(PilotArray.Count() - 1)].PersonID;
    flightNew.CopilotId = PilotArray[rnd.Next(PilotArray.Count() - 1)].PersonID;
    ctx.FlightSet.Add(flightNew);
   }
   ctx.SaveChanges();
   CUI.PrintSuccess("Number of flights: " + ctx.FlightSet.Count());


   var f101 = ctx.FlightSet.Find(101);
   f101.Departure = "Berlin";
   f101.Destination = "Seattle";
   ctx.SaveChanges();


   return rnd;
  }

  private static void Init_BookingSet()
  {
   WWWingsContext ctx = new WWWingsContext();
   int k = 0;
   Random rnd5 = new Random();
   Console.WriteLine("Create bookings...");
   var flightSet2 = from f in ctx.FlightSet select f;

   Passenger[] passengerArray = ctx.PassengerSet.ToArray();
   foreach (Flight f in flightSet2.ToList())
   {
    k++;
    if (k % 100 == 0) Console.WriteLine(k);

    Console.WriteLine("Creating bookings for flight " + f.FlightNo);
    for (int j = 1; j < 10; j++)
    {
     Random rnd2 = new Random(DateTime.Now.Millisecond);
     // Randomly choose a passenger
     Passenger p = passengerArray[Convert.ToInt16(rnd5.Next(passengerArray.Count()))];

     if (!ctx.BookingSet.Any(b => b.FlightNo == f.FlightNo && b.PassengerID == p.PersonID))
     {
      Booking b = null;
      try 
      {
       b = new Booking();
       b.FlightNo = f.FlightNo;
       b.PassengerID = p.PersonID;
       // Add booking 
       ctx.BookingSet.Add(b);
       ctx.SaveChanges(); 
      }
      catch (Exception ex)
      {
       Console.WriteLine(ex.Message);
       f.BookingSet.Remove(b);
      }
     }
    }
   }
   CUI.PrintSuccess("Number of bookings: " + ctx.BookingSet.Count());
  }

  private static void Init_PilotSet(WWWingsContext ctx)
  {
   Console.WriteLine("Create pilots...");

   for (int PNummer = 1; PNummer <= NO_OF_PILOTS; PNummer++)
   {
    string Vorname = PilotsVornamen[rnd.Next(0, PilotsVornamen.Length - 1)];
    ;
    string Nachname = PilotsNachnamen[rnd.Next(0, PilotsNachnamen.Length - 1)];
    ;
    Pilot p = new Pilot();
    //p.PersonID = 1000 + PNummer;
    p.Surname = Nachname;
    p.GivenName = Vorname;
    p.Birthday =
     new DateTime(1940, 1, 1).AddDays(Convert.ToInt32(new Random(DateTime.Now.Millisecond).Next(20000)));
    //p.HireDate = DateTime.Now.AddDays(-rnd.Next(365));
    ctx.PilotSet.Add(p);
    ctx.SaveChanges();

    Console.WriteLine("Pilot #" + PNummer + ": " + Vorname + " " + Nachname);
   }

   CUI.PrintSuccess("Number of pilots: " + ctx.PilotSet.Count());
  }

  private static void Init_PassengerSet(WWWingsContext ctx)
  {
   Console.WriteLine("Create Passengers...");

   Random rnd2 = new Random();

   for (int PNummer = 1; PNummer <= NO_OF_PASSENGERS; PNummer++)
   {
    string Vorname = Vornamen[rnd2.Next(0, Vornamen.Length - 1)];
    ;
    string Nachname = Nachnamen[rnd2.Next(0, Nachnamen.Length - 1)];
    ;
    Passenger p = new Passenger();
    //  p.PersonID = 1;
    p.Surname = Nachname;
    p.GivenName = Vorname;
    p.Birthday =
     new DateTime(1940, 1, 1).AddDays(Convert.ToInt32(new Random(DateTime.Now.Millisecond).Next(20000)));
    //p.PersonID = PNummer;

    p.Status = PassengerStatus.PassengerStatusSet.ElementAt(rnd.Next(3));
    ctx.PassengerSet.Add(p);
    ctx.SaveChanges();

    if (PNummer % 100 == 0)
    {
     Console.WriteLine("Passenger #" + PNummer + ": " + Vorname + " " + Nachname);
    }
   }
   CUI.PrintSuccess("Number of passengers: " + ctx.PassengerSet.Count());
  }
 }
}