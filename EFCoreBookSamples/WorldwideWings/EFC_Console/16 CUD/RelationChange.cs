using System;
using System.Diagnostics;
using System.Linq;
using BO;
using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ITVisions.EFCore;

namespace EFC_Console
{
 class RelationChange
 {
  [EFCBook()]
  public static void Demo_CreateRelatedObjects()
  {
   CUI.MainHeadline(nameof(Demo_CreateRelatedObjects));
   using (var ctx = new WWWingsContext())
   {
    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo = 456789");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo = 456789");

    var p = new Pilot();
    p.GivenName = "Holger";
    p.Surname = "Schwichtenberg";
    p.HireDate = DateTime.Now;
    p.LicenseDate = DateTime.Now;
    var pd = new Persondetail();
    pd.City = "Essen";
    pd.Country = "DE";
    p.Detail = pd;

    var act = new AircraftType();
    act.TypeID = (byte)(ctx.AircraftTypeSet.Max(x=>x.TypeID)+1);
    act.Manufacturer = "Airbus";
    act.Name = "A380-800";
    ctx.AircraftTypeSet.Add(act);
    ctx.SaveChanges();

    var actd = new AircraftTypeDetail();
    actd.TurbineCount = 4;
    actd.Length = 72.30f;
    actd.Tare = 275;
    act.Detail = actd;

    var f = new Flight();
    f.FlightNo = 456789;
    f.Pilot = p;
    f.Copilot = null;
    f.Seats = 850;
    f.FreeSeats = 850;
    f.AircraftType = act;

    // One Add() is enough for all related objects!
    ctx.FlightSet.Add(f);
    ctx.SaveChanges();

    CUI.Print("Total number of flights: " + ctx.FlightSet.Count());
    CUI.Print("Total number of pilots: " + ctx.PilotSet.Count());
   }
  }

  [EFCBook()]
  public static void Demo_RelationhipFixup1N()
  {
   CUI.MainHeadline(nameof(Demo_RelationhipFixup1N));
   using (var ctx = new WWWingsContext())
   {
    // Load a flight
    var flight101 = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");

    // Load the pilot for this flight with the list of his flights
    var pilotAlt = ctx.PilotSet.Include(x => x.FlightAsPilotSet).SingleOrDefault(x => x.PersonID == flight101.PilotId);
    Console.WriteLine("Pilot: " + pilotAlt.PersonID + ": " + pilotAlt.GivenName + " " + pilotAlt.Surname + " has " + pilotAlt.FlightAsPilotSet.Count + " flights as pilot!");

    // Next pilot in the list load with the list of his flights
    var newPilot = ctx.PilotSet.Include(x => x.FlightAsPilotSet).SingleOrDefault(x => x.PersonID == flight101.PilotId + 1);
    Console.WriteLine("Planned Pilot: " + newPilot.PersonID + ": " + newPilot.GivenName + " " + newPilot.Surname + " has " + newPilot.FlightAsPilotSet.Count + " flights as pilot!");

    // Assign to Flight
    CUI.Print("Assignment of the flight to the planned pilot...", ConsoleColor.Cyan);
    newPilot.FlightAsPilotSet.Add(flight101);

    // optional:force Relationship Fixup 
    // ctx.ChangeTracker.DetectChanges();

    CUI.Print("Control output before saving: ", ConsoleColor.Cyan);
    Console.WriteLine("Old pilot: " + pilotAlt.PersonID + ": " + pilotAlt.GivenName + " " + pilotAlt.Surname + " has " + pilotAlt.FlightAsPilotSet.Count + " flights as a pilot!");
    Console.WriteLine("New pilot: " + newPilot.PersonID + ": " + newPilot.GivenName + " " + newPilot.Surname + " has " + newPilot.FlightAsPilotSet.Count + " flights as a pilot!");
    var pilotAktuell = flight101.Pilot; // Aktueller Pilot aus der Sicht des Flight-Objekts
    Console.WriteLine("Pilot for flight " + flight101.FlightNo + " is currently: " + pilotAktuell.PersonID + ": " + pilotAktuell.GivenName + " " + pilotAktuell.Surname);

    // SaveChanges()()
    CUI.Print("Saving... ", ConsoleColor.Cyan);
    var anz = ctx.SaveChanges();
    CUI.MainHeadline("Number of saved changes: " + anz);

    CUI.Print("Control output after save: ", ConsoleColor.Cyan);
    Console.WriteLine("Old Pilot: " + pilotAlt.PersonID + ": " + pilotAlt.GivenName + " " + pilotAlt.Surname + " has " + pilotAlt.FlightAsPilotSet.Count + " flights as pilot!");
    Console.WriteLine("New Pilot: " + newPilot.PersonID + ": " + newPilot.GivenName + " " + newPilot.Surname + " has " + newPilot.FlightAsPilotSet.Count + " flights as pilot!");
    pilotAktuell = flight101.Pilot; // Current pilot from the perspective of the Flight object
    Console.WriteLine("Pilot for Flight " + flight101.FlightNo + " is now: " + pilotAktuell.PersonID + ": " + pilotAktuell.GivenName + " " + pilotAktuell.Surname);
   }
  }


