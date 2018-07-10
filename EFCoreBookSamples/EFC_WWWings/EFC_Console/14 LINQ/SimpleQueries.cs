using System;
using System.Collections.Generic;
using System.Linq;
using EFC_Console;
using BO;
using ITVisions.EFCore;
using DA;
using ITVisions;

namespace EFC_Console
{
 public class SimpleQueries
 {
  /// <summary>
  /// Print a simple LINQ query that returns all unbooked Flights from a Departure location - sorted by date and Departure
  /// </summary>
  [EFCBook("Where, OrderBy, ToList")]
  public static void LINQ_List()
  {
   CUI.MainHeadline(nameof(LINQ_List));

   var city = "Berlin";

   // Instantiate context
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    // Define queries, but do not execute yet
    IQueryable<Flight> query = (from x in ctx.FlightSet
                                  where x.Departure == city &&
                                        x.FreeSeats > 0
                                  orderby x.Date, x.Departure
                                  select x);

    // Alternative:
    var query2 = ctx.FlightSet.Where(x => x.Departure == city && x.FreeSeats > 0)
     .OrderBy(x => x.Date).ThenBy(x => x.Departure);

    // DatabaseViewWithPseudoEntity query now
    List<Flight> flightSet = query.ToList();

    // Alternative:
    var flightSet2 = (from x in ctx.FlightSet
                      where x.Departure == city &&
                            x.FreeSeats > 0
                      orderby x.Date, x.Departure
                      select x).ToList();

    // Count loaded objects
    var count1 = flightSet.Count;
    Console.WriteLine("Number of loaded flights: " + count1);

    // not good, because another round trip to the DBMS:
    var count2 = query.Count();

    // Print results
    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
   } // End using-Block -> Dispose() will be called
  }

  /// <summary>
  /// LINQ query that returns all not booked up flights on a route, with both Departure and destination being optional
  /// </summary>
  [EFCBook("Composition")]
  public static void LINQ_Composition()
  {
   CUI.MainHeadline(nameof(LINQ_Composition));

   var departure = "";
   var destination = "Rome";

   // Create context instance
   using (var ctx = new WWWingsContext())
   {
    // Define query, but do not execute yet
    IQueryable<Flight> query = from x in ctx.FlightSet
                                where x.FreeSeats > 0
                                select x;

    // Conditional addition of further conditions
    if (!String.IsNullOrEmpty(departure)) query = query.Where(x => x.Departure == departure);
    if (!String.IsNullOrEmpty(destination)) query = query.Where(x => x.Destination == destination);

    // now use sorting, otherwise there will be problems with variable query type (IQueryable <Flight> vs. IOrderedQueryable <Flight>)
    IOrderedQueryable<Flight> querySorted = from x in query
                                            orderby x.Date, x.Departure
                                            select x;

    // Execute query now
    List<Flight> flightSet = querySorted.ToList();

    // Count loaded objects
    long c = flightSet.Count;
    Console.WriteLine("Number of loaded flights: " + c);

    // Print result
    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
   } // End using-Block -> Dispose()
  }

  /// <summary>
  /// LINQ query that returns all not booked up flights on a route, with both Departure and destination being optional
  /// </summary>
  [EFCBook("Composition, WRONG")]
  public static void LINQ_CompositionWrongOrder()
  {
   CUI.MainHeadline(nameof(LINQ_Composition));

   var departure = "";
   var destination = "Rome";

   // Create context instance
   using (var ctx = new WWWingsContext())
   {
    // Define query (ToList() ist WRONG here!)
    var query = (from x in ctx.FlightSet
     where x.FreeSeats > 0
     select x).ToList();

    // Conditional addition of further conditions
    if (!String.IsNullOrEmpty(departure)) query = query.Where(x => x.Departure == departure).ToList();
    if (!String.IsNullOrEmpty(destination)) query = query.Where(x => x.Destination == destination).ToList();

    // Sorting
    var querySorted = from x in query
     orderby x.Date, x.Departure
     select x;

    // The query shoud execute here, but it is already executed
    List<Flight> flightSet = querySorted.ToList();

    // Count loaded objects
    long c = flightSet.Count;
    Console.WriteLine("Number of loaded flights: " + c);

    // Print result
    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
   } // End using-Block -> Dispose()
  }

  /// <summary>
  /// Use of repository class 
  /// </summary>
  [EFCBook("RepositoryPattern")]
  public static void LINQ_RepositoryPattern()
  {
   CUI.MainHeadline(nameof(LINQ_RepositoryPattern));
   using (var fm = new BL.FlightManager())
   {
    IQueryable<Flight> query = fm.GetAllAvailableFlightsInTheFuture();
    // Extend base query now
    query = query.Where(f => f.Departure == "Berlin");
    // Execute the query now
    var flightSet = query.ToList();
    Console.WriteLine("Number of loaded flights: " + flightSet.Count);
   }
  }

  [EFCBook("Paging")]
  public static void LINQ_QueryWithPaging()
  {
   CUI.MainHeadline(nameof(LINQ_QueryWithPaging));
   string name = "Müller";
   DateTime date = new DateTime(1972, 1, 1);
   // Create context instance
   using (var ctx = new WWWingsContext())
   {

    // Define query and execute
    var flightSet = (from f in ctx.FlightSet
                     where f.FreeSeats > 0 &&
                           f.BookingSet.Count > 0 &&
                           f.BookingSet.Any(b => b.Passenger.Surname == name) &&
                           f.Pilot.Birthday < date &&
                           f.Copilot != null
                     select f).Skip(5).Take(10).ToList();

    // Count number of loaded objects
    var c = flightSet.Count;
    Console.WriteLine("Number of found flights: " + c);

    // Print objects
    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
   } // End using-Block -> Dispose()
  }

  [EFCBook("Projection")]
  public static void Projection_Read()
  {
   CUI.MainHeadline(nameof(Projection_Read));
   using (var ctx = new WWWingsContext())
   {
    var query = from f in ctx.FlightSet
                  where f.FlightNo > 100 && f.FlightNo < 200
                  orderby f.FlightNo
                  select new Flight()
                  {
                   FlightNo = f.FlightNo,
                   Date = f.Date,
                   Departure = f.Departure,
                   Destination = f.Destination,
                   FreeSeats = f.FreeSeats
                  };

    var flightSet = query.ToList();

    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
   }
  }

  [EFCBook("SingleOrDefault")]
  public static void LINQ_SingleOrDefault()
  {
   CUI.MainHeadline(nameof(LINQ_SingleOrDefault));
   using (var ctx = new WWWingsContext())
   {
    var flightNo = 101;

    var f = (from x in ctx.FlightSet
             where x.FlightNo == flightNo
             select x).SingleOrDefault();

    if (f != null)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
    else
    {
     Console.WriteLine("Flight not found!");
    }
   } // End using-Block -> Dispose()
  }

  /// <summary>
  /// Find() is supported since EFC 1.1
  /// </summary>
  [EFCBook("Find")]
  public static void LINQ_Find()
  {
   CUI.MainHeadline(nameof(LINQ_Find));
   using (var ctx = new WWWingsContext())
   {
    ctx.FlightSet.ToList(); // Caching all flights in context (here as an example only to show the caching effect!)

    var flightNo = 101;
    var f = ctx.FlightSet.Find(flightNo); // Flight is loaded from cache!

    if (f != null)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }
    else
    {
     Console.WriteLine("Flight not found!");
    }
   } // End using-Block -> Dispose()

  }
 }
}