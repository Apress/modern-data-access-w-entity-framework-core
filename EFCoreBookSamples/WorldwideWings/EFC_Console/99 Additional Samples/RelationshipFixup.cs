using ITVisions;
using System;
using System.Linq;
using DA;
using BO;
using Microsoft.EntityFrameworkCore;

//**** NOTE: This sample is not in the English book. Therefore it has not been translated!

namespace EFC_Console
{
 public class RelationshipFixup
 {
  [NotYetInTheBook]
  public static void Demo1_PilotBleibtImRAM()
  {
   CUI.Headline("RelationshipFixup/First Level Cache");

   int flightNo = 101;
   int pilotID = 0;
   // Vorspiel: passende PilotID ermittelt
   using (var ctx = new WWWingsContext())
   {
    pilotID = ctx.FlightSet.Include(f => f.Pilot).SingleOrDefault(f => f.FlightNo == flightNo).PilotId;

   }

   using (var ctx = new WWWingsContext())
   {
    var pilot = ctx.PilotSet.AsNoTracking().SingleOrDefault(x => x.PersonID == pilotID);

    Console.WriteLine("--- Navigationseigenschaften-Mengentypen");
    Console.WriteLine(pilot.FlightAsPilotSet?.Count + " FlightAsPilotSet: " + pilot.FlightAsPilotSet?.GetType().FullName);
    Console.WriteLine(pilot.FlightAsCopilotSet?.Count + " FlightAsCopilotSet: " + pilot.FlightAsCopilotSet?.GetType().FullName);

    Console.WriteLine("Pilot: " + pilot);
    pilot = null;

    // der Pilot sollte nun weg sein
    var flight = ctx.FlightSet.Include(f => f.Pilot).SingleOrDefault(f => f.FlightNo == flightNo);

    Console.WriteLine(flight);
    Console.WriteLine(flight.Pilot);
    if (flight.Pilot != null) CUI.PrintSuccess("Fixup OK!");
    else CUI.PrintError("Fixup geht nicht!");
   }


  }

  /// <summary>
  /// Relationsship Fixup
  /// </summary>
  [NotYetInTheBook]
  public static void Demo2_RueckwartigeBeziehung()
  {
   int id = 250;

   // Vorspiel
   using (var ctx = new WWWingsContext())
   {
    var falt = ctx.FlightSet.Find(id);
    if (falt != null)
    {
     ctx.FlightSet.Remove(falt); ctx.SaveChanges();
    }
   }

   using (var ctx = new WWWingsContext())
   {
    var falt = ctx.FlightSet.Find(id);
    if (falt != null)
    {
     ctx.FlightSet.Remove(falt); ctx.SaveChanges();
    }
    CUI.Print("Neuer Flight", ConsoleColor.Yellow);
    var f = new BO.Flight();
    f.FlightNo = id;
    f.Departure = "Frankfurt";
    f.Destination = "Berlin";
    f.Date = DateTime.Now;
    f.FreeSeats = 10;
    f.Seats = 100;

    ctx.FlightSet.Add(f);
    Console.WriteLine(f);

    CUI.Print("Neuer Pilot", ConsoleColor.Yellow);
    var p = new BO.Pilot();
    p.Surname = "Schwichtenberg";
    p.GivenName = "Holger";
    p.PilotLicenseType = PilotLicenseType.ATP;
    ctx.PilotSet.Add(p);
    Console.WriteLine(p);

    Console.WriteLine("--- Navigationseigenschaften-Mengentypen");
    Console.WriteLine(p.FlightAsPilotSet?.Count + " FlightAsPilotSet: " + p.FlightAsPilotSet?.GetType().FullName);
    Console.WriteLine(p.FlightAsCopilotSet?.Count + " FlightAsCopilotSet: " + p.FlightAsCopilotSet?.GetType().FullName);
    p.LicenseDate = DateTime.Now;

    CUI.Print("Zuweisung des Pilots zum Flight", ConsoleColor.Yellow);
    f.Pilot = p;
    f.Copilot = p;

    Console.WriteLine("--- Navigationseigenschaften-Mengentypen");
    Console.WriteLine(p.FlightAsPilotSet?.Count + " FlightAsPilotSet: " + p.FlightAsPilotSet?.GetType().FullName);
    Console.WriteLine(p.FlightAsCopilotSet?.Count + " FlightAsCopilotSet: " + p.FlightAsCopilotSet?.GetType().FullName);

    CUI.Print("Speichern und Relationship Fixup", ConsoleColor.Yellow);
    ctx.SaveChanges();
    ctx.ChangeTracker.DetectChanges();

    // to SaveChanges sind rückwärtige Beziehungen hergestellt
    Console.WriteLine("--- Navigationseigenschaften-Mengentypen");
    Console.WriteLine(p.FlightAsPilotSet?.Count + " FlightAsPilotSet: " + p.FlightAsPilotSet?.GetType().FullName);
    Console.WriteLine(p.FlightAsCopilotSet?.Count + " FlightAsCopilotSet: " + p.FlightAsCopilotSet?.GetType().FullName);

   }
  }


 }

}
