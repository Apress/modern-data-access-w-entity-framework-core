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
 class Queries
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

  [EFCBook("GroupBy")]
  public static void GroupBy()
  {

   // Simple Group by (Number of flights per destination)
   using (var ctx = new WWWingsContext())
   {
    Console.WriteLine(ctx.Database.GetType().FullName);
    ctx.Log();

    var groups = (from p in ctx.FlightSet
                 group p by p.Departure into g
                 select new { City = g.Key, Count =   g.Count() }).Where(x => x.Count > 5).OrderBy(x => x.Count);
    
    // First roundtrip to the database
    Console.WriteLine("Number of groups: " + groups.Count());

    // Second roundtrip to the database
    foreach (var g in groups.ToList())
    {
     Console.WriteLine(g.City + ": " + g.Count);
    }

   }
   return;

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

  class DepartureGroup
  {
   public string Departure { get; set; }
   public int FlightCount { get; set; }
  }

  /// <summary>
  /// Mapping to non-entity types --> not implemented yet :-(
  /// https://github.com/aspnet/EntityFramework/issues/1862
  /// "Message=Cannot create a DbSet for 'DepartureGrouping' because this type is not included in the model for the context."
  /// </summary>
  [NotYetInTheBook]
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
    var groupSet = ctx.Set<DepartureGroup>().FromSql(sql);
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
    var groupSet = ctx.Set<BO.DepartureGrouping>().FromSql(sql).Where(x=>x.FlightCount>5).OrderBy(x=>x.FlightCount);

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
  
  public static void Demo_Paging()
  {
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
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();

   CUI.MainHeadline("Query with Average() - RAM in EFC 1.x, DB in 2.x");

   var durchschnitt = (from p3 in ctx.FlightSet select p3.Seats.Value).Average(x=>x);

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
              .Where(f=>f.Departure == "Rome")
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
   
   CUI.MainHeadline("Query with Custom Function - RAM :-(");
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

  [NotYetInTheBook]
  public static void Demo_Projection_OneFlight()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline("Projection");

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
    
            };

    var f = q.FirstOrDefault();

    Console.WriteLine(f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    ctx.Attach(f);
    Console.WriteLine(f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
    f.FreeSeats++;
    Console.WriteLine(f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));


    var anz = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + anz);

   }
  }



  public static void Projection_Change()
  {
   using (var ctx = new WWWingsContext())
   {
    CUI.MainHeadline(nameof(Projection_Change));

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

     var anz = ctx.SaveChanges();
     Console.WriteLine("Number of saved changes: " + anz);
     Console.WriteLine("After saving: " + f + " State: " + ctx.Entry(f).State + " Timestamp: " + ByteArrayToString(f.Timestamp));
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
  public static IQueryable<Flight> FilterBySearchParameters(this IQueryable<Flight> collection, int Anz)
  {
   if (Anz <= 0) return collection;
   return collection.Where(f => f.FreeSeats > Anz);
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