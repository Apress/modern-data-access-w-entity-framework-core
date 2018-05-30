using DA;
using BO;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics;
using System.Linq;

namespace EFC_Console
{

 class CalculatedColumns
 {
  [EFCBook()]
  public static void DefaultValues()
  {
   CUI.MainHeadline(nameof(DefaultValues));

   using (WWWingsContext ctx = new WWWingsContext())
   {
    var pilot = ctx.PilotSet.FirstOrDefault();
    ctx.Log();
    var f = new Flight();
    f.FlightNo = ctx.FlightSet.Max(x => x.FlightNo) + 1;
    f.Departure = "Berlin";
    f.Destination = null;
    f.Pilot = pilot;
    f.Copilot = null;
    f.FreeSeats = 100;
    f.Seats = 100;
    CUI.Headline("Object has been created in RAM");
    Console.WriteLine($"{f} Price: {f.Price:###0.00} Euro.");
    ctx.FlightSet.Add(f);
    CUI.Headline("Object has been connected to the ORM");
    Console.WriteLine($"{f} Price: {f.Price:###0.00} Euro.");
    ctx.SaveChanges();
    CUI.Headline("Object has been saved");
    Console.WriteLine($"{f} Price: {f.Price:###0.00} Euro.");

    f.FreeSeats--;
    CUI.Headline("Object has been changed in RAM");
    Console.WriteLine($"{f} Price: {f.Price:###0.00} Euro.");
    ctx.SaveChanges();
    CUI.Headline("Object has been saved"); ;
    Console.WriteLine($"{f} Price: {f.Price:###0.00} Euro.");

    //if (f.Destination != "(not set)") Debugger.Break();
    //if (f.Price != 123.45m) Debugger.Break();
   }
  }

  /// <summary>
  /// The Utilization column in the Flight class is based on a formula
  /// </summary>
  [EFCBook()]
  public static void ComputedColumnWithFormula()
  {
   CUI.MainHeadline(nameof(ComputedColumnWithFormula));

   int flightNo = 101;
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    var flight = ctx.FlightSet.Find(flightNo);
    Console.WriteLine($"BEFORE: {flight}: Utilization={flight.Utilization:##0.00}%");

    flight.FreeSeats -= 10;
    //not possible: flight.Utilization = 100;

    // The change is not yet visible in the Utilization, since the Utilization is calculated in the DBMS
    Console.WriteLine($"After changes: {flight}: Utilization={flight.Utilization:##0.00}%");

    ctx.SaveChanges();
    // The change in Utilization is now visible
    Console.WriteLine($"After saving: {flight}: Utilization={flight.Utilization:##0.00}%");

    CUI.Headline("Metadata of Flight properties");
    foreach (PropertyEntry p in ctx.Entry(flight).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": " + p.Metadata.ValueGenerated);
    }
   }
  }
 }
}
