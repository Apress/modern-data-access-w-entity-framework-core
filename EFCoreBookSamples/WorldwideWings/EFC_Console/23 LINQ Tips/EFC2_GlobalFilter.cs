using DA;
using BO;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC_Console
{

 /// <summary>
 /// New in EFC 2.0
 /// Prerequisite in context: 
 /// modelBuilder.Entity<Flight>().HasQueryFilter(x => x.FreeSeats > 0 && x.AirlineCode == "WWW");
 /// </summary>
 [EFCBook] [Article("dnp81_122017")]
 public class EFC2_GlobalFilters
 {


  public static void GlobalFilter()
  {
   CUI.MainHeadline(nameof(GlobalFilter));

   using (var ctx = new WWWingsContext(GlobalQueryFilter: true))
   {
    ctx.Log();
    //-------------------------------------------------------
    CUI.Headline("LINQ query");
    List<Flight> flightSet = (from f in ctx.FlightSet // 
                          where f.Departure == "Berlin"
                          select f).ToList();
    CUI.Print(flightSet.Count + " Results", ConsoleColor.Yellow);
    foreach (var f in flightSet)
    {
     Console.WriteLine(f);
    }

    CUI.Headline("SQL query");
    List<Flight> flightSet2 = ctx.FlightSet.FromSql("select * from Flight where Departure = 'Berlin'").ToList();
    CUI.Print(flightSet2.Count + " results", ConsoleColor.Yellow);
    foreach (var f in flightSet2)
    {
     Console.WriteLine(f);
    }

    CUI.Headline("TVF");
    List<Flight> flightSet3 = ctx.FlightSet.FromSql("Select * from GetFlightsFromTVF({0})", "Berlin").Where(f=>f.NonSmokingFlight == true).ToList();
    CUI.Print(flightSet3.Count + " results", ConsoleColor.Yellow);
    foreach (var flight in flightSet3)
    {
     Console.WriteLine(flight);
    }

    CUI.Headline("SP");
    List<Flight> flightSet4 = ctx.FlightSet.FromSql("EXEC GetFlightsFromSP {0}", "Berlin").ToList();
    CUI.Print(flightSet4.Count + " results", ConsoleColor.Yellow);
    foreach (var flight in flightSet4)
    {
     Console.WriteLine(flight);
    }


    //-------------------------------------------------------
    List<Flight> flightAlleSet = (from f in ctx.FlightSet.IgnoreQueryFilters()
                              where f.Departure == "Berlin"
                              select f).ToList();
    CUI.Print(flightAlleSet.Count + " results", ConsoleColor.Yellow);
    foreach (var f in flightAlleSet)
    {
     Console.WriteLine(f);
    }

    //-------------------------------------------------------
    Console.WriteLine("Filtered: " + ctx.FlightSet.Count());
    Console.WriteLine("All: " + ctx.FlightSet.IgnoreQueryFilters().Count());

    //-------------------------------------------------------
    CUI.Headline("Pilots (Eager Loading)");

    var pilotWithFlights = ctx.PilotSet.Include(x => x.FlightAsPilotSet).ToList();

    foreach (var p in pilotWithFlights)
    {
     Console.WriteLine(p);
     foreach (var f in p.FlightAsPilotSet.ToList())
     {
      Console.WriteLine(" - " + f.ToString());
     }
    }

    //-------------------------------------------------------
    CUI.Headline("Pilots (Explicit Loading)");

    var pilotenSet = ctx.PilotSet.ToList();

    foreach (var p in pilotenSet)
    {
     Console.WriteLine(p);
     ctx.Entry(p).Collection(x => x.FlightAsPilotSet).Load();
     foreach (var f in p.FlightAsPilotSet.ToList())
     {
      Console.WriteLine(" - " + f.ToString());
     }
    }
   }
  }
 }
}