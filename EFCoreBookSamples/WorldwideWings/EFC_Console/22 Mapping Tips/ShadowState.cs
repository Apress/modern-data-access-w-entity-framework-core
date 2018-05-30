using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DA;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;

namespace EFC_Console
{
 static internal class ShadowState
 {
  [EFCBook()]
  public static void ReadAndChangeShadowProperty()
  {
   int flightNo = 101;
   CUI.MainHeadline(nameof(ReadAndChangeShadowProperty));

   using (WWWingsContext ctx = new WWWingsContext())
   {

    var flight = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == flightNo);

    CUI.Headline("List of all shadow property of type Flight");
    foreach (var p in ctx.Entry(flight).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": " + p.Metadata.IsShadowProperty);
    }

    CUI.Print("Before: " + flight.ToString() + " / " + ctx.Entry(flight).State, ConsoleColor.Cyan);
    Console.WriteLine("Free seats: " + ctx.Entry(flight).Property("FreeSeats").CurrentValue);
    Console.WriteLine("Last change: " + ctx.Entry(flight).Property("LastChange").CurrentValue);

    CUI.PrintWarning("Changing object...");
    flight.FreeSeats += 1;
    ctx.Entry(flight).Property("LastChange").CurrentValue = DateTime.Now;

    CUI.Print("After: " + flight.ToString() + " / " + ctx.Entry(flight).State, ConsoleColor.Cyan);
    Console.WriteLine("Free seats: " + ctx.Entry(flight).Property("FreeSeats").CurrentValue);
    Console.WriteLine("Last change: " + ctx.Entry(flight).Property("LastChange").CurrentValue);

    var count = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + count);
   }

   CUI.Headline("LINQ query using a Shadow Property");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var date = ctx.FlightSet
     .Where(c => EF.Property<DateTime>(c, WWWingsContext.ShadowPropertyName) > DateTime.Now.AddDays(-2))
     .OrderByDescending(c => EF.Property<DateTime>(c, WWWingsContext.ShadowPropertyName))
     .Select(x => EF.Property<DateTime>(x, WWWingsContext.ShadowPropertyName))
     .FirstOrDefault();

    Console.WriteLine("Last change: " + date);
   }

   CUI.Headline("Retest: Access shadow property column using SQL");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    string CONNSTRING = ctx.Database.GetDbConnection().ConnectionString;
    SqlConnection conn = new SqlConnection(CONNSTRING);
    conn.Open();
    string SQL = "Select LastChange from Flight where FlightNo = " + flightNo;
    SqlDataReader dr = new SqlCommand(SQL, conn).ExecuteReader();

    while (dr.Read())
    {
     Console.WriteLine(dr["LastChange"]);
    }
    dr.Close();
    conn.Close();
   }
  }

  [EFCBook]
  public static void ColumnsAddedAfterCompilation(bool logging = true)
  {
   CUI.MainHeadline(nameof(ColumnsAddedAfterCompilation));
   // List of additional columns can be read from a config file or the database schema
   List<string> additionalColumnSet = new List<string>() { "BO.Airline;Address;System.String", "BO.Airline;FoundingYear;System.Nullable`1[System.Int32]", "BO.Airline;Bunkrupt;System.Boolean?", "BO.AircraftType;Role;System.String" };
   // List of additional columns must be set before creating the first instance of the context!
   WWWingsContext.AdditionalColumnSet = additionalColumnSet;

   using (WWWingsContext ctx = new WWWingsContext())
   {
    if (logging) ctx.Log();
    // read any Airline object
    var a = ctx.AirlineSet.SingleOrDefault(x => x.Code == "WWW");
    if (a == null) throw new ApplicationException("No Airline found!");
    Console.WriteLine(a);
    Console.WriteLine("Extra columns:");
    foreach (var col in additionalColumnSet.Where(x=>x.StartsWith("BO.Airline")))
    {
     string columnname = col.Split(';')[1];
     Console.WriteLine(col + "=" + ctx.Entry(a).Property(columnname).CurrentValue);
    }
   }
  }
 }
}