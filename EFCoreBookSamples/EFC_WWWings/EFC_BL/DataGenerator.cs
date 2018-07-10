using BO;
using DA;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace EFC_Console
{
 /// <summary>
 /// Generates random database test data from common first and last names, and selected cities
 /// </summary>
 public class DataGenerator
 {
  // just a few airports
  static string[] Airports =
  {
   "Berlin", "Frankfurt", "Munich", "Hamburg", "Chicago", "Rome", "London", "Paris", "Milan", "Prague", "Moscow", "New York/JFC", "Seattle", "Berlin", "Kapstadt", "Madrid", "Oslo", "Dallas", "Vienna", "Sydney", "Auckland"
  };

  // Common German names
  // Source: http://de.wikipedia.org/wiki/Liste_der_h%C3%A4ufigsten_Familiennamen_in_Deutschland
  static string[] Surnames =
  {
   "Müller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Schulz", "Hoffmann", "Schäfer",
   "Koch", "Bauer", "Richter", "Klein", "Wolf", "Schröder", "Neumann", "Schwarz", "Zimmermann"
  };

  // Source: http://www.beliebte-vornamen.de/3467-alle-spitzenreiter.htm
  static string[] Firstnames =
   {"Leon", "Hannah", "Lukas", "Anna", "Leonie", "Marie", "Niklas", "Sarah", "Jan", "Laura", "Julia", "Lisa", "Kevin"};

  // Names of German politians 
  static string[] PilotSurnames =
  {
   "Gysi", "Seehofer", "Wagenknecht", "Steinmeyer", "Schulz", "Merkel", "Lindner", "Gabriel", "Lafontaine", "Özdemir",
   "Roth"
  };

  static string[] PilotFirstnames =
  {
   "Horst", "Sahra", "Martin", "Angela", "Joschka", "Christian", "Gregor", "Sigmar", "Frank-Walter", "Oskar", "Cem", "Claudia"
  };

  static int NO_OF_FLIGHTS = 1000;
  static int NO_OF_PASSENGERS = 2000;
  static int NO_OF_PILOTS = 50;

  static Random rnd = new Random(DateTime.Now.Millisecond);

  public static void Run(bool delete = false, int FlightCount = 1000, int PilotCount = 100, int PassengerCount = 2000)
  {

   NO_OF_FLIGHTS = FlightCount;
   NO_OF_PASSENGERS = PassengerCount;
   NO_OF_PILOTS = PilotCount;

   CUI.Print($"Data Generator: {FlightCount}/{PassengerCount}/{PilotCount} ", ConsoleColor.Blue, ConsoleColor.White);

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

   Init_AirlineSet();
   Init_PilotSet();
   Init_FlightSet();
   Init_PassengerSet();
   Init_BookingSet();

   CUI.PrintSuccess("=== End of data generation!");
  }


  /// <summary>
  /// 
  /// </summary>
  private static void Init_AirlineSet()
  {
   CUI.Headline("Create 2 Airlines...");
   WWWingsContext ctx = new WWWingsContext();
   var a1 = new Airline() { Code = "WWW", Name = "World Wide Wings" };
   var a2 = new Airline() { Code = "NCB", Name = "Never Come Back Airline" };
   ctx.AirlineSet.Add(a1);
   ctx.AirlineSet.Add(a2);
   ctx.SaveChanges();
   CUI.PrintSuccess("Number of airlines: " + ctx.AirlineSet.Count());
  }


  /// <summary>
  /// Create random flights
  /// </summary>
  private static Random Init_FlightSet()
  {
   Console.WriteLine("Create Flights...");
   WWWingsContext ctx = new WWWingsContext();
   ctx.ChangeTracker.AutoDetectChangesEnabled = false;
   var act1 = new AircraftType();
   act1.TypeID = 1;
   act1.Manufacturer = "Airbus";
   act1.Name = "A380-800"; // max 853 Passengers
   ctx.AircraftTypeSet.Add(act1);

   var act2 = new AircraftType();
   act2.TypeID = 2;
   act2.Manufacturer = "Boeing";
   act2.Name = "737-500"; // max 140 Passengers
   ctx.AircraftTypeSet.Add(act2);
   ctx.SaveChanges();

   var flightSet = from f in ctx.FlightSet select f;
   int Start = 100;
   Pilot[] PilotArray = ctx.PilotSet.ToArray();

   var sw = new Stopwatch();
   sw.Start();
   for (int i = Start; i < (Start + NO_OF_FLIGHTS); i++)
   {
    if (i % 100 == 0)
    {
     Console.WriteLine("Flight #" + i + " / Duration of creation of last 100:" + sw.ElapsedMilliseconds + "ms");
     sw.Restart();
     try
     {
      ctx.SaveChanges();
      ctx = new WWWingsContext();
     }
     catch (Exception ex)
     {
      Console.WriteLine("ERROR: " + ex.Message.ToString());
     }
    }
    Flight flightNew = new Flight();
    flightNew.FlightNo = i;
    flightNew.Departure = Airports[rnd.Next(0, Airports.Length - 1)];
    flightNew.Destination = Airports[rnd.Next(0, Airports.Length - 1)];
    if (flightNew.Departure == flightNew.Destination) // no roundtrip flights :-)
    {
     i--;
     continue;
    }
    flightNew.FreeSeats = Convert.ToInt16(new Random(i).Next(250));


    int randdomAircrafttype = (DateTime.Now.Millisecond % 2) + 1;
    flightNew.AircraftTypeID = (byte)randdomAircrafttype;

    if (randdomAircrafttype == 1) flightNew.Seats = 853;
    else flightNew.Seats = 140;

    flightNew.Date = DateTime.Now.AddDays((double)flightNew.FreeSeats).AddMinutes((double)flightNew.FreeSeats * 7);
    flightNew.PilotId = PilotArray[rnd.Next(PilotArray.Count() - 1)].PersonID;
    flightNew.CopilotId = PilotArray[rnd.Next(PilotArray.Count() - 1)].PersonID;
    ctx.FlightSet.Add(flightNew);
   }
   ctx.SaveChanges();
   CUI.PrintSuccess("Number of Flights: " + ctx.FlightSet.Count());


   var f101 = ctx.FlightSet.Find(101);
   f101.Departure = "Berlin";
   f101.Destination = "Seattle";
   ctx.SaveChanges();


   return rnd;
  }

  private static void Init_BookingSet()
  {
   WWWingsContext ctx = new WWWingsContext();
   int counter = 0;
   Random rnd5 = new Random();
   CUI.Headline("Create Bookings...");
   var flightSet2 = from f in ctx.FlightSet select f;

   var sw = new Stopwatch();
   sw.Start();

   Passenger[] passengerArray = ctx.PassengerSet.ToArray();
   foreach (Flight f in flightSet2.ToList())
   {
    counter++;

    if (counter % 100 == 0)
    {
     Console.WriteLine("Booking #" + counter + " / Duration of creation of last 100:" + sw.ElapsedMilliseconds + "ms");
     sw.Restart();
     try
     {
      ctx.SaveChanges();
     }
     catch (Exception ex)
     {
      Console.WriteLine(ex.Message);
     }
     ctx = new WWWingsContext();
    }


    //Console.WriteLine("Creating Bookings for Flight " + f.FlightNo);
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
      }
      catch (Exception ex)
      {
       Console.WriteLine(ex.Message);
       f.BookingSet.Remove(b);
      }
     }
    }
   }
   CUI.PrintSuccess("Number of Bookings: " + ctx.BookingSet.Count());
  }

  private static void Init_PilotSet()
  {
   CUI.Headline("Create Pilots...");
   WWWingsContext ctx = new WWWingsContext();
   for (int PNummer = 1; PNummer <= NO_OF_PILOTS; PNummer++)
   {
    string Vorname = PilotFirstnames[rnd.Next(0, PilotFirstnames.Length - 1)];
    string Nachname = PilotSurnames[rnd.Next(0, PilotSurnames.Length - 1)];
    Pilot p = new Pilot();
    p.Surname = Nachname;
    p.GivenName = Vorname;
    p.Birthday =
     new DateTime(1940, 1, 1).AddDays(Convert.ToInt32(new Random(DateTime.Now.Millisecond).Next(20000)));
    p.HireDate = DateTime.Now.AddDays(-rnd.Next(365));
    ctx.PilotSet.Add(p);
    ctx.SaveChanges();

    Console.WriteLine("Pilot #" + PNummer + ": " + Vorname + " " + Nachname);
   }

   CUI.PrintSuccess("Number of Pilots: " + ctx.PilotSet.Count());
  }

  private static void Init_PassengerSet()
  {
   CUI.Headline("Create Passengers...");
   WWWingsContext ctx = new WWWingsContext();
   //ctx.Log();
   ctx.ChangeTracker.AutoDetectChangesEnabled = false;
   Random rnd2 = new Random();

   var sw = new Stopwatch();
   sw.Start();

   for (int PNummer = 1; PNummer <= NO_OF_PASSENGERS; PNummer++)
   {
    string firstname = Firstnames[rnd2.Next(0, Firstnames.Length - 1)];   
    string surname = Surnames[rnd2.Next(0, Surnames.Length - 1)];
    Passenger p = new Passenger(); // ID is auto increment!
    p.Surname = surname;
    p.GivenName = firstname;
    p.Detail.City = Airports[rnd.Next(0, Airports.Length - 1)];
    p.Birthday =
    new DateTime(1930, 1, 1).AddDays(Convert.ToInt32(new Random(DateTime.Now.Millisecond).Next(30000)));

    p.Status = PassengerStatus.PassengerStatusSet.ElementAt(rnd.Next(3));
    ctx.PassengerSet.Add(p);

    if (PNummer % 100 == 0)
    {
     Console.WriteLine("Passenger #" + PNummer + " / Duration of creation of last 100:" + sw.ElapsedMilliseconds + "ms");
     sw.Restart();
     try
     {
      ctx.SaveChanges();
     }
     catch (Exception ex)
     {
      Console.WriteLine(ex.Message);
     }
     ctx = new WWWingsContext();
    }
   }
   CUI.PrintSuccess("Number of Passengers: " + ctx.PassengerSet.Count());
  }
 }
}