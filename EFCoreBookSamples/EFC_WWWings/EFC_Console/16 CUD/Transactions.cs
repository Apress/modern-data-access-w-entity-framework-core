using System;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using DA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using ITVisions;

//using Microsoft.EntityFrameworkCore.SqlServer;

namespace EFC_Console
{
 class Transactions
 {

  /// <summary>
  /// a transaction over two changes to a flight, each of which is independently persisted with SaveChanges ().
  /// </summary>
  [EFCBook("2.0")]
  public static void ExplicitTransactionTwoSaveChanges()
  {

   CUI.MainHeadline(nameof(ExplicitTransactionTwoSaveChanges));
   using (var ctx = new WWWingsContext())
   {
    // Start transaction. Default is System.Data.IsolationLevel.ReadCommitted
    using (var t = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
    {
     // Print isolation level
     RelationalTransaction rt = t as RelationalTransaction;
     DbTransaction dbt = rt.GetDbTransaction();
     Console.WriteLine("Transaction with Level: " + dbt.IsolationLevel);

     // Read data
     int flightNo = ctx.FlightSet.OrderBy(x => x.FlightNo).FirstOrDefault().FlightNo;
     var f = ctx.FlightSet.Where(x => x.FlightNo == flightNo).SingleOrDefault();

     Console.WriteLine("Before: " + f.ToString());

     // Change data and save
     f.FreeSeats--;
     var count1 = ctx.SaveChanges();
     Console.WriteLine("Number of saved changes: " + count1);

     //  Change data again and save
     f.Memo = "last changed at " + DateTime.Now.ToString();
     var count2 = ctx.SaveChanges();
     Console.WriteLine("Number of saved changes: " + count2);

     Console.WriteLine("Commit or Rollback? 1 = Commit, other = Rollback");
     var eingabe = Console.ReadKey().Key;
     if (eingabe == ConsoleKey.D1)
     { t.Commit(); Console.WriteLine("Commit done!"); }
     else
     { t.Rollback(); Console.WriteLine("Rollback done!"); }

     Console.WriteLine("After in RAM: " + f.ToString());
     ctx.Entry(f).Reload();
     Console.WriteLine("After in DB: " + f.ToString());
    }
   }
  }

  /// <summary>
  /// a transaction via a change to the table Booking (insert booking) and to the table Flight (reduce number of free seats).
  /// ATTENTION: Does not work with EF Profiler!
  /// </summary>
  [EFCBook()]
  public static void ExplicitTransactionTwoContextInstances()
  {
 CUI.MainHeadline(nameof(ExplicitTransactionTwoContextInstances));

   // Open shared connection
   using (var connection = new SqlConnection(Program.CONNSTRING))
   {
    connection.Open();
    // Start transaction. Default is System.Data.IsolationLevel.ReadCommitted
    using (var t = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
    {
     // Print isolation level
     Console.WriteLine("Transaction with Level: " + t.IsolationLevel);
     int flightNo;

     using (var ctx = new WWWingsContext(connection))
     {
      ctx.Database.UseTransaction(t);
      var all = ctx.FlightSet.ToList();
 
      var flight = ctx.FlightSet.Find(111);
      flightNo = flight.FlightNo;
      ctx.Database.ExecuteSqlCommand("Delete from booking where flightno= " + flightNo);
      var pasID = ctx.PassengerSet.FirstOrDefault().PersonID;

      // Create and persist booking
      var b = new BO.Booking();
      b.FlightNo = flightNo;
      b.PassengerID = pasID;
      ctx.BookingSet.Add(b);
      var count1 = ctx.SaveChanges();
      Console.WriteLine("Numer of bookings saved: " + count1);
     }

     using (var ctx = new WWWingsContext(connection))
     {
      ctx.Database.UseTransaction(t);

      // Change free seats and save
      var f = ctx.FlightSet.Find(flightNo);
      Console.WriteLine("BEFORE: " + f.ToString());
      f.FreeSeats--;
      f.Memo = "last changed at " + DateTime.Now.ToString();
      Console.WriteLine("AFTER: " + f.ToString());
      var count2 = ctx.SaveChanges();
      Console.WriteLine("Number of saved changes: " + count2);

      Console.WriteLine("Commit or Rollback? 1 = Commit, other = Rollback");
      var eingabe = Console.ReadKey().Key;
      Console.WriteLine();
      if (eingabe == ConsoleKey.D1)
      { t.Commit(); Console.WriteLine("Commit done!"); }
      else
      { t.Rollback(); Console.WriteLine("Rollback done!"); }

      Console.WriteLine("After in RAM: " + f.ToString());
      ctx.Entry(f).Reload();
      Console.WriteLine("After in DB: " + f.ToString());
     }
    }
   }
  }

  /// <summary>
  /// New in v2.1 / not possible in EFC <= 2.0
  /// </summary>
  [EFCBook("5.0","2.1")]
  public static void TransactionScopeDemo()
  {
   CUI.Headline(nameof(TransactionScopeDemo));

   using (var t = new TransactionScope())
   {
    using (var ctx1 = new WWWingsContext())
    {
     int flightNo = ctx1.FlightSet.OrderBy(x => x.FlightNo).FirstOrDefault().FlightNo;
     var f = ctx1.FlightSet.Where(x => x.FlightNo == flightNo).SingleOrDefault();

     Console.WriteLine("Before: " + f.ToString());
     f.FreeSeats--;
     f.Memo = "Last changed at " + DateTime.Now.ToString();

     Console.WriteLine("After: " + f.ToString());

     var count1 = ctx1.SaveChanges();
     Console.WriteLine("Number of saved changes: " + count1);
    }

    using (var ctx2 = new WWWingsContext())
    {
     var f = ctx2.FlightSet.OrderBy(x => x.FlightNo).Skip(1).Take(1).SingleOrDefault();

     Console.WriteLine("Before: " + f.ToString());
     f.FreeSeats--;
     f.Memo = "Last changed at " + DateTime.Now.ToString();

     Console.WriteLine("After: " + f.ToString());

     var count1 = ctx2.SaveChanges();
     Console.WriteLine("Number of saved changes: " + count1);
    }

    // Commit Transaction!
    t.Complete();
    CUI.PrintSuccess("Completed!");
   }
  }
 }
}
