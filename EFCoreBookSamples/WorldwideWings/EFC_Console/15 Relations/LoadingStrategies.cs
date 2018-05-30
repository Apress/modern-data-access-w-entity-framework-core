using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using ITVisions.EFCore;
using BO;

namespace EFC_Console
{
 class LoadingStrategies
 {

  [EFCBook("Default loading strategy")]
  public static void Demo_DefaultLoadingStrategy()
  {
   using (var ctx = new WWWingsContext())
   {
    List<Flight> list = (from x in ctx.FlightSet
                         where x.Departure == "Berlin" &&
                               x.FreeSeats > 0
                         orderby x.Date, x.Departure
                         select x).ToList();


   }

  }


  /// <summary>
  /// Does not work yet in EFC 1.x / 2.x :-(
  /// </summary>
  [EFCBook("Lazy Loading")]
  public static void Demo_LazyLoading()
  {
   CUI.MainHeadline(nameof(Demo_LazyLoading));

   using (var ctx = new WWWingsContext())
   {
    // Load only the flight
    var f = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == 101);

    Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    if (f.Pilot != null) Console.WriteLine($"Pilot: {f.Pilot.Surname} has {f.Pilot.FlightAsPilotSet.Count} flights as pilot!");
    else Console.WriteLine("No pilot assigned!");
    if (f.Copilot != null) Console.WriteLine($"Copilot: {f.Copilot.Surname} has {f.Copilot.FlightAsCopilotSet.Count} flights as copilot!");
    else Console.WriteLine("No copilot assigned!");

    if (f.BookingSet is null) Console.WriteLine("No bookings :-(");
    else
    {
     Console.WriteLine("Number of passengers on this flight: " + f.BookingSet.Count);

     Console.WriteLine("Passengers on this flight:");
     foreach (var b in f.BookingSet)
     {
      Console.WriteLine("- Passenger #{0}: {1} {2}", b.Passenger.PersonID, b.Passenger.GivenName, b.Passenger.Surname);
     }
    }
   }
  }

  /// <summary>
  /// Provides pilot, booking and passenger information via an explicit loading
  /// EFCore >= 1.1
  /// </summary>
  [EFCBook("Explicit Loading")]
  public static void Demo_ExplizitLoading()
  {
   CUI.MainHeadline(nameof(Demo_ExplizitLoading));

   using (var ctx = new WWWingsContext())
   {
    // Load only the flight
    var f = ctx.FlightSet
          .SingleOrDefault(x => x.FlightNo == 101);

    Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");

    // Now load the pilot and copilot
    if (!ctx.Entry(f).Reference(x => x.Pilot).IsLoaded)
     ctx.Entry(f).Reference(x => x.Pilot).Load();
    if (!ctx.Entry(f).Reference(x => x.Copilot).IsLoaded)
     ctx.Entry(f).Reference(x => x.Copilot).Load();

    // Check if loaded
    if (ctx.Entry(f).Reference(x => x.Pilot).IsLoaded) Console.WriteLine("Pilot is loaded!");
    if (ctx.Entry(f).Reference(x => x.Copilot).IsLoaded) Console.WriteLine("Copilot is loaded!");

    if (f.Pilot != null) Console.WriteLine($"Pilot: {f.Pilot.Surname} has {f.Pilot.FlightAsPilotSet.Count} flights as pilot!");
    else Console.WriteLine("No pilot assigned!");
    if (f.Copilot != null) Console.WriteLine($"Copilot: {f.Copilot.Surname} has {f.Copilot.FlightAsCopilotSet.Count} flights as copilot!");
    else Console.WriteLine("No copilot assigned!");

    // No download the booking list
    if (!ctx.Entry(f).Collection(x => x.BookingSet).IsLoaded)
     ctx.Entry(f).Collection(x => x.BookingSet).Load();

    Console.WriteLine("Number of passengers on this flight: " + f.BookingSet.Count);
    Console.WriteLine("Passengers on this flight:");
    foreach (var b in f.BookingSet)
    {
     // Now load the passenger object for this booking
     if (!ctx.Entry(b).Reference(x => x.Passenger).IsLoaded)
      ctx.Entry(b).Reference(x => x.Passenger).Load();
     Console.WriteLine("- Passenger #{0}: {1} {2}", b.Passenger.PersonID, b.Passenger.GivenName, b.Passenger.Surname);
    }
   }
  }

