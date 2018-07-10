using DA;
using BO;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC_Console
{
 class AsyncOperations
 {
  /// <summary>
  /// Use of ToListAsync()
  /// </summary>
  [EFCBook()]
  public static async void ReadDataAsync()
  {
   CUI.MainHeadline("Start " + nameof(ReadDataAsync));
   using (var ctx = new WWWingsContext())
   {
    // Define query
    var query = (from f in ctx.FlightSet.Include(p => p.BookingSet).ThenInclude(b => b.Passenger) where f.Departure == "Rome" && f.FreeSeats > 0 select f).Take(1);
    // Abfrage aynchron ausführen
    CUI.PrintWithThreadID("Start database query");
    var flightSet = await query.ToListAsync();
    CUI.PrintWithThreadID("End database query");
    // Print results 
    foreach (Flight flight in flightSet)
    {
     CUI.PrintWithThreadID("Flight: " + flight.FlightNo + " from " + flight.Departure + " to " +
       flight.Destination + " has " + flight.FreeSeats + " free seats");

     foreach (var p in flight.BookingSet.Take(5))
     {
      CUI.PrintWithThreadID(" Passenger:  " + p.Passenger.GivenName + " " + p.Passenger.Surname);
     }
    }
    CUI.Print("End " + nameof(ReadDataAsync));
   }
  }

  /// <summary>
  /// Use of SaveChangesAsync()
  /// </summary>
  [EFCBook()]
  public static async void ChangeDataAsync()
  {
   CUI.MainHeadline("Start " + nameof(ChangeDataAsync));
   using (var ctx = new WWWingsContext())
   {
    // Define query
    var query = (from f in ctx.FlightSet.Include(p => p.BookingSet).ThenInclude(b => b.Passenger) where f.Departure == "Rome" && f.FreeSeats > 0 select f).Take(1);
    // Abfrage aynchron ausführen
    CUI.PrintWithThreadID("Start database query");
    var flightSet = await query.ToListAsync();
    CUI.PrintWithThreadID("End database query");
    // Print results 
    foreach (Flight flight in flightSet)
    {
     CUI.PrintWithThreadID("Flight: " + flight.FlightNo + " from " + flight.Departure + " to " +
       flight.Destination + " has " + flight.FreeSeats + " free seats");

     foreach (var b in flight.BookingSet.Take(5))
     {
      CUI.PrintWithThreadID(" Passenger:  " + b.Passenger.GivenName + " " + b.Passenger.Surname);
      CUI.PrintWithThreadID("   Start saving");
      b.Passenger.Status = 'A';
      var count = await ctx.SaveChangesAsync();
      CUI.PrintWithThreadID($"   {count} Changes saved!");
     }
    }
    CUI.Headline("End " + nameof(ChangeDataAsync));
   }
  }

  /// <summary>
  /// Use of ForEachAsync() 
  /// </summary>
  [EFCBook()]
  public static async void AsyncForeach()
  {
   CUI.MainHeadline("Start " + nameof(AsyncForeach));
   WWWingsContext ctx = new WWWingsContext();

   // Define query
   var query = (from f in ctx.FlightSet.Include(p => p.BookingSet).ThenInclude(b => b.Passenger) where f.Departure == "Rome" && f.FreeSeats > 0 select f).Take(1);
   // Executing und iterate query with ForEachAsync
   CUI.PrintWithThreadID("Print objects");
   await query.ForEachAsync(async flight =>
  {
   // Print results 
   CUI.PrintWithThreadID("Flight: " + flight.FlightNo + " from " + flight.Departure + " to " + flight.Destination + " has " + flight.FreeSeats + " free Seats");

   foreach (var p in flight.BookingSet)
   {
    CUI.PrintWithThreadID(" Passenger: " + p.Passenger.GivenName + " " + p.Passenger.Surname);
   }

   // Save changes to each flight object within the loop 
   //TODO: not possible wih EFCore 2.1.1 --> "New transaction is not allowed because there are other threads running in the session."
   //  CUI.PrintWithThreadID("  Start saving");
   //  flight.FreeSeats--;
   //await ctx.SaveChangesAsync();
   //CUI.PrintWithThreadID("   Changes saved!");

   //not possible wih EFCore 2.0: var count  = await ctx.SaveChangesAsync(); --> "New transaction is not allowed because there are other threads running in the session."


  });

   CUI.Print("End " + nameof(AsyncForeach));
  }

  [NotYetInTheBook]
  public async static void LINQ_MiscAsync()
  {
   CUI.MainHeadline("Start " + nameof(LINQ_MiscAsync));
   var ort = "Berlin";

   // Create context instance
   using (var ctx = new WWWingsContext())
   {
    // Abfragen definieren, aber noch nicht ausführen
    IQueryable<Flight> query = (from x in ctx.FlightSet
                                where x.Departure == ort &&
                                      x.FreeSeats > 0
                                orderby x.Date, x.Departure
                                select x);

    // Alternative:
    var query2 = ctx.FlightSet.Where(x => x.Departure == ort && x.FreeSeats > 0)
     .OrderBy(x => x.Date).ThenBy(x => x.Departure);

    // Count
    var count1 = await query.CountAsync();
  CUI.Print(System.Threading.Thread.CurrentThread.ManagedThreadId + ": " + count1);
    // Execute query now
    List<Flight> flightSet = await query.ToListAsync();
    CUI.Print(System.Threading.Thread.CurrentThread.ManagedThreadId + ": " + flightSet.Count);

    var flight = flightSet.Take(1).SingleOrDefault();

    flight.FreeSeats--;

    var count2 = await ctx.SaveChangesAsync();
    CUI.Print(System.Threading.Thread.CurrentThread.ManagedThreadId + ": " + count2);
   }
  }
 }
}
