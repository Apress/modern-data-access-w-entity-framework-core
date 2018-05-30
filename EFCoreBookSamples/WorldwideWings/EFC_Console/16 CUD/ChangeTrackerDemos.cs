using System;
using System.Linq;
using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using BO;
using System.Collections.Generic;
using ITVisions.EFCore;

namespace EFC_Console.AutoMapper
{
 static internal class ChangeTrackerDemos
 {

  [EFCBook()]
  public static void ChangeTracking_OneObject()
  {
   CUI.MainHeadline(nameof(ChangeTracking_OneObject));

   Flight flight;
   EntityEntry<BO.Flight> entryObj;
   PropertyEntry propObj;

   using (var ctx = new WWWingsContext())
   {

    CUI.Headline("Loading Object...");
    flight = (from y in ctx.FlightSet select y).FirstOrDefault();

    // Access Change Tracker
    entryObj = ctx.Entry(flight);
    propObj = entryObj.Property(f => f.FreeSeats);
    Console.WriteLine(" Object state: " + entryObj.State);
    Console.WriteLine(" Is FreeSeats modified?: " + propObj.IsModified);

    CUI.Headline("Chanhing Object...");
    flight.FreeSeats--;

    // Access Change Tracker again
    entryObj = ctx.Entry(flight);
    propObj = entryObj.Property(f => f.FreeSeats);
    Console.WriteLine(" Object state: " + entryObj.State);
    Console.WriteLine(" Is FreeSeats modified?: " + propObj.IsModified);

    // Print old and new values
    if (entryObj.State == EntityState.Modified)
    {
     foreach (PropertyEntry p in entryObj.Properties)
     {
      if (p.IsModified)
       Console.WriteLine(" " + p.Metadata.Name + ": " + p.OriginalValue + "->" + p.CurrentValue +
                         " / State in database: " + entryObj.GetDatabaseValues()[p.Metadata.Name]);
     }
    }

    CUI.Headline("Save...");
    int anz = ctx.SaveChanges();
    Console.WriteLine(" Number of changes: " + anz);

    // aktualisieren der Objecte des Change Trackers
    entryObj = ctx.Entry(flight);
    propObj = entryObj.Property(f => f.FreeSeats);
    Console.WriteLine(" Object state: " + entryObj.State);
    Console.WriteLine(" Is FreeSeats modified?: " + propObj.IsModified);
   }
  }

  /// <summary>
  /// </summary>
  [EFCBook()]
  public static void ChangeTracking_MultipleObjects()
  {
   CUI.MainHeadline(nameof(ChangeTracking_MultipleObjects));

   using (var ctx = new WWWingsContext())
   {
    var flightQuery = (from y in ctx.FlightSet select y).OrderBy(f4 => f4.FlightNo).Take(3);
    foreach (var flight in flightQuery.ToList())
    {
     flight.FreeSeats -= 2;
     flight.Memo = "Changed on " + DateTime.Now;
    }

    var newFlight = ctx.FlightSet.Find(123456);
    if (newFlight != null)
    {
     ctx.Remove(newFlight);
    }
    else
    {
     newFlight = new Flight();
     newFlight.FlightNo = 123456;
     newFlight.Departure = "Essen";
     newFlight.Destination = "Sydney";
     newFlight.AirlineCode = "WWW";
     newFlight.PilotId = ctx.PilotSet.FirstOrDefault().PersonID;
     newFlight.Seats = 100;
     newFlight.FreeSeats = 100;
     ctx.FlightSet.Add(newFlight);
    }
    CUI.Headline("New objects");
    IEnumerable<EntityEntry> neueObjecte = ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
    if (neueObjecte.Count() == 0) Console.WriteLine("none");
    foreach (EntityEntry entry in neueObjecte)
    {
     CUI.Print("Object " + entry.Entity.ToString() + " State: " + entry.State, ConsoleColor.Cyan);
     ITVisions.EFCore.EFC_Util.PrintChangedProperties(entry);
    }

    CUI.Headline("Changed objects");
    IEnumerable<EntityEntry> geaenderteObjecte =
     ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
    if (geaenderteObjecte.Count() == 0) Console.WriteLine("none");
    foreach (EntityEntry entry in geaenderteObjecte)
    {
     CUI.Print("Object " + entry.Entity.ToString() + " State: " + entry.State, ConsoleColor.Cyan);
     ITVisions.EFCore.EFC_Util.PrintChangedProperties(entry);
    }

    CUI.Headline("Deleted objects");
    IEnumerable<EntityEntry> geloeschteObjecte = ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
    if (geloeschteObjecte.Count() == 0) Console.WriteLine("none");
    foreach (EntityEntry entry in geloeschteObjecte)
    {
     CUI.Print("Object " + entry.Entity.ToString() + " State: " + entry.State, ConsoleColor.Cyan);
    }
    Console.WriteLine("Changes: " + ctx.SaveChanges());
   }
  }
  // nicht erlaubt, Modified auf false zu setzen: obj.Property(f => f.FreeSeats).IsModified = false;
  //ctx.Configuration.ValidateOnSaveEnabled = false;

  [NotYetInTheBook]
  public static void EF_ChangeTrackerAuswerten()
  {
   CUI.MainHeadline(nameof(EF_ChangeTrackerAuswerten));

   using (WWWingsContext ctx = new WWWingsContext())
   {
    var f = ctx.FlightSet.FirstOrDefault();
    Console.WriteLine("Before: " + f.ToString());


    f.FreeSeats = (short)(new Random(DateTime.Now.Millisecond).Next(1, 100));
    Console.WriteLine("After: " + f.ToString());

    EFC_Util.PrintChangeInfo(ctx);
    var anz = ctx.SaveChanges();
    Console.WriteLine("Changes: " + anz);

   }
  }

 }
}