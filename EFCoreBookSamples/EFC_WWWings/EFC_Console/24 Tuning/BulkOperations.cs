using DA;
using BO;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using ITVisions.EFCore;
using Z.EntityFramework.Plus;

namespace EFC_Console
{
 class BulkOperations
 {

  /// <summary>
  /// Create  1000 flights, which you can then delete again with one of the BulkDelete...() methods
  /// Insert will happen in bulk INSERTs as well
  /// </summary>
  [EFCBook("PREPARATION")]
  public static void BulkDelete_Prepare()
  {
   int countNew = 1000;
   CUI.Headline($"Create records {countNew}...");

   using (var ctx = new WWWingsContext())
   {
    int pilotID = ctx.PilotSet.FirstOrDefault().PersonID;
    for (int i = 0; i < countNew; i++)
    {
     // Create flight in RAM
     var f = new Flight();
     f.FlightNo = 20000 + i;
     f.Departure = "Berlin";
     f.Destination = "Sydney";
     f.AirlineCode = "WWW";
     f.PilotId = pilotID;
     f.Seats = 100;
     f.FreeSeats = 100;
     ctx.FlightSet.Add(f);
     // or: ctx.Add(f);  
    }
    var count = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + count);
   }
  }

  [NotYetInTheBook]
  public static void BulkPerformance()
  {

   for (int i = 0; i < 1; i++)
   {
    BulkOperations.BulkDelete_Prepare();
    BulkOperations.BulkDeleteEFPlus();
    BulkOperations.BulkDelete_Prepare();
    BulkOperations.BulkDeleteEFCAPIusingPseudoObject();
    BulkOperations.BulkDelete_Prepare();
    BulkOperations.BulkDeleteADONETCommand();
    BulkOperations.BulkDelete_Prepare();
    BulkOperations.BulkDeleteEFCSQL();
   }

   CUI.PrintSuccess("BulkDeleteEFCSQL: " + BulkOperations.Timer_BulkDeleteEFCSQL / 10);
   CUI.PrintSuccess("BulkDeleteADONETCommand: " + BulkOperations.Timer_BulkDeleteADONETCommand / 10);
   CUI.PrintSuccess("BulkDeleteEFPlus: " + BulkOperations.Timer_BulkDeleteEFPlus / 10);
   CUI.PrintSuccess("BulkDeleteEFCAPIusingPseudoObject: " + BulkOperations.Timer_BulkDeleteADONETCommand / 10);
  }

  [EFCBook()]
  public static void BulkDeleteEFCAPIwithoutBatching()
  {
   CUI.Headline(nameof(BulkDeleteEFCAPIwithoutBatching));
   var sw = new Stopwatch();
   sw.Start();
   int total = 0;
   using (var ctx = new WWWingsContext())
   {
    ctx.Log(null);
    var min = 20000;
    var flightSet = ctx.FlightSet.Where(x => x.FlightNo >= min).ToList();
    foreach (Flight f in flightSet)
    {
     ctx.FlightSet.Remove(f);
     var count = ctx.SaveChanges();
     total += count;
    }
   }
   sw.Stop();
   Console.WriteLine("Number of DELETE statements: " + total);
   Console.WriteLine("Duration: " + sw.ElapsedMilliseconds);
  }


  [EFCBook()]
  public static void BulkDeleteEFCAPIwithBatching()
  {
   CUI.Headline(nameof(BulkDeleteEFCAPIwithBatching));
   int total = 0;
   var sw = new Stopwatch();
   sw.Start();
   using (var ctx = new WWWingsContext())
   {
    var min = 20000;
    var flightSet = ctx.FlightSet.Where(x => x.FlightNo >= min).ToList();
    foreach (Flight f in flightSet)
    {
     ctx.FlightSet.Remove(f);
    }
    total = ctx.SaveChanges();
   }
   sw.Stop();
   Console.WriteLine("Number of DELETE statements: " + total);
   Console.WriteLine("Duration: " + sw.ElapsedMilliseconds);
  }

