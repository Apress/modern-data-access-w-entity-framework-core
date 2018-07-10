using DA;
using BO;
using ITVisions;
using System;
using System.Linq;

namespace EFC_Console
{
 [EFCBook()]
 class RelationshipFixupDemo
 {
  [EFCBook()]
  public static void RelationshipFixUp_Case1()
  {
   CUI.MainHeadline(nameof(RelationshipFixUp_Case1));

   using (var ctx = new WWWingsContext())
   {

    int flightNr = 101;

    // 1. Just load the flight
    var flight = ctx.FlightSet.Find(flightNr);

    // 2. Output of the pilot of the Flight
    Console.WriteLine(flight.PilotId + ": " + (flight.Pilot != null ? flight.Pilot.ToString() : "Pilot not loaded!"));

    // 3. Load the pilot separately
    var pilot = ctx.PilotSet.Find(flight.PilotId);

    // 4. Output des Pilots of the Flight: Pilot now availavle
    Console.WriteLine(flight.PilotId + ": " + (flight.Pilot != null ? flight.Pilot.ToString() : "Pilot not loaded!"));

    // 5. Output the list oftflights of this pilot
    foreach (var f in pilot.FlightAsPilotSet)
    {
     Console.WriteLine(f);
    }
   }
  }

  [EFCBook()]
  public static void RelationshipFixUp_Case2()
  {
   CUI.MainHeadline(nameof(RelationshipFixUp_Case2));
   void PrintPilot(Pilot pilot)
   {
    CUI.PrintSuccess(pilot.ToString());
    if (pilot.FlightAsPilotSet != null)
    {
     Console.WriteLine("Flights of this pilot:");
     foreach (var f in pilot.FlightAsPilotSet)
     {
      Console.WriteLine(f);
     }
    }
    else
    {
     CUI.PrintWarning("No flights!");
    }
   }

   using (var ctx = new WWWingsContext())
   {
    // Load a Pilot
    var pilot = ctx.PilotSet.FirstOrDefault();

    // Print pilot and his flights
    PrintPilot(pilot);
    // Create a new flight for this pilot
    var flight = new Flight();
    flight.Departure = "Berlin";
    flight.Destination = "Berlin";
    flight.Date = DateTime.Now.AddDays(10);
    flight.FlightNo = ctx.FlightSet.Max(x => x.FlightNo) + 1;
    flight.PilotId = pilot.PersonID;
    ctx.FlightSet.Add(flight);
  // this does not help:  ctx.ChangeTracker.DetectChanges();
    // Print pilot and his flights
    PrintPilot(pilot);
    // Print pilot of the new flight
    Console.WriteLine(flight.Pilot);
   }
  }

  [EFCBook()]
  public static void RelationshipFixUp_Case3()
  {
   CUI.MainHeadline(nameof(RelationshipFixUp_Case3));

   // Inline helper for output (>= C# 7.0)
   void PrintflightPilot(Flight flight, Pilot pilot)
   {
    CUI.PrintSuccess(flight);
    Console.WriteLine(flight.PilotId + ": " + (flight.Pilot != null ? flight.Pilot.ToString() : "Pilot not loaded!"));
    CUI.PrintSuccess(pilot.ToString());
    if (pilot.FlightAsPilotSet != null)
    {
     Console.WriteLine("Flights of this pilot:");
     foreach (var f in pilot.FlightAsPilotSet)
     {
      Console.WriteLine(f);
     }
    }
    else
    {
     CUI.PrintWarning("No flights!");
    }
   }

   using (var ctx = new WWWingsContext())
   {
    int flightNr = 101;

    CUI.Headline("Load flight");

    var flight = ctx.FlightSet.Find(flightNr);
    Console.WriteLine(flight);
    // Pilot of this flight
    Console.WriteLine(flight.PilotId + ": " + (flight.Pilot != null ? flight.Pilot.ToString() : "Pilot not loaded!"));

    CUI.Headline("Load pilot");
    var pilot = ctx.PilotSet.FirstOrDefault();
    Console.WriteLine(pilot);

    CUI.Headline("Assign a new pilot");
    flight.Pilot = pilot;  // Case 3a
    //flight.PilotId = pilot.PersonID; // Case 3b

    // Determine which relationships exist
    PrintflightPilot(flight, pilot);

    // Here you have to trigger the Relationshop fixup yourself
    CUI.Headline("DetectChanges...");
    ctx.ChangeTracker.DetectChanges();

    // Determine which relationships exist
    PrintflightPilot(flight, pilot);
   }
  }
 }
}
