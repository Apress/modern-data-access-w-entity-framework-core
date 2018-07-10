using DA;
using BO;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EFC_Console
{
 class Updates
 {
 

  [EFCBook()]
  public static void ChangeFlightManyProperties()
  {
   CUI.MainHeadline(nameof(ChangeFlightManyProperties));
   int flightNo = 101;
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var f = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == flightNo);

    Console.WriteLine($"After loading: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the Flight object: " + ctx.Entry(f).State);
    f.FreeSeats -= 2;
    //f.Departure = "sdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhsdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh";
    f.Memo = $"Last change durch Benutzer {System.Environment.UserName} on {DateTime.Now}."; ;  // Änderung 2

    Console.WriteLine($"After changes: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0) Console.WriteLine("Problem: No changes saved!");
     else Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the Flight object: " + ctx.Entry(f).State);
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }
   }
  }

  [EFCBook()]
  public static void ChangeFlightAndPilot()
  {
   CUI.MainHeadline(nameof(ChangeFlightAndPilot));

   int flightNo = 101;
   using (WWWingsContext ctx = new WWWingsContext())
   {

    var f = ctx.FlightSet.Include(x => x.Pilot).SingleOrDefault(x => x.FlightNo == flightNo);

    Console.WriteLine($"After loading: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats!\nState of the flight object: " + ctx.Entry(f).State + " / State of the Pilot object: " + ctx.Entry(f.Pilot).State);
    f.FreeSeats -= 2;
    f.Pilot.FlightHours = (f.Pilot.FlightHours ?? 0) + 10;
    f.Memo = $"Changed by User {System.Environment.UserName} on {DateTime.Now}.";

    Console.WriteLine($"After changes: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats!\nState of the flight object: " + ctx.Entry(f).State + " / State of the Pilot object: " + ctx.Entry(f.Pilot).State);

    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0) Console.WriteLine("Problem: No changes saved!");
     else Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats!\nState of the flight object: " + ctx.Entry(f).State + " / State of the Pilot object: " + ctx.Entry(f.Pilot).State);
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }
   }
  }

  [EFCBook()]
  public static void AddFlight()
  {
   CUI.MainHeadline(nameof(AddFlight));


   using (WWWingsContext ctx = new WWWingsContext())
   {
    // Create flight in RAM
    var f = new Flight();
    f.FlightNo = 123456;
    f.Departure = "Essen";
    f.Destination = "Sydney";
    f.AirlineCode = "WWW";
    f.PilotId = ctx.PilotSet.FirstOrDefault().PersonID;
    f.Seats = 100;
    f.FreeSeats = 100;

    Console.WriteLine($"Before adding: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the Flight object: " + ctx.Entry(f).State);

    // Add flight to Kontext 
    ctx.FlightSet.Add(f);
    // or: ctx.Add(f);

    Console.WriteLine($"After adding: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the Flight object: " + ctx.Entry(f).State);

    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0) Console.WriteLine("Problem: No changes saved!");
     else Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the Flight object: " + ctx.Entry(f).State);
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }

   }
  }


  public static void AddFlightWithDetails()
  {
   Console.WriteLine(nameof(AddFlightWithDetails));

   using (WWWingsContext ctx = new WWWingsContext())
   {
    //var p = ctx.PilotSet.SingleOrDefault(x => x.PersonID == 234);

    // Flight im RAM anlegen

    var f = new Flight();
    f.FlightNo = 123456;
    //f.Pilot = p;
    f.Departure = "Essen";
    f.Destination = "Sydney";
    f.AirlineCode = "WWW";
    f.PilotId = 23;

    f.Copilot = null; // kein Copilot :-(
    f.Seats = 100;
    f.FreeSeats = 100;

    Console.WriteLine($"Vor dem Anfügen: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    // Flight dem Kontext "hinzufügen"
    ctx.FlightSet.Add(f);
    ctx.FlightSet.Add(f);
    ctx.Add(f);

    Console.WriteLine($"Nach dem Anfügen: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0) Console.WriteLine("Problem: No changes saved!");
     else Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }

   }
  }

  [EFCBook()]
  public static void RemoveFlight()
  {
   CUI.MainHeadline(nameof(RemoveFlight));

   using (WWWingsContext ctx = new WWWingsContext())
   {
    var f = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == 123456);
    if (f == null) return;

    Console.WriteLine($"After loading: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    // Remove flight
    ctx.FlightSet.Remove(f);
    // or: ctx.Remove(f);

    Console.WriteLine($"After deleting: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0) Console.WriteLine("Problem: No changes saved!");
     else Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }
   }
  }


  [EFCBook()]
  public static void RemoveFlightWithKey()
  {
   Console.WriteLine(nameof(RemoveFlightWithKey));

   using (WWWingsContext ctx = new WWWingsContext())
   {
    // Create a dummy object
    var f = new Flight();
    f.FlightNo = 123456;

    Console.WriteLine($"After creation: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    // Append dummy object to context
    ctx.Attach(f);

    Console.WriteLine($"After attach: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    // Delete flight
    ctx.FlightSet.Remove(f);
    // or: ctx.Remove(f);

    Console.WriteLine($"After remove: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0) Console.WriteLine("Problem: No changes saved!");
     else Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }
   }
  }

  public static void Batching_Add()
  {
   Console.WriteLine(nameof(Batching_Add));

   const short Anz = 300;

   using (var ctx = new WWWingsContext())

   {
    //ctx.Log();
    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo >= 10000");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo >= 10000");

    var pilot1 = ctx.PilotSet.FirstOrDefault();
    var pilot2 = ctx.PilotSet.Skip(1).FirstOrDefault();

    CUI.MainHeadline(Anz + " Add");
    Stopwatch sw1 = new Stopwatch();
    sw1.Start();
    for (int i = 0; i < Anz; i++)
    {
     var newFlight = new Flight();
     newFlight.FlightNo = 10000 + i;
     newFlight.Pilot = pilot1;
     newFlight.Seats = 100;
     newFlight.Copilot = pilot2;
     newFlight.FreeSeats = 100;
     ctx.FlightSet.Add(newFlight);
     ctx.SaveChanges();
    }
    sw1.Stop();
    Console.WriteLine(Anz + " Add: " + sw1.ElapsedMilliseconds + "ms");
   }
  }

  public static void AddRange(short Anz = 400)
  {
   Console.WriteLine(nameof(AddRange));
   using (var ctx = new WWWingsContext())
   {

    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo >= 10000");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo >= 10000");
    var pilot1 = ctx.PilotSet.FirstOrDefault();
    var pilot2 = ctx.PilotSet.Skip(1).FirstOrDefault();
    CUI.MainHeadline(Anz + " AddRange");
    Stopwatch sw3 = new Stopwatch();
    sw3.Start();

    var liste = new List<Flight>();
    for (int i = 0; i < Anz; i++)
    {

     var newFlight = new Flight();
     newFlight.FlightNo = 30000 + i;
     newFlight.Pilot = pilot1;
     newFlight.Seats = 100;
     newFlight.Copilot = pilot2;
     newFlight.FreeSeats = 100;
     liste.Add(newFlight);
    }

    ctx.FlightSet.AddRange(liste);
    ctx.SaveChanges();
    sw3.Stop();
    Console.WriteLine(Anz + " AddRange: " + sw3.ElapsedMilliseconds + "ms");
   }
  }

 [NotYetInTheBook]
  public static void AddBatch(short Anz = 400)
  {
   Console.WriteLine(nameof(AddBatch));

   using (var ctx = new WWWingsContext())

   {
    CUI.MainHeadline(Anz + " Add Batch");
    var pilot1 = ctx.PilotSet.FirstOrDefault();
    var pilot2 = ctx.PilotSet.Skip(1).FirstOrDefault();
    ctx.Log();
    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo >= 10000");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo >= 10000");

    Stopwatch sw2 = new Stopwatch();
    sw2.Start();
    for (int i = 0; i < Anz; i++)
    {
     var newFlight = new Flight();
     newFlight.FlightNo = 20000 + i;
     newFlight.Pilot = pilot1;
     newFlight.Seats = 100;
     newFlight.Copilot = pilot2;
     newFlight.FreeSeats = 100;
     ctx.FlightSet.Add(newFlight);
    }
    ctx.SaveChanges();
    sw2.Stop();
    Console.WriteLine(Anz + " Add Batch: " + sw2.ElapsedMilliseconds + "ms");

    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo >= 10000");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo >= 10000");
   }
  }
  [NotYetInTheBook]

  public static void EF_ChangeTracking()
  {
   CUI.MainHeadline(nameof(EF_ChangeTracking));

   using (WWWingsContext ctx = new WWWingsContext())
   {
    // kommt in nächster Version
    //ctx.Configuration.AutoDetectChangesEnabled = false;

    var f = ctx.FlightSet.FirstOrDefault();

    Console.WriteLine("Before: " + f.ToString());

    //Änderung
    f.FreeSeats = (short)(new Random(DateTime.Now.Millisecond).Next(1, 100));
    Console.WriteLine("After: " + f.ToString());

    EFC_Util.PrintChangeInfo(ctx);
    var anz = ctx.SaveChanges();
    Console.WriteLine("Anzahl gespeicherter Ändeurngen: " + anz);
   }
  }

 [NotYetInTheBook]
  public static void Batching_Change10Flights()
  {
   CUI.MainHeadline(nameof(Batching_Change10Flights));
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    var list = ctx.FlightSet.Take(10).ToList();
    foreach (var f in list)
    {
     Console.WriteLine("Before: " + f.ToString());
     f.FreeSeats -= 1;  //Änderung
     Console.WriteLine("After: " + f.ToString());
    }
    var anz = ctx.SaveChanges();
    CUI.MainHeadline("Number of saved changes: " + anz);
   }
  }

  /// <summary>
  /// SaveChanges() does not work within a foreach loop unless you have previously materialized the records
  /// Possible solutions:
  /// 1. query.ToList ()
  /// 2. SaveChangesAsync() instead of SaveChanges()
  /// </summary>
  [EFCBook("foreach, update")]
  public static void Demo_ForeachProblem()
  {
   CUI.Headline(nameof(Demo_ForeachProblem));
   WWWingsContext ctx = new WWWingsContext();
   // Define query
   var query = (from f in ctx.FlightSet.Include(p => p.BookingSet).ThenInclude(b => b.Passenger) where f.Departure == "Rome" && f.FreeSeats > 0 select f).Take(1);
   // Query is performed implicitly by foreach
   foreach (var flight in query)
   {
    // Print results
    CUI.Print("Flight: " + flight.FlightNo + " from " + flight.Departure + " to " + flight.Destination + " has " + flight.FreeSeats + " free seats");

    foreach (var p in flight.BookingSet)
    {
     CUI.Print(" Passenger  " + p.Passenger.GivenName + " " + p.Passenger.Surname);
    }

    // Save change to every flight object within the loop
    CUI.Print("  Start saving");
    flight.FreeSeats--;
    ctx.SaveChangesAsync(); // SaveChanges() will produce ERROR!!!
    CUI.Print("  End saving");
   }
  }
 }
}