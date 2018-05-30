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
 class EFC_LINQ_CheatSheet
 {
  static WWWingsContext ctx = new WWWingsContext();

  static public void Run()
  {
   ctx.Log();

   // ========================= Simple SELECT Commands (All Records)
   CUI.Headline("All records as Array<T>");
   Flight[] flightSet0a = (from f in ctx.FlightSet select f).ToArray();
   Flight[] flightSet0b = ctx.FlightSet.ToArray();

   CUI.Headline("All records as List<T>");
   List<Flight> flightSet1a = (from f in ctx.FlightSet select f).ToList();
   List<Flight> flightSet1b = ctx.FlightSet.ToList();

   CUI.Headline("All records as Dictionary<T, T>");
   Dictionary<int, Flight> flightSet2a = (from f in ctx.FlightSet select f).ToDictionary(f=>f.FlightNo, f=>f);
   Dictionary<int, Flight> flightSet2b = ctx.FlightSet.ToDictionary(f => f.FlightNo, f => f);

   CUI.Headline("All records as ILookup<T, T>");
   ILookup<int, Flight> flightSet2c = (from f in ctx.FlightSet select f).ToLookup(f => f.FlightNo, f => f);
   ILookup<int, Flight> flightSet2d = ctx.FlightSet.ToLookup(f => f.FlightNo, f => f);

   // ========================= 
   CUI.Headline("Conditions");
   List<Flight> flightSet3a = (from f in ctx.FlightSet
                           where f.Departure == "Berlin" &&
                           (f.Destination.StartsWith("Rome") || f.Destination.Contains("Paris"))
                           && f.FreeSeats > 0
                           select f)
                           .ToList();

   List<Flight> flightSet3b = ctx.FlightSet.Where(f => f.Departure == "Berlin" &&
                           (f.Destination.StartsWith("Rome") || f.Destination.Contains("Paris"))
                           && f.FreeSeats > 0)
                           .ToList();

   // ========================= 
   CUI.Headline("Contains");
   List<string> Orte = new List<string>() { "Berlin", "Hamburg", "Köln", "Berlin" };
   List<Flight> flightSet4a = (from f in ctx.FlightSet
                           where Orte.Contains(f.Departure)
                           select f)
                          .ToList();
   List<Flight> flightSet4b = ctx.FlightSet.Where(f => Orte.Contains(f.Departure)).ToList();

   // ========================= 
   CUI.Headline("Sorting");
   List<Flight> flightSet5a = (from f in ctx.FlightSet
                           where f.Departure == "Berlin"
                           orderby f.Date, f.Destination, f.FreeSeats descending
                           select f).ToList();
   List<Flight> flightSet5b = ctx.FlightSet.Where(f => f.Departure == "Berlin")
                           .OrderBy(f => f.Date)
                           .ThenBy(f => f.Destination)
                           .ThenByDescending(f => f.FreeSeats)
                           .ToList();

   // ========================= 
   CUI.Headline("Paging");
   List<Flight> flightSet6a = (from f in ctx.FlightSet
                           where f.Departure == "Berlin"
                           orderby f.Date
                           select f).Skip(100).Take(10).ToList();
   List<Flight> flightSet6b = ctx.FlightSet.Where(f => f.Departure == "Berlin")
                           .OrderBy(f => f.Date)
                           .Skip(100).Take(10).ToList();

   // ========================= 
   CUI.Headline("Projection to entity type");

   List<Flight> flightSet7a = (from f in ctx.FlightSet
                           where f.Departure == "Berlin"
                           orderby f.Date
                           select new Flight()
                           {
                            FlightNo = f.FlightNo,
                            Date = f.Date,
                            Departure = f.Departure,
                            Destination = f.Destination,
                            FreeSeats = f.FreeSeats,
                            Timestamp = f.Timestamp
                           }).ToList();

   List<Flight> flightSet7b = ctx.FlightSet
                           .Where(f => f.Departure == "Berlin")
                           .OrderBy(f => f.Date)
                           .Select(f => new Flight()
                           {
                            FlightNo = f.FlightNo,
                            Date = f.Date,
                            Departure = f.Departure,
                            Destination = f.Destination,
                            FreeSeats = f.FreeSeats,
                            Timestamp = f.Timestamp
                           }).ToList();
   // ========================= 
   CUI.Headline("Aggregate");

   int agg1a = (from f in ctx.FlightSet select f).Count();
   int? agg2a = (from f in ctx.FlightSet select f).Sum(f => f.FreeSeats);
   int? agg3a = (from f in ctx.FlightSet select f).Min(f => f.FreeSeats);
   int? agg4a = (from f in ctx.FlightSet select f).Max(f => f.FreeSeats);
   double? agg5a = (from f in ctx.FlightSet select f).Average(f => f.FreeSeats);

   int agg1b = ctx.FlightSet.Count();
   int? agg2b = ctx.FlightSet.Sum(f => f.FreeSeats);
   int? agg3b = ctx.FlightSet.Min(f => f.FreeSeats);
   int? agg4b = ctx.FlightSet.Max(f => f.FreeSeats);
   double? agg5b = ctx.FlightSet.Average(f => f.FreeSeats);

   // ========================= 
   CUI.Headline("Grouping");
   var group1a = (from f in ctx.FlightSet
                   group f by f.Departure into g
                   select new { City = g.Key, Count = g.Count(), Sum = g.Sum(f => f.FreeSeats), Avg = g.Average(f => f.FreeSeats) })
                  .ToList();

   var group1b = ctx.FlightSet
                 .GroupBy(f => f.Departure)
                 .Select(g => new
                 {
                  City = g.Key,
                  Count = g.Count(),
                  Sum = g.Sum(f => f.FreeSeats),
                  Avg = g.Average(f => f.FreeSeats)
                 }).ToList();

   // ========================= 
   CUI.Headline("Single object");
   Flight flight1a = (from f in ctx.FlightSet select f).SingleOrDefault(f => f.FlightNo == 101);

   Flight flight1b = ctx.FlightSet.SingleOrDefault(f => f.FlightNo == 101);

   Flight flight2a = (from f in ctx.FlightSet
                  where f.FreeSeats > 0
                  orderby f.Date
                  select f).FirstOrDefault();

   Flight flight2b = ctx.FlightSet
                 .Where(f => f.FreeSeats > 0)
                 .OrderBy(f => f.Date)
                 .FirstOrDefault();

   // ========================= 
   CUI.Headline("Related objects");

   List<Flight> flightDetailsSet1a = (from f in ctx.FlightSet
                                  .Include(f => f.Pilot)
                                  .Include(f => f.BookingSet).ThenInclude(b => b.Passenger)
                                  where f.Departure == "Berlin"
                                  orderby f.Date
                                  select f)
                                  .ToList();

   List<Flight> flightDetailsSet1b = ctx.FlightSet
                                 .Include(f => f.Pilot)
                                 .Include(f => f.BookingSet).ThenInclude(b => b.Passenger)
                                 .Where(f => f.Departure == "Berlin")
                                 .OrderBy(f => f.Date)
                                 .ToList();


   CUI.Headline("Inner Join");

   // ========================= 
   var flightDetailsSet2a = (from f in ctx.FlightSet
                           join p in ctx.PilotSet
                           on f.FlightNo equals p.PersonID
                           select new { Nr = f.FlightNo, flight = f, Pilot = p })
                                  .ToList();

   var flightDetailsSet2b = ctx.FlightSet
                          .Join(ctx.PilotSet, f => f.FlightNo, p => p.PersonID,
                          (f, p) => new { Nr = f.FlightNo, flight = f, Pilot = p })
                          .ToList();

   // ========================= 
   CUI.Headline("Cross Join (cartesian product)");
   
   var flightDetailsSet3a = (from f in ctx.FlightSet
                           from b in ctx.BookingSet
                           from p in ctx.PassengerSet
                           where f.FlightNo == b.FlightNo && b.PassengerID == p.PersonID && f.Departure == "Rome"
                           select new { flight = f, passengers = p })
                           .ToList();

   var flightDetailsSet3b = ctx.FlightSet
      .SelectMany(f => ctx.BookingSet, (f, b) => new  { f = f,  b = b})
      .SelectMany(z => ctx.PassengerSet, (x, p) => new {x = x, p = p})
      .Where(y => ((y.x.f.FlightNo == y.x.b.FlightNo) &&
                        (y.x.b.PassengerID == y.p.PersonID)) && y.x.f.Departure == "Rome")
      .Select(z => new {flight = z.x.f, passengers = z.p } )
      .ToList();

   foreach (var x in flightDetailsSet3b)
   {
    Console.WriteLine(x.flight.ToString() + ": " + x.passengers.ToString());
   }

   // ========================= 
   CUI.Headline("Join with grouping (!!!will do Lazy Loading of the passengers!!!)");

   var flightDetailsSet4a = (from b in ctx.BookingSet
                           join f in ctx.FlightSet on b.FlightNo equals f.FlightNo
                           join p in ctx.PassengerSet on b.PassengerID equals p.PersonID
                           where f.Departure == "Berlin"
                           group b by b.Flight into g
                           select new { flight = g.Key, passengers = g.Select(x => x.Passenger) })
                           .ToList();

   
   var flightDetailsSet4b = ctx.BookingSet
                             .Join(ctx.FlightSet, b => b.FlightNo, f => f.FlightNo, (b, f) => new { b = b, f = f })
                             .Join(ctx.PassengerSet, x => x.b.PassengerID, p => p.PersonID, (x, p) => new { x = x, p = p })
                             .Where(z => (z.x.f.Departure == "Berlin"))
                             .GroupBy(y => y.x.b.Flight, y => y.x.b)
                             .Select(g => new { flight = g.Key, passengers = g.Select(x => x.Passenger) })
                             .ToList();

   
   foreach (var x in flightDetailsSet4b)
   {
    Console.WriteLine(x.flight);
    foreach (var p in x.passengers)
    {
     Console.WriteLine(" - " + p);
    }
   }

   // ========================= 
   CUI.Headline("Subquery");

   // Attention: Subqueries are executed individually for each result data record of the main query!
   List<Flight> flightDetailsSet5a = (from f in ctx.FlightSet
         where f.FlightNo == 101
         select new Flight()
         {
          FlightNo = f.FlightNo,
          Date = f.Date,
          Departure = f.Departure,
          Destination = f.Destination,
          FreeSeats = f.FreeSeats,
          Timestamp = f.Timestamp,
          Pilot = (from p in ctx.PilotSet where 
                   p.PersonID == f.PilotId select p)
                   .FirstOrDefault(),
          Copilot = (from p in ctx.PilotSet where 
                     p.PersonID == f.CopilotId select p)
                     .FirstOrDefault(),
         }).ToList();


   List<Flight> flightDetailsSet5b = ctx.FlightSet.Where(f => f.FlightNo == 101)
   .Select(f =>new Flight()
         {
          FlightNo = f.FlightNo,
          Date = f.Date,
          Departure = f.Departure,
          Destination = f.Destination,
          FreeSeats = f.FreeSeats,
          Timestamp = f.Timestamp,
          Pilot = ctx.PilotSet
               .Where(p => (p.PersonID == f.PilotId))
               .FirstOrDefault(),
          Copilot = ctx.PilotSet
               .Where(p => (p.PersonID) == f.CopilotId)
               .FirstOrDefault()
         }
   ).ToList();

   foreach (var x in flightDetailsSet5b)
   {
    Console.WriteLine(x.FlightNo + ": " + x.Pilot.Surname + "/" + x.Copilot.Surname);
   }

   //var flightDetailsSet3b = ctx.FlightSet
   //                       .GroupJoin(ctx.PilotSet, f => f.FlightNo, p => p.FlightHours,
   //                       (f, pset) => new { Nr = f.FlightNo, Flight = f, Pilots = pset })
   //                       .ToList();

   //foreach (var f in flightDetailsSet3a)
   //{

   // Console.WriteLine(f.Flight);
   // foreach (var p in f.Passagiere)
   // {
   //  Console.WriteLine(p);
   // }
   //}

   // not supported!
   //ctx.FlightSet.SkipWhile(f => f.FreeSeats < 200).ToList();


  }
 }
}
