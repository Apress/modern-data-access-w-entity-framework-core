using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using BO;
using DA;
using EFC_Console;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
namespace EFC_Console
{
 static internal class ReadLock
 {
  /// <summary>
  /// Update flight with a read lock
  /// </summary>
  [EFCBook("Concurrency, Read Lock")]
  public static void UpdateWithReadLock()
  {
   CUI.MainHeadline(nameof(UpdateWithReadLock));
   Console.WriteLine("--- Change flight");
   int flightNo = 101;
   using (WWWingsContext ctx = new WWWingsContext())
   {
    try
    {
     ctx.Database.SetCommandTimeout(10); // 10 seconds
                                         // Start transaction
     IDbContextTransaction t = ctx.Database.BeginTransaction(IsolationLevel.ReadUncommitted); // default is System.Data.IsolationLevel.ReadCommitted
     Console.WriteLine("Transaction with Level: " + t.GetDbTransaction().IsolationLevel);

     // Load flight with read lock using  WITH (UPDLOCK)
     Console.WriteLine("Load flight using SQL...");
     Flight f = ctx.FlightSet.FromSql("SELECT * FROM dbo.Flight WITH (UPDLOCK) WHERE FlightNo = {0}", flightNo).SingleOrDefault();

     Console.WriteLine($"Before changes: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

     Console.WriteLine("Waiting for ENTER key...");
     Console.ReadLine();

     // Change object in RAM
     Console.WriteLine("Change flight...");
     f.FreeSeats -= 2;

     Console.WriteLine($"After changes: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

     // Send changes to DBMS 
     Console.WriteLine("Save changes...");
     var c = ctx.SaveChanges();
     t.Commit();
     if (c == 0)
     {
      Console.WriteLine("Problem: No changes saved!");
     }
     else
     {
      Console.WriteLine("Number of saved changes: " + c);
      Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);
     }
    }
    catch (Exception ex)
    {
     CUI.PrintError("Error: " + ex.ToString());
    }
   }
  }

  /// <summary>
  /// For test of read lock
  /// </summary>
  [EFCBook("CHECK")]
  public static void ADONET_DataAdapter()
  {
   CUI.MainHeadline(nameof(ADONET_DataAdapter));
   var connstring = new WWWingsContext().Database.GetDbConnection().ConnectionString;

   DbProviderFactory factory =
    DbProviderFactories.GetFactory("System.Data.SqlClient");

   SqlConnection connection = new SqlConnection(connstring);

   using (connection)
   {
    SqlDataAdapter adapter = new SqlDataAdapter(
     "SELECT * FROM dbo.Flight", connection);
    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

    Console.WriteLine(builder.GetUpdateCommand().CommandText);
   }
  }
 }
}