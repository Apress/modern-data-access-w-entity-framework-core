using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ITVisions.EFCore;

namespace EFC_Console
{

 class TrackingModes
 {

  [NotYetInTheBook]
  public static void TrackingMode_Performance()
  {
   for (int i = 0; i < 10; i++)
   {
    using (var ctx = new WWWingsContext())
    {
     var sw = new Stopwatch();
     sw.Start();
     var liste1 = ctx.FlightSet.ToList();
     sw.Stop();
     Console.WriteLine("TRACKING: Loaded objects: " + liste1.Count + " Duration: " + sw.ElapsedMilliseconds);
    }

    using (var ctx = new WWWingsContext())
    {
     var sw = new Stopwatch();
     sw.Start();
     var liste2 = ctx.FlightSet.AsNoTracking().ToList();
     sw.Stop();
     Console.WriteLine("NO-TRACKING: Loaded objects: " + liste2.Count + " Duration: " + sw.ElapsedMilliseconds);
    }
   }
  }

  [EFCBook()]
  public static void TrackingMode_AsNoTracking()
  {
   CUI.MainHeadline(nameof(TrackingMode_AsNoTracking));
   CUI.Headline("Tracking mode");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Unchanged
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Modified
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 1
   }

   CUI.Headline("No-Tracking mode");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.AsNoTracking().ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 0
   }
  }

  [EFCBook()]
  public static void TrackingMode_NoTracking_Attach()
  {

   CUI.MainHeadline(nameof(TrackingMode_NoTracking_Attach));

   CUI.Headline("Attach() before change");

   using (WWWingsContext ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.AsNoTracking().ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    ctx.Attach(flight);
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Unchanged
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Modified
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 0
   }

   CUI.Headline("Attach() after change (change state per property)");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.AsNoTracking().ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    ctx.Attach(flight);
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Unchanged
    // geändertes Attribut bei EFC melden
    ctx.Entry(flight).Property(f => f.FreeSeats).IsModified = true;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Modified
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 1
   }

   CUI.Headline("Attach() after change (change state per object)");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    var flightSet = ctx.FlightSet.AsNoTracking().ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    ctx.Attach(flight);
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Unchanged
    ctx.Entry(flight).State = EntityState.Modified;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Modified
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 1
   }

   CUI.Headline("Attach() with non entity class -- not allowed!!!");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var obj = new FileInfo(@"c:\temp\daten.txt");
    try
    {
     ctx.Attach(obj); // Error: The entity type 'FileInfo' was not found. Ensure that the entity type has been added to the model.
    }
    catch (Exception e)
    {
    CUI.PrintError(e);
    }

   }
  }

  [EFCBook()]
  public static void TrackingMode_QueryTrackingBehavior()
  {
   CUI.MainHeadline("Default setting: TrackAll. Use AsNoTracking()");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll; // is default
    var flightSet = ctx.FlightSet.AsNoTracking().ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 1
   }

   CUI.MainHeadline("Default setting: NoTracking.");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // NoTracking
    var flightSet = ctx.FlightSet.ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Detached
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 1
   }

   CUI.MainHeadline("Default setting: NoTracking. Use AsTracking()");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // NoTracking
    var flightSet = ctx.FlightSet.AsTracking().ToList();
    var flight = flightSet[0];
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Unchanged
    flight.FreeSeats--;
    Console.WriteLine(flight + " object state: " + ctx.Entry(flight).State); // Modified
    int count = ctx.SaveChanges();
    Console.WriteLine($"Saved changes: {count}"); // 1
   }
  }
 }
}
