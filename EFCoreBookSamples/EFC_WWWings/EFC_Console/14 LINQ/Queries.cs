using BO;
using DA;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC_Console
{
 public class Queries
 {

  [NotYetInTheBook]
  public static void GeneratedSQL()
  {
   CUI.MainHeadline(nameof(GeneratedSQL));

   var ctx = new WWWingsContext();

   var flightSet1 = ctx.FlightSet.ToList();

   var flightSet2 = ctx.FlightSet.Where(f2 => f2.Departure == "Rome").ToList();

   var flightSet3 = (from f3 in ctx.FlightSet
                     where f3.Departure.StartsWith("M")
                     select f3)
                 .ToList();

   var flightSet4 = (from f4 in ctx.FlightSet
                     where f4.Departure.StartsWith("M")
                     orderby f4.FlightNo
                     select f4).Skip(5).Take(10).ToList();

  }


  /// <summary>
  /// 2.1 Preview 1: An aggregate may not appear in the WHERE clause unless it is in a subquery contained in a HAVING clause or a select list, and the column being aggregated is an outer reference.
  /// </summary>
  [EFCBook("GroupBy")]
  public static void GroupBy1()
  {
   CUI.MainHeadline(nameof(GroupBy1));

   // Simple Group by: Min, Max, Sum und Average of FreeSeats per Destination
   using (var ctx = new WWWingsContext())
   {
    Console.WriteLine(ctx.Database.GetType().FullName);
    ctx.Log();

    var groups1 = (from p in ctx.FlightSet
                   group p by p.Departure into g
                   select new { City = g.Key, Min = g.Min(x => x.FreeSeats), Max = g.Max(x => x.FreeSeats), Sum = g.Sum(x => x.FreeSeats), Avg = g.Average(x => x.FreeSeats) });
    var count1 = groups1.Count();
    Console.WriteLine("Number of groups: " + count1);
    if (count1 > 0)
    {
     // Second roundtrip to the database
     foreach (var g in groups1.ToList())
     {
      Console.WriteLine(g.City + ": Min=" + g.Min + " / Max=" + g.Max + " / Avergae" + g.Avg + " / Sum:" + g.Sum);
     }
    }

   }
  }

  [EFCBook("GroupBy")]
  public static void GroupBy2()
  {
   CUI.MainHeadline(nameof(GroupBy2));

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    //Simple Group by(Number of flights per destination)
    // BUG in EFCore 2.1 Preview 1
    var groups2 = (from p in ctx.FlightSet
                   group p by p.Departure into g
                   select new { City = g.Key, Count = g.Count() }).Where(x => x.Count > 5).OrderBy(x => x.Count).ToList();

    // First roundtrip to the database (done intentionally here!)
    var count2 = groups2.Count();
    Console.WriteLine("Number of groups: " + count2);
    if (count2 > 0)
    {
     // Second roundtrip to the database
     foreach (var g in groups2.ToList())
     {
      Console.WriteLine(g.City + ": " + g.Count);
     }
    }

   }
  }

     [EFCBook("GroupBy")]
  public static void GroupBy3()
  {
   CUI.MainHeadline(nameof(GroupBy3));
   #region nested Groupy by
   using (var ctx = new WWWingsContext())
   {
    //ctx.Log();

    var groupSet = (from p in ctx.FlightSet
                    orderby p.FreeSeats
                    group p by p.Departure into g
                    select g);

    Console.WriteLine("Number of groups: " + groupSet.Count());

    foreach (var g in groupSet)
    {
     Console.WriteLine(g.Key + ": " + g.Count());
     foreach (var f in g)
     {
      // do something...
      // Console.WriteLine("   " + f.FlightNo);
     }
    }

   }
   #endregion
  }


  /// <summary>
  /// Works with EFC >= 2.1 Preview1
  /// https://github.com/aspnet/EntityFramework/issues/1862
  /// older Version using ctx.Set<T>(): "Message=Cannot create a DbSet for 'DepartureGrouping' because this type is not included in the model for the context."
  /// </summary>
  [EFCBook]
  public static void GroupBy_SQL_NonEntityType()
  {

   // Get the number of flights per Departure
   using (var ctx = new WWWingsContext())
   {
    // Map SQL to non-entity class
    Console.WriteLine(ctx.Database.GetType().FullName);
    ctx.Log();
    var sql = "SELECT Departure, COUNT(FlightNo) AS FlightCount FROM Flight GROUP BY Departure";
    // ERROR!!! Cannot create a DbSet for 'Group' because this type is not included in the model for the context."
    var groupSet = ctx.Query<DepartureGroup>().FromSql(sql);
    // Output
    foreach (var g in groupSet)
    {
     Console.WriteLine(g.Departure + ": " + g.FlightCount);
    }
   }
   return;

   // Groupy by "nested"
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    var groupSet = from p in ctx.FlightSet
                   orderby p.FreeSeats
                   group p by p.Departure into g
                   select g;

    Console.WriteLine("Count: " + groupSet.Count());

    foreach (var g in groupSet)
    {
     Console.WriteLine(g.Key + ": " + g.Count());
     foreach (var f in g)
     {
      Console.WriteLine("   " + f.FlightNo);
     }
    }

   }
  }

  /// <summary>
  /// geht mit Pseudo-Entitätsklasse "DepartureGrouping"
  /// </summary>
  public static void GroupBy_SQL_Trick()
  {
   CUI.MainHeadline(nameof(GroupBy_SQL_Trick));
   // Get the number of flights per Departure
   using (var ctx = new WWWingsContext())
   {
    Console.WriteLine(ctx.Database.GetType().FullName);
    ctx.Log();

    // Map SQL to entity class
    var sql = "SELECT Departure, COUNT(FlightNo) AS FlightCount FROM Flight GROUP BY Departure";
    var groupSet = ctx.Set<BO.DepartureGrouping>().FromSql(sql).Where(x => x.FlightCount > 5).OrderBy(x => x.FlightCount);

    // Output
    foreach (var g in groupSet)
    {
     Console.WriteLine(g.Departure + ": " + g.FlightCount);
    }

   }
   return;

   // Groupy by "nested"
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();

    var groupSet = from p in ctx.FlightSet
                   orderby p.FreeSeats
                   group p by p.Departure into g
                   select g;

    Console.WriteLine("Count: " + groupSet.Count());

    foreach (var g in groupSet)
    {
     Console.WriteLine(g.Key + ": " + g.Count());
     foreach (var f in g)
     {
      Console.WriteLine("   " + f.FlightNo);
     }
    }

   }
  }

  public static void LINQ_ToString()
  {

   CUI.MainHeadline(nameof(LINQ_ToString));

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();


    var q1 = from p in ctx.FlightSet
             where p.FlightNo < 10
             orderby p.FreeSeats
             select p;

    EntityQueryable<Flight> q1b = (EntityQueryable<Flight>)q1;

    Console.WriteLine(q1.ToString()); // EF: returns SQL. EFC: returns type :-(
    Console.WriteLine(q1b.ToString()); // EF: returns SQL. EFC: returns type :-(
   }

  }

  public static void Paging()
  {
   CUI.MainHeadline(nameof(Paging));

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();

    CUI.MainHeadline("Query with Skip() without OrderBy / with OFFSET and FETCH");
    var q5 = (from p5 in ctx.FlightSet
              where p5.Departure.StartsWith("M")
              select p5).Skip(5).Take(10);

    var l5 = q5.ToList();

    Console.WriteLine("Count: " + l5.Count());
   }
  }

  [EFCBook("Average, Union, AddDays, ToString")]
  public static void LINQ_RAMQueries()
  {
   CUI.MainHeadline(nameof(LINQ_RAMQueries));

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();

    CUI.MainHeadline("Query with Average() - RAM in EFC 1.x, DB in 2.x");

    var durchschnitt = (from p3 in ctx.FlightSet select p3.Seats.Value).Average(x => x);

    Console.WriteLine("Average: " + durchschnitt);

    CUI.MainHeadline("Query with Union() - RAM in EFC 1.x and EFC 2.0 :-(");

    var alleOrte = (from f in ctx.FlightSet select f.Departure).Union(from f in ctx.FlightSet select f.Destination).Count();

    Console.WriteLine("Number of cities: " + alleOrte);

    CUI.MainHeadline("Query with AddDays() - RAM in EFC 1.x, DB in 2.x");

    var q3 = from f in ctx.FlightSet
             where f.FreeSeats > 0 &&
             f.Date > DateTime.Now.AddDays(10)
             orderby f.FlightNo
             select f;

    List<Flight> l3 = q3.Take(10).ToList();

    Console.WriteLine("Count: " + l3.Count);

    foreach (var f in l3)
    {
     Console.WriteLine(f);
    }

    CUI.MainHeadline("Query with ToString() - RAM in 1.x, DB in 2.x :-(");
    var q2 = from f in ctx.FlightSet
             where f.FlightNo > 100
             && f.FreeSeats.ToString().Contains("1")
             orderby f.FlightNo
             select f;

    List<Flight> l2 = q2.Take(10).ToList();

    Console.WriteLine("Count: " + l2.Count);

    foreach (var f in l2)
    {
     Console.WriteLine(f);
    }


    // fixed in 2.0 ! https://github.com/aspnet/EntityFramework/issues/7234
    var q5 = (from f in ctx.FlightSet
              select new DTO { Ort = f.Departure }).Distinct().ToList();
    foreach (var f in q5.ToList())
    {
     Console.WriteLine(f);
    }

    // fixed in 2.0 ? https://github.com/aspnet/EntityFramework/issues/7122
    var q6 = (from p5 in ctx.FlightSet
              .Where(f => f.Departure == "Rome")
              .FilterBySearchParameters(10)
              select p5);
    foreach (var f in q6.ToList())
    {
     Console.WriteLine(f);
    }


   }
  }

  class DTO
  {
   public string Ort { get; set; }

  }


  private static int GetNumberOfDaysUntil(DateTime t)
  {
   return (t - DateTime.Now).Days;
  }

  /// <summary>
  /// Custom Functions in LINQ
  /// </summary>
  [EFCBook("Client Evaluation, Custom Function")]
  public static void LINQ_CustomFunction()
  {

   CUI.MainHeadline(nameof(LINQ_CustomFunction));
   using (var ctx = new WWWingsContext())
   {

    var q4 = from f in ctx.FlightSet
             where f.FreeSeats > 0 &&
                   GetNumberOfDaysUntil(f.Date) > 10
             orderby f.FlightNo
             select f;

    List<Flight> l4 = q4.Take(10).ToList();

    Console.WriteLine("Count: " + l4.Count);

    foreach (var f in l4)
    {
     Console.WriteLine(f);
    }
   }
  }

  [EFCBook]
  public static void Projection_OneFlight()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline(nameof(Projection_OneFlight));

    var q = from x in ctx.FlightSet
            where x.FlightNo == 101
            orderby x.FlightNo
            select new Flight()
            {
             FlightNo = x.FlightNo,
             Date = x.Date,
             Departure = x.Departure,
             Destination = x.Destination,
             Seats = x.Seats,
             FreeSeats = x.FreeSeats,
             Timestamp = x.Timestamp // !important!
            };

    var f = q.FirstOrDefault();

    Console.WriteLine(f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    ctx.Attach(f);
    Console.WriteLine(f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    f.FreeSeats++;
    Console.WriteLine(f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));


    var count = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + count);

   }
  }


  [EFCBook]
  public static void Projection_EntityType()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline(nameof(Projection_EntityType));

    var q = (from f in ctx.FlightSet
             where f.FlightNo > 100
             orderby f.FlightNo
             select new Flight()
             {
              FlightNo = f.FlightNo,
              Date = f.Date,
              Departure = f.Departure,
              Destination = f.Destination,
              FreeSeats = f.FreeSeats,
              Timestamp = f.Timestamp
             }).Take(2);

    var flightSet = q.ToList();

    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }

    Console.WriteLine("Number of flights: " + flightSet.Count);

    foreach (var f in flightSet)
    {
     Console.WriteLine("Before attach: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
     ctx.Attach(f);
     Console.WriteLine("After attach: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
     f.FreeSeats--;
     Console.WriteLine("After Änderung: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));

     var count = ctx.SaveChanges();
     Console.WriteLine("Number of saved changes: " + count);
     Console.WriteLine("After saving: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    }
   }
  }



  [EFCBook]
  public static void Projection_AnonymousType()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline(nameof(Projection_AnonymousType));

    var q = (from f in ctx.FlightSet
             where f.FlightNo > 100
             orderby f.FlightNo
             select new
             {
              FlightID = f.FlightNo,
              f.Date,
              f.Departure,
              f.Destination,
              f.FreeSeats,
              f.Timestamp
             }).Take(2);

    var flightSet = q.ToList();

    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightID} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }

    Console.WriteLine("Number of flights: " + flightSet.Count);

    foreach (var f in flightSet)
    {
     Console.WriteLine(f.FlightID);
     // not posssible:  Console.WriteLine("Before attach: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
     // not posssible:   ctx.Attach(f);
     // not posssible:   Console.WriteLine("After attach: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
     // not posssible: 
     // f.FreeSeats--;
     // not posssible:   Console.WriteLine("After Änderung: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));

     var count = ctx.SaveChanges(); // no changes can be saved
     Console.WriteLine("Number of saved changes: " + count);
     // not posssible:  Console.WriteLine("After saving: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    }
   }
  }


  class FlightDTO
  {
   public int FlightID { get; set; }
   public DateTime Date { get; set; }
   public string Departure { get; set; }
   public string Destination { get; set; }
   public short? FreeSeats { get; set; }
   public byte[] Timestamp { get; set; }
  }

  [EFCBook]
  public static void Projection_DTO()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline(nameof(Projection_DTO));

    var q = (from f in ctx.FlightSet
             where f.FlightNo > 100
             orderby f.FlightNo
             select new FlightDTO()
             {
              FlightID = f.FlightNo,
              Date = f.Date,
              Departure = f.Departure,
              Destination = f.Destination,
              FreeSeats = f.FreeSeats,
              Timestamp = f.Timestamp
             }).Take(2);

    var flightSet = q.ToList();

    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightID} from {f.Departure} to {f.Destination} has {f.FreeSeats} free seats!");
    }

    Console.WriteLine("Number of flights: " + flightSet.Count);

    foreach (var f in flightSet)
    {
     Console.WriteLine(f.FlightID);
     // not posssible:  Console.WriteLine("Before attach: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
     // not posssible:   ctx.Attach(f);
     // not posssible:   Console.WriteLine("After attach: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
     // not posssible: 
     // f.FreeSeats--;
     // not posssible:   Console.WriteLine("After Änderung: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));

     var count = ctx.SaveChanges(); // no changes can be saved
     Console.WriteLine("Number of saved changes: " + count);
     // not posssible:  Console.WriteLine("After saving: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    }
   }
  }

  public static string ByteArrayToString(byte[] ba)
  {
   if (ba == null) return "";
   string hex = BitConverter.ToString(ba);
   return hex.Replace("-", "");
  }
 }

 public static class Extension
 {
  public static IQueryable<Flight> FilterBySearchParameters(this IQueryable<Flight> collection, int count)
  {
   if (count <= 0) return collection;
   return collection.Where(f => f.FreeSeats > count);
  }


  /// <summary>
  /// EFC 2.0
  /// </summary>
  public static void Like()
  {

   using (var ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.Where(f => EF.Functions.Like("Departure", "%o%")).ToList();
    flightSet.ForEach(f => Console.WriteLine(f));
   }
  }
 }
}