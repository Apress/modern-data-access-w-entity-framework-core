using System;
using System.Linq;
using DA;
using EFC_Console;
using ITVisions;
using Microsoft.EntityFrameworkCore;
namespace EFC_Console
{
 class ContradictoryRelationships
 {
  /// <summary>
  /// Four test scenarios for the question of which value has priority, if the relationship is set contradictory
  /// </summary>
  [EFCBook()]
  public static void Demo_ContradictoryRelationships()
  {
   CUI.MainHeadline(nameof(Demo_ContradictoryRelationships));
   Attempt1();
   Attempt2();
   Attempt3();
   Attempt4();
  }

  public static int pilotID = new WWWingsContext().PilotSet.Min(x => x.PersonID);

  public static int GetPilotIdEinesFreienPilots()
  {
   // here we assume that the next one in the list has time for this flight :-)
   pilotID++; return pilotID;
  }

  private static void Attempt1()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline("Attempt 1: first assignment by navigation property, then by foreign key property");
    CUI.PrintStep("Load a flight...");
    var flight101 = ctx.FlightSet.Include(f => f.Pilot).SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");
    CUI.Print("Pilot object: " + flight101.Pilot.PersonID + " PilotId: " + flight101.PilotId);

    CUI.PrintStep("Load another pilot...");
    var newPilot2 = ctx.PilotSet.Find(GetPilotIdEinesFreienPilots()); // nächster Pilot
    CUI.PrintStep($"Assign a new pilot #{newPilot2.PersonID} via navigation property...");
    flight101.Pilot = newPilot2;
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("Reassign a new pilot via foreign key property...");
    var neuePilotID = GetPilotIdEinesFreienPilots();
    CUI.PrintStep($"Assign a new pilot #{neuePilotID} via foreign key property...");
    flight101.PilotId = neuePilotID;
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("SaveChanges()");
    var anz2 = ctx.SaveChanges();
    CUI.PrintSuccess("Number of saved changes: " + anz2);

    CUI.PrintStep("Control output after saving: ");
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");
   }
  }

  private static void Attempt2()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline("Attempt 2: First assignment by foreign key property, then navigation property");
    CUI.PrintStep("Load a flight...");
    var flight101 = ctx.FlightSet.Include(f => f.Pilot).SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");
    CUI.Print("Pilot object: " + flight101.Pilot.PersonID + " PilotId: " + flight101.PilotId);

    var neuePilotID2 = GetPilotIdEinesFreienPilots();
    CUI.PrintStep($"Assign a new pilot #{neuePilotID2} via foreign key property...");
    flight101.PilotId = neuePilotID2;
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("Load another pilot...");
    var newPilot1 = ctx.PilotSet.Find(GetPilotIdEinesFreienPilots()); // nächster Pilot
    CUI.PrintStep($"Assign a new pilot #{newPilot1.PersonID} via navigation property...");
    flight101.Pilot = newPilot1;
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("SaveChanges()");
    var anz2 = ctx.SaveChanges();
    CUI.PrintSuccess("Number of saved changes: " + anz2);

    CUI.PrintStep("Control output after saving: ");
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");
   }
  }

  private static void Attempt3()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline("Attempt 3: Assigment using FK, then Navigation Property at Flight, then Navigation Property at Pilot");
    CUI.PrintStep("Load a flight...");
    var flight101 = ctx.FlightSet.Include(f => f.Pilot).SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight No {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");
    CUI.Print("Pilot object: " + flight101.Pilot.PersonID + " PilotId: " + flight101.PilotId);

    var neuePilotID3 = GetPilotIdEinesFreienPilots();
    CUI.PrintStep($"Assign a new pilot #{neuePilotID3} via foreign key property...");
    flight101.PilotId = neuePilotID3;
    CUI.Print("flight101.PilotId=" + flight101.PilotId);
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("Load another pilot...");
    var newPilot3a = ctx.PilotSet.Find(GetPilotIdEinesFreienPilots()); // nächster Pilot
    CUI.PrintStep($"Assign a new pilot #{newPilot3a.PersonID} via navigation property bei Flight...");
    flight101.Pilot = newPilot3a;
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("Load another Pilot...");
    var newPilot3b = ctx.PilotSet.Include(p => p.FlightAsPilotSet).SingleOrDefault(p => p.PersonID == GetPilotIdEinesFreienPilots()); // nächster Pilot
    CUI.PrintStep($"Assign a new pilot #{newPilot3b.PersonID} via navigation property bei Pilot...");
    newPilot3b.FlightAsPilotSet.Add(flight101);
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("SaveChanges()");
    var anz3 = ctx.SaveChanges();
    CUI.PrintSuccess("Number of saved changes: " + anz3);

    CUI.PrintStep("Control output after saving: ");
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

   }
  }

  private static void Attempt4()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline("Attempt 4: First assignment by FK, then Navigation Property at Pilot, then Navigation Property at Flight");
    CUI.PrintStep("Load a flight...");
    var flight101 = ctx.FlightSet.Include(f => f.Pilot).SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");
    CUI.Print("Pilot object: " + flight101.Pilot.PersonID + " PilotId: " + flight101.PilotId);

    var neuePilotID4 = GetPilotIdEinesFreienPilots();
    CUI.PrintStep($"Assign a new pilot #{neuePilotID4} via foreign key property...");
    flight101.PilotId = neuePilotID4;
    CUI.Print("flight101.PilotId=" + flight101.PilotId);
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("Load another pilot...");
    var newPilot4b = Queryable.SingleOrDefault(ctx.PilotSet.Include(p => p.FlightAsPilotSet), p => p.PersonID == GetPilotIdEinesFreienPilots()); // nächster Pilot
    CUI.PrintStep($"Assign a new pilot #{newPilot4b.PersonID} via navigation property...");
    newPilot4b.FlightAsPilotSet.Add(flight101);
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("Lade noch einen anderen Pilots...");
    var newPilot4a = ctx.PilotSet.Find(GetPilotIdEinesFreienPilots()); // nächster Pilot
    CUI.PrintStep($"Assign a new pilot #{newPilot4a.PersonID} via navigation property bei Flight...");
    flight101.Pilot = newPilot4a;
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");

    CUI.PrintStep("SaveChanges()");
    var anz4 = ctx.SaveChanges();
    CUI.PrintSuccess("Number of saved changes: " + anz4);

    CUI.PrintStep("Control output after saving: ");
    CUI.Print($"PilotId: {flight101.PilotId} Pilot object: {flight101.Pilot?.PersonID}");
   }
  }
 }
}