  /// <summary>
  /// Provides pilot, booking and passenger information
  /// </summary>
  [EFCBook()]
  public static void Demo_EagerLoading()
  {
   CUI.MainHeadline(nameof(Demo_EagerLoading));

   using (var ctx = new WWWingsContext())
   {
    var flightNo = 101;
    
    // Load the flight and some connected objects via Eager Loading
    var f = ctx.FlightSet
          .Include(b => b.BookingSet).ThenInclude(p => p.Passenger)
          .Include(b => b.Pilot).ThenInclude(p => p.FlightAsPilotSet)
          .Include(b => b.Copilot).ThenInclude(p => p.FlightAsCopilotSet)
          .SingleOrDefault(x => x.FlightNo == flightNo);

    Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    if (f.Pilot != null) Console.WriteLine($"Pilot: {f.Pilot.Surname} has {f.Pilot.FlightAsPilotSet.Count} flights as a pilot!");
    else Console.WriteLine("No pilot assigned!");
    if (f.Copilot != null) Console.WriteLine($"Copilot: {f.Copilot.Surname} has {f.Copilot.FlightAsCopilotSet.Count} flights as a Copilot!");
    else Console.WriteLine("No Copilot assigned!");

    Console.WriteLine("Number of passengers on this flight: " + f.BookingSet.Count);
    Console.WriteLine("Passengers on this flight:");
    foreach (var b in f.BookingSet)
    {
     Console.WriteLine("- Passenger #{0}: {1} {2}", b.Passenger.PersonID, b.Passenger.GivenName, b.Passenger.Surname);
    }
   }
  }

  [EFCBook("Preloading all")]
  public static void Demo_PreLoadingPilotsCaching()
  {
   CUI.MainHeadline(nameof(Demo_PreLoadingPilotsCaching));

   using (var ctx = new WWWingsContext())
   {

    // 1. Load ALL pilots
    ctx.PilotSet.ToList();

    // 2. Load only several flights. The Pilot and Copilot object will then be available for every flight!
    var flightNoListe = new List<int>() { 101, 117, 119, 118 };
    foreach (var flightNo in flightNoListe)
    {
     var f = ctx.FlightSet
      .SingleOrDefault(x => x.FlightNo == flightNo);

     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
     if (f.Pilot != null) Console.WriteLine($"Pilot: {f.Pilot.Surname} has {f.Pilot.FlightAsPilotSet.Count} flights as pilot!");
     else Console.WriteLine("No pilot assigned!");
     if (f.Copilot != null)
      Console.WriteLine($"Copilot: {f.Copilot.Surname} has {f.Copilot.FlightAsCopilotSet.Count} flights as copilot!");
     else Console.WriteLine("No copilot assigned!");
    }
   }
  }


  /// <summary>
  /// Liefert Pilot-, Buchungs- und Passagierinformationen via Preloading / RelationshipFixup
  /// </summary>
  [EFCBook("Preloading selected")]
  public static void Demo_PreLoading()
  {
   CUI.MainHeadline(nameof(Demo_PreLoading));

   using (var ctx = new WWWingsContext())
   {

    int flightNo = 101;

    // 1. Load only the flight itself
    var f = ctx.FlightSet
          .SingleOrDefault(x => x.FlightNo == flightNo);

    // 2. Invite both pilots for the above flight
    ctx.PilotSet.Where(p => p.FlightAsPilotSet.Any(x => x.FlightNo == flightNo) || p.FlightAsCopilotSet.Any(x => x.FlightNo == flightNo)).ToList();

    // 3. Load other flights from these two pilots
    ctx.FlightSet.Where(x => x.PilotId == f.PilotId || x.CopilotId == f.CopilotId).ToList();

    // 4. Load booking set for the above flight
    ctx.BookingSet.Where(x => x.FlightNo == flightNo).ToList();

    // 5. Load passengers for the above flight
    ctx.PassengerSet.Where(p => p.BookingSet.Any(x => x.FlightNo == flightNo)).ToList();

    // unnecessary: ctx.ChangeTracker.DetectChanges();

    Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    if (f.Pilot != null) Console.WriteLine($"Pilot: {f.Pilot.Surname} has {f.Pilot.FlightAsPilotSet.Count} flights as pilot!");
    else Console.WriteLine("No pilot assigned!");
    if (f.Copilot != null) Console.WriteLine($"Copilot: {f.Copilot.Surname} has {f.Copilot.FlightAsCopilotSet.Count} flights as copilot!");
    else Console.WriteLine("No copilot assigned!");

    Console.WriteLine("Number of passengers on this flight: " + f.BookingSet.Count);
    Console.WriteLine("Passengers on this flight:");
    foreach (var b in f.BookingSet)
    {
     Console.WriteLine("- Passenger #{0}: {1} {2}", b.Passenger.PersonID, b.Passenger.GivenName, b.Passenger.Surname);
    }
   }
  }


