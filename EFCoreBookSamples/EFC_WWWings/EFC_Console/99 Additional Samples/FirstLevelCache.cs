using System;
using System.Linq;
using BO;
using DA;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

//**** NOTE: This sample is not in the English book. Therefore it has not been translated!

namespace EFC_Console
{
 static internal class FirstLevelCache
 {
  [NotYetInTheBook]
  public static void RelationshipFixup_Run()
  {
   CUI.Headline("RelationshipFixup/First Level Cache");

   using (WWWingsContext ctx = new WWWingsContext())
   {

    ctx.Log();
    var pilot = ctx.PilotSet.Find(1030);
    Console.WriteLine("Pilot: " + pilot.PersonID);

    pilot = null;

    // Möglich: Löschen aus Cache
    // ctx.Pilot.Local.Clear(); somit würde es wirklich weg sein!
    // or: ctx.Pilot.Local.Remove(pilot); VOR DEM SETZEN AUF NULL!
    // der Pilot sollte nun weg sein
    var flight = ctx.FlightSet.Find(101);
    // ist er aber nicht !!!
    Console.WriteLine(flight);
    Console.WriteLine("Pilot dieses flightSets: " + flight.Pilot.PersonID);
   }
  }

  /// <summary>
  /// Local.Clear-Problem
  /// </summary>
  /// <param name="departure"></param>
  /// <returns></returns>
  [NotYetInTheBook]
  public static void LocalClear()
  {

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    CUI.Headline("Lade die ersten 5 Flights und Pilots...");
    ctx.FlightSet.Take(5).ToList();
    ctx.PilotSet.Take(5).ToList();

    Console.WriteLine("Flights im Cache: " + ctx.FlightSet.Local.Count);
    Console.WriteLine("Pilots im Cache: " + ctx.PilotSet.Local.Count);

    CUI.Headline("Lade die ersten 10 Flights und Pilots...");
    ctx.FlightSet.Take(10).ToList();
    ctx.PilotSet.Take(10).ToList();

    Console.WriteLine("Flights im Cache: " + ctx.FlightSet.Local.Count);
    Console.WriteLine("Pilots im Cache: " + ctx.PilotSet.Local.Count);
    PrintCache(ctx.FlightSet);

    CUI.Headline("Löschen Flight-Cache...");
    ctx.FlightSet.Local.Clear();
    Console.WriteLine("Flights im Cache: " + ctx.FlightSet.Local.Count);
    Console.WriteLine("Pilots im Cache: " + ctx.PilotSet.Local.Count);

    foreach (EntityEntry entry in ctx.ChangeTracker.Entries())
    {
     Console.WriteLine(entry.Entity.ToString() + ": " + entry.State);
    }
    //ctx.SaveChanges();

    CUI.Headline("Lade die ersten 15 Flights und Pilots...");
    var flightSet = ctx.FlightSet.Take(15).ToList();
    Console.WriteLine("Geladene Flights: " + ctx.FlightSet.Local.Count);
    Console.WriteLine(flightSet[0]);
    CUI.Headline("Zugriff auf einen Flight via Find()...");
    ctx.FlightSet.Find(100);
    Console.WriteLine("Flights im Cache: " + ctx.FlightSet.Local.Count);
    Console.WriteLine(ctx.FlightSet.Local.ElementAt(0));

    ctx.PilotSet.Take(15).ToList();

    Console.WriteLine("Pilots im Cache: " + ctx.PilotSet.Local.Count);
    PrintCache(ctx.FlightSet);
   }
  }


  public static void ClearCache()
  {

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    CUI.Headline("Load 5 Flights and Pilots...");
    ctx.FlightSet.Take(5).ToList();
    ctx.PilotSet.Take(5).ToList();

    Console.WriteLine("Flights in Cache: " + ctx.FlightSet.Local.Count);
    Console.WriteLine("Pilots in Cache: " + ctx.PilotSet.Local.Count);

    foreach (var f in ctx.FlightSet.Local.ToList())
    {
     ctx.Entry(f).State = EntityState.Detached;
    }

    Console.WriteLine("Flights in Cache: " + ctx.FlightSet.Local.Count);
    Console.WriteLine("Pilots in Cache: " + ctx.PilotSet.Local.Count);

   }
  }

   public static void PrintCache(DbSet<Flight> set)
  {
   //Console.WriteLine("Cacheinhalt from " + set.GetType());
   //foreach (var f in set.Local)
   //{
   // Console.WriteLine(f.ToString());
   //}

  }

  /// <summary>
  /// Getfluege mit Standardcaching des Kontextes und Änderung!
  /// </summary>
  /// <param name="departure"></param>
  /// <returns></returns>
  [NotYetInTheBook]
  public static void GetFlight_StandardCache_Aendern(string departure = "Rome")
  {
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    CUI.Headline($"Alle Flights from {departure} laden!");
    // Alle Flights laden
    var flightSet = ctx.FlightSet.Where(x => x.Departure == departure && x.FlightNo < 200).ToList();

    var flightNo = 190; // FlightSet 190 ist ein FlightSet from Rom, der schon geladen wurde // flightSet.ElementAt

    // Ein Objekt wird im RAM geändert
    var flightSetImRAM = flightSet.FirstOrDefault(x => x.FlightNo == flightNo);
    CUI.Print("FlightSet Before changes: " + flightSetImRAM?.ToShortString(), ConsoleColor.White);
    flightSetImRAM.FreeSeats--;
    CUI.Print("FlightSet to der Änderung: " + flightSetImRAM?.ToShortString(), ConsoleColor.White);

    // Dann wird das Objekt neu geladen!
    CUI.Headline("Ein FlightSet laden mit Find() - allein aus Cache!");
    var flight1 = ctx.FlightSet.Find(flightNo);
    CUI.Print(flight1?.ToShortString(), ConsoleColor.White);

    CUI.Headline("Ein FlightSet laden mit SingleOrDefault() -> Query!");
    var flight2 = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == flightNo);
    CUI.Print(flight2?.ToShortString(), ConsoleColor.White);

    CUI.Headline("Ein FlightSet laden mit FirstOrDefault() -> Query!");
    var flight3 = ctx.FlightSet.FirstOrDefault(x => x.FlightNo == flightNo);
    CUI.Print(flight3?.ToShortString(), ConsoleColor.White);

    CUI.Headline("Ein FlightSet laden mit where/SingleOrDefault() -> Query!");
    var flight4 = ctx.FlightSet.Where(x => x.FlightNo == flightNo).SingleOrDefault();
    CUI.Print(flight4.ToShortString(), ConsoleColor.White);

    CUI.Headline("Cacheinhalt");
    foreach (var f in ctx.FlightSet.Local)
    {
     Console.WriteLine(f.ToShortString());
    }
   }
  }
 }
}