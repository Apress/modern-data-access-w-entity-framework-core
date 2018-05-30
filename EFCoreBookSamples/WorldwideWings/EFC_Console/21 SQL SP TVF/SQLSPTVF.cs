using System;
using System.Linq;
using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using ITVisions.EFCore;
using System.Collections.Generic;
using BO;
using ITVisions.EFC;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFC_Console
{
 class SQLSPTVF
 {

  /// <summary>
  /// Dangerous: SQL Injection!
  /// </summary>
  [EFCBook()]
  public static void Demo_SQLDirect1()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirect1));
   string departure = "Berlin";
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    IQueryable<Flight> flightSet = ctx.FlightSet.FromSql("Select * from Flight where Departure='" + departure + "'");
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }

  /// <summary>
  /// Right eay if SQL composition
  /// </summary>
  [EFCBook()]
  public static void Demo_SQLDirect2()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirect2));
   string departure = "Berlin";
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    IQueryable<Flight> flightSet = ctx.FlightSet.FromSql("Select * from Flight where Departure={0}", departure);
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }


  /// <summary>
  // SQL composition using string interpolation (since EFC 2.0 Preview 2)
  /// </summary>
  [EFCBook()]
  public static void Demo_SQLDirect3()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirect3));
   string departure = "Berlin";
   string destination = "Rome";
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    IQueryable<Flight> flightSet = ctx.FlightSet.FromSql($@"Select * from Flight where Departure={departure} and Destination={destination}");
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }

  /// <summary>
  // ToList() ensures that there is only one roundtrip to the database.
  /// </summary>
  [EFCBook]
  public static void Demo_SQLDirect4()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirect4));
   string departure = "Berlin";
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    List<Flight> flightSet = ctx.FlightSet.FromSql($@"Select * from Flight where Departure={departure}").ToList();
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }


  /// <summary>
  /// Compose SQL and LINQ
  /// </summary>
  [EFCBook]
  public static void Demo_SQLDirectAndLINQComposition()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirectAndLINQComposition));
   string departure = "Berlin";
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    var flightSet = ctx.FlightSet.FromSql("Select * from Flight where Departure={0}", departure).Include(f => f.Pilot).Where(x => x.FreeSeats > 10).OrderBy(x => x.FreeSeats).ToList();
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }


  /// <summary>
  /// Weglassen einer Spalte geht noch nicht. Er will All Spalten, auch die optionalen :-( -> The required column 'Memo' was not present in the results of a 'FromSql' operation.
  /// </summary>
  [EFCBook]
  public static void Demo_SQLDirect_Projection()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirect_Projection));

   using (var ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.FromSql("Select FlightNo, Departure, Destination, strikebound, FlightDate,CopilotId,pilotid, AircraftTypeID, AirlineCode, memo, Seats, Freeseats, Timestamp, LastChange, NonSmokingFlight, Price, Utilization from Flight where Departure={0}", "Berlin"); //memo fehlt --> The required column 'Memo' was not present in the results of a 'FromSql' operation.
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight + ": " + ctx.Entry(flight).State);
    }
    Console.WriteLine(flightSet.Count());
   }

  }

  /// <summary>

  /// </summary>
  [EFCBook]
  public static void Demo_ExecuteSqlQuery()
  {
   // geht momentan gar nicht!
   CUI.MainHeadline(nameof(Demo_ExecuteSqlQuery));
   using (var ctx = new WWWingsContext())
   {
    var liste = ctx.Database.ExecuteSqlQuery("Select * from Flight");
    var dr = liste.DbDataReader;
    while (dr.Read())
    {
     Console.WriteLine(dr["FlightNo"]);
    }
   }
  }


  /// <summary>
  /// Use of a stored procedure that delivers Flight records
  /// </summary>
  [EFCBook]
  public static void Demo_SP()
  {
   CUI.MainHeadline(nameof(Demo_SP));

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    var flightSet = ctx.FlightSet.FromSql("EXEC GetFlightsFromSP {0}", "Berlin").Where(x => x.FreeSeats > 0).ToList();
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }

  /// <summary>
  /// Usage of a Table Value Function that delivers Flight records
  /// </summary>
  [EFCBook]
  public static void Demo_TVF()
  {
   CUI.MainHeadline(nameof(Demo_TVF));
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    var flightSet = ctx.FlightSet.FromSql("Select * from GetFlightsFromTVF({0})", "Berlin").Where(x => x.FreeSeats > 10).ToList();
    Console.WriteLine(flightSet.Count());
    foreach (var flight in flightSet)
    {
     Console.WriteLine(flight);
    }
    Console.WriteLine(flightSet.Count());
   }
  }

  class fligthDTO
  {
   public int FlightNo { get; set; }
   public string Departure { get; set; }
   public string Destination { get; set; }
   public DateTime Date { get; set; }
  }

  /// <summary>
  /// Not yet possible: Non-entity classes as a result set
  /// </summary>
  [EFCBook]
  public static void Demo_SQLDirekt_NonEntityType()
  {
   CUI.MainHeadline(nameof(Demo_SQLDirekt_NonEntityType));

   using (var ctx = new WWWingsContext())
   {
    // this does not compile:  var flightSet = ctx.FromSql<fligthDTO>("Select FlightNo, Departure, Destination, Date from Flight");
    var flightSet = ctx.Set<fligthDTO>().FromSql("Select FlightNo, Departure, Destination, Date from Flight");
    Console.WriteLine(flightSet.Count());
    foreach (var f in flightSet)
    {
     Console.WriteLine(f);
    }
    Console.WriteLine(flightSet.Count());
   }
  }


  /// <summary>
  /// Uses the extension method ExecuteSqlQuery
  /// </summary>
  [EFCBook]
  public static void Demo_Datareader()
  {
   CUI.MainHeadline(nameof(Demo_Datareader));
   string Ort = "Berlin";
   using (var ctx = new WWWingsContext())
   {
    RelationalDataReader rdr = ctx.Database.ExecuteSqlQuery("Select * from Flight where Departure={0}", Ort);
    DbDataReader dr = rdr.DbDataReader;
    while (dr.Read())
    {
     Console.WriteLine("{0}\t{1}\t{2}\t{3} \n", dr[0], dr[1], dr[2], dr[3]);
    }
    dr.Dispose();
   }
  }


  /// <summary>
  /// ExecuteSqlCommand
  /// </summary>
  [EFCBook]
  public static void Demo_SqlCommand()
  {
   CUI.MainHeadline(nameof(Demo_SqlCommand));
   using (var ctx = new WWWingsContext())
   {
    var count = ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo > {0}", 10000);
    Console.WriteLine("Number of deleted records: " + count);
   }
  }

 }
}