  [NotYetInTheBook]
  public static void Demo_FlightAddAndRead()
  {
   CUI.MainHeadline("Demo_FlightAddAndRead");

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo = 101");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo = 101");

    var pilot = new Pilot();
    pilot.Surname = "testpilot";
    ctx.PilotSet.Add(pilot);


    var fneu = new Flight();
    fneu.FlightNo = 101;
    fneu.Pilot = pilot;
    fneu.Seats = 100;
    fneu.Copilot = pilot;
    fneu.FreeSeats = 100;
    ctx.FlightSet.Add(fneu);



    ctx.SaveChanges();

    // hier funktioniert das Lazy Loading (noch nicht)
    // var buchung = ctx.BookingSet.FirstOrDefault();

    // Console.WriteLine(buchung.Passenger.FullName);
    //Console.WriteLine(buchung.Flight.ToString());


    var flight101 = ctx.FlightSet
     .Include(b => b.BookingSet).ThenInclude(p => p.Passenger)
     .Include(b => b.Pilot) //.ThenInclude(p=>p.FlightAsPilotSet)
     .Include(b => b.Copilot)// .ThenInclude(p => p.FlightAsCopilotSet)
     .SingleOrDefault(x => x.FlightNo == 101);
    // Flightdaten ausgeben
    Console.WriteLine("Flight #" + flight101.FlightNo + " from " + flight101.Departure + " to " + flight101.Destination);
    // Pilot und Copilot ausgeben
    if (flight101.Pilot != null) Console.WriteLine("Pilot: " + flight101.Pilot.Surname);
    if (flight101.Copilot != null) Console.WriteLine("Copilot: " + flight101.Copilot.Surname);
    // All Passagiere ausgeben
    foreach (var b in flight101.BookingSet)
    {
     Console.WriteLine(" - " + b.Passenger.Surname);
    }
    flight101.FreeSeats--;
    int anz = ctx.SaveChanges();
    if (anz != 1) Debugger.Break();
   }
  }

  public static void TrackGraph_CallbackAdded(EntityEntryGraphNode e)
  {
   Console.WriteLine("TrackGraph_Callback: " + e.Entry.Entity + ": " + e.NodeState + ", " + e.Entry.State);
   e.Entry.State = EntityState.Added;
   Console.WriteLine("TrackGraph_Callback: " + e.Entry.Entity + ": " + e.NodeState + ", " + e.Entry.State);
  }

  public static void BuchungAnlegen()
  {
   Console.WriteLine("BuchungAnlegen");
   using (WWWingsContext ctx = new WWWingsContext())
   {

    var flight = ctx.FlightSet.FirstOrDefault();
    var pas = ctx.PassengerSet.FirstOrDefault();
    Console.WriteLine($"Passenger {pas.PersonID} -> Flight {flight.FlightNo}");

    #region Vorbereitung
    // nur für die idempotente Demo: Booking vorher löschen, wenn schon da :-)
    var b_alt = ctx.BookingSet.FirstOrDefault(x => x.FlightNo == flight.FlightNo && x.PassengerID == pas.PersonID);
    if (b_alt != null) { ctx.BookingSet.Remove(b_alt); ctx.SaveChanges(); }
    #endregion

    // so würde man das in EF 6.x machen:
    //flight.PassengerSet.Add(pas);

    // in EF Core braucht man Zwischenobjekt Booking mit den PKs als FKs :-(
    var b = new Booking();
    b.FlightNo = flight.FlightNo;
    b.PassengerID = pas.PersonID;
    ctx.BookingSet.Add(b);

    var anz = ctx.SaveChanges();
    Console.WriteLine("Saved changes: " + anz);
   }

  }

  

  public static void Kontextwechselproblem()
  {

   Flight flight101;

   using (var ctx1 = new WWWingsContext())
   {
    // Lade einen Flight
    flight101 = ctx1.FlightSet.Include(f => f.Pilot).SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats! Pilot: " + flight101.Pilot);

  }
    using (var ctx2 = new WWWingsContext())
    {
     // Lade einen Flight
     var pilot = ctx2.PilotSet.Find(flight101.PilotId + 1);
     flight101.Pilot = pilot;
     Console.WriteLine("Neuer Pilot: " + pilot.ToString());
     var anz = ctx2.SaveChanges();
     Console.WriteLine(anz + " Changes saved!");
    }


   }
 }
 }