  [NotYetInTheBook]
  public static void Demo_EagerLoadingPilotDetails()
  {
   CUI.MainHeadline(nameof(Demo_EagerLoadingPilotDetails));

   var ctx = new WWWingsContext();
   var pilot = ctx.PilotSet.Include(p => p.FlightAsPilotSet).FirstOrDefault();
   Console.WriteLine("Pilot " + pilot?.FullName + " has " + pilot?.FlightAsPilotSet?.Count + " Flights!");
  }

  [NotYetInTheBook]
  public static void Demo_ExplizitLoadingCustom()
  {
   CUI.MainHeadline(nameof(Demo_ExplizitLoadingCustom));

   var ctx = new WWWingsContext();
   ctx.PassengerSet.ToList();

   var flightSet = (from f in ctx.FlightSet
                    where f.Departure.StartsWith("M")
                    select f)
                 .ToList();

   var flight = flightSet.ElementAt(0);

   Console.WriteLine(flight.ToString());

   // Explicit reloading all pilots
   ctx.PilotSet.ToList();
   Console.WriteLine("Pilot: " + flight.Pilot.FullName);
   foreach (var f in flightSet.Take(10))
   {
    Console.WriteLine(f.ToString());
    ctx.PilotSet.Where(x => x.FlightAsPilotSet.Any(y => y.FlightNo == f.FlightNo)).ToList();
    Console.WriteLine("Number of Passengers on this Flight: " + f.BookingSet.Count);
    Console.WriteLine("Pilot: " + f.Pilot.FullName);
   }
  }


  /// <summary>
  /// Preloading trick
  /// </summary>
  [NotYetInTheBook]
  public static void Demo_PreLoading1()
  {
   CUI.MainHeadline(nameof(Demo_PreLoading1));

   var ctx = new WWWingsContext();

   // Pre-Loading all pilots
   var piotenSet = ctx.PilotSet.ToList();
   Console.WriteLine("Anzahl Pilots: " + piotenSet.Count);
   //ctx.PassengerSet.ToList();

   // Now load some flights
   var flightSet = ctx.FlightSet.Take(10).ToList();

   var flight = flightSet.ElementAt(0);

   Console.WriteLine(flight.ToString());
   //Console.WriteLine("Number of passengers on this flight: " + flight.PassengerSet.Count);
   if (flight.Pilot != null) Console.WriteLine("Pilot: " + flight.Pilot.FullName);
   else Debugger.Break();

   foreach (var f in flightSet.Take(10))
   {
    Console.WriteLine(f.ToString());
    //Console.WriteLine("Number of passengers on this flight: " + f.PassengerSet.Count);
    if (flight.Pilot != null) Console.WriteLine("Pilot: " + f.Pilot.FullName);
   }
  }

  public static void Demo_EagerLoadingAllFlights()
  {
   CUI.MainHeadline(nameof(Demo_EagerLoadingAllFlights));
   var ctx = new WWWingsContext();

   ctx.Log();
   //var l1 = ctx.FlightSet.Count();

   //Console.WriteLine("Number of flights:" + l1);

   var q = ctx.FlightSet
    .Include(flight1 => flight1.Pilot).ThenInclude(pilot1 => pilot1.FlightAsPilotSet)
    .Include(flight2 => flight2.Copilot).ThenInclude(pilot2 => pilot2.FlightAsCopilotSet)
    .Include(flight3 => flight3.BookingSet).ThenInclude(buchung => buchung.Passenger);

   var flightSet = q.ToList();

   var flight = flightSet.FirstOrDefault();

   Console.WriteLine(flight.ToString());

   if (flight.Pilot != null) Console.WriteLine("Pilot: " + flight.Pilot.FullName);
   if (flight.Copilot != null) Console.WriteLine("Copilot: " + flight.Copilot.FullName);

   Console.WriteLine(flight.BookingSet.Count + " Passengers on this flight: ");

   foreach (var b in flight.BookingSet)
   {
    Console.WriteLine(b.Passenger);
   }
  }

  /// <summary>
  /// requirement: No instantiation of List<Booking> in Flight ctor
  /// </summary>
  public static void DemoFillNavigationProperties()
  {
   var ctx = new WWWingsContext();
   Console.WriteLine("---------- Load flight");
   var flight101 = ctx.FlightSet.Where(x => x.FlightNo == 101).SingleOrDefault();
   Console.WriteLine("BookingSet: " + flight101.BookingSet?.Count);
   Console.WriteLine("---------- New flight");
   var flightNeu = new BO.Flight();
   Console.WriteLine("BookingSet: " + flightNeu.BookingSet?.Count);
  }
 }
}