  /// <summary>
  /// Not possible if activated timestamps!
  /// </summary>
  [EFCBook()]
  public static void BulkDeleteEFCAPIusingPseudoObject()
  {
   CUI.Headline(nameof(BulkDeleteEFCAPIusingPseudoObject));
   int total = 0;
   var sw = new Stopwatch();
   sw.Start();
   using (var ctx = new WWWingsContext())
   {
    for (int i = 20001; i < 21000; i++)
    {
     var f = new Flight() { FlightNo = i };
     ctx.FlightSet.Attach(f);
     ctx.FlightSet.Remove(f);
    }
   total = ctx.SaveChanges();
   }
   sw.Stop();
   Console.WriteLine("Number of DELETE statements: " + total);
   Console.WriteLine("Duration: " + sw.ElapsedMilliseconds);
   Timer_BulkDeleteEFCAPIusingPseudoObject += sw.ElapsedMilliseconds;
  }

  public static long Timer_BulkDeleteADONETCommand;
  [EFCBook()]
  public static void BulkDeleteADONETCommand()
  {
   CUI.Headline(nameof(BulkDeleteADONETCommand));
   int total = 0;
   var min = 20000;
   var sw = new Stopwatch();
   sw.Start();
   using (SqlConnection connection = new SqlConnection(Program.CONNSTRING))
   {
    connection.Open();
    SqlCommand command = new SqlCommand("Delete dbo.Flight where FlightNo >= @min", connection);
    command.Parameters.Add(new SqlParameter("@min", min));
    total = command.ExecuteNonQuery();
   }
   sw.Stop();
   Console.WriteLine("Number of DELETE statements: " + total);
   Console.WriteLine("Duration: " + sw.ElapsedMilliseconds);
   Timer_BulkDeleteADONETCommand += sw.ElapsedMilliseconds;
  }

  public static long Timer_BulkDeleteEFCSQL;
  [EFCBook()]
  public static void BulkDeleteEFCSQL()
  {
   CUI.Headline(nameof(BulkDeleteEFCSQL));
   int total = 0;
   var min = 20000;
   var sw = new Stopwatch();
   sw.Start();
   using (var ctx = new WWWingsContext())
   {
    total = ctx.Database.ExecuteSqlCommand("Delete dbo.Flight where FlightNo >= {0}", min);
   }
   sw.Stop();
   Console.WriteLine("Number of DELETE statements: " + total);
   Console.WriteLine("Duration: " + sw.ElapsedMilliseconds);
   Timer_BulkDeleteEFCSQL += sw.ElapsedMilliseconds;
  }

  public static long Timer_BulkDeleteEFPlus;
  private static long Timer_BulkDeleteEFCAPIusingPseudoObject;

  [EFCBook()]
  public static void BulkDeleteEFPlus()
  {
   CUI.Headline(nameof(BulkDeleteEFPlus));
   int min = 20000;
   int total = 0;
   var sw = new Stopwatch();
   sw.Start();
   using (var ctx = new WWWingsContext())
   {
    ctx.LogAll(verbose: true);
    var count = ctx.FlightSet.Where(x => x.FlightNo >= min).Delete();
    Console.WriteLine("Number of DELETE statements: " + count);
   }
   sw.Stop();
   Console.WriteLine("Duration: " + sw.ElapsedMilliseconds);
   Timer_BulkDeleteEFPlus += sw.ElapsedMilliseconds;
  }

  [EFCBook()]
  public static void BulkUpdateEFPlus()
  {
   CUI.Headline(nameof(BulkUpdateEFPlus));
   using (var ctx = new WWWingsContext())
   {
    var count = ctx.FlightSet.Where(x => x.Departure == "Berlin" && x.Date >= DateTime.Now).Update(x => new Flight() { FreeSeats = (short)(x.FreeSeats - 1) });
    Console.WriteLine("Changed records: " + count);
   }
  }
 }
}