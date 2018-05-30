using System;
using System.Collections.Generic;
using System.Linq;
using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ITVisions.EFCore;
using System.IO;

namespace EFC_Console
{
 public class Logging
 {
  public static void Log(string s)
  {
   var sw = System.IO.File.AppendText(@"c:\temp\EFC3.log");
   sw.WriteLine(s);
   sw.Close();
   CUI.Print(s, ConsoleColor.Cyan);
  }
  public static void LoggingWithLogExtensionMethod()
  {
   CUI.MainHeadline(nameof(LoggingWithLogExtensionMethod));
   
   // ToString() geht nicht :-(
   using (var ctx = new WWWingsContext())
   {
    var query = ctx.FlightSet.Where(x => x.FlightNo > 100).OrderBy(x => x.Date).Skip(10).Take(5);
    Console.WriteLine(query.ToString()); // nur Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[GO.Flight]
   }

   CUI.Headline("Log to Console.WriteLine - only updates");
   using (var ctx1 = new WWWingsContext())
   {
    Console.WriteLine("Get some flights...");
    var query1 = ctx1.FlightSet.Where(x => x.FlightNo > 100).OrderBy(x => x.Date).Skip(10).Take(5);
    ctx1.Log(Console.WriteLine, new List<string>() { "Microsoft.EntityFrameworkCore.Database.Command" }, new List<int>() { 20100, 20101 });
    //ctx1.Log(Console.WriteLine, new List<string>(), new List<int>());
    var flightSet1 = query1.ToList();
    flightSet1.ElementAt(0).FreeSeats--;
    Console.WriteLine("Save changes...");
    ctx1.SaveChanges();
   }


   CUI.Headline("No Logging");
   using (var ctx1 = new WWWingsContext())
   {
    Console.WriteLine("Get some flights...");
    var query1 = ctx1.FlightSet.Where(x => x.FlightNo > 100).OrderBy(x => x.Date).Skip(10).Take(5);
   var flightSet1 = query1.ToList();
    flightSet1.ElementAt(0).FreeSeats--;
    Console.WriteLine("Save changes...");
    ctx1.SaveChanges();
   }

   CUI.Headline("Log to Console.WriteLine");
   using (var ctx1 = new WWWingsContext())
   {
    Console.WriteLine("Get some flights...");
    var query1 = ctx1.FlightSet.Where(x => x.FlightNo > 100).OrderBy(x => x.Date).Skip(10).Take(5);
    ctx1.Log(Console.WriteLine);
    var flightSet1 = query1.ToList();
    flightSet1.ElementAt(0).FreeSeats--;
    Console.WriteLine("Save changes...");
    ctx1.SaveChanges();
   }

   CUI.Headline("Log to Console.WriteLine again");
   using (var ctx1 = new WWWingsContext())
   {
    Console.WriteLine("Get some flights...");
    var query1 = ctx1.FlightSet.Where(x => x.FlightNo > 100).OrderBy(x => x.Date).Skip(10).Take(5);
    ctx1.Log(Console.WriteLine);
    var flightSet1 = query1.ToList();
    flightSet1.ElementAt(0).FreeSeats--;
    Console.WriteLine("Save changes...");
    ctx1.SaveChanges();
   }

   CUI.Headline("Log to Default");
   using (var ctx2 = new WWWingsContext())
   {
    Console.WriteLine("Get some flights...");
    var query2 = ctx2.FlightSet.Where(x => x.FlightNo < 3000).OrderBy(x => x.Date).Skip(10).Take(5);
    ctx2.Log();
    var flightSet2 = query2.ToList();
    flightSet2.ElementAt(0).FreeSeats--;
    ctx2.SaveChanges();
   }

   CUI.Headline("Log to file");
   using (var ctx3 = new WWWingsContext())
   {
    Console.WriteLine("Get some flights...");
    var query3 = ctx3.FlightSet.Where(x => x.FlightNo > 100).OrderBy(x => x.Date).Skip(10).Take(5);
    ctx3.Log(LogToFile);
    var flightSet3 = query3.ToList();
    flightSet3.ElementAt(0).FreeSeats--;
    Console.WriteLine("Save changes...");
    ctx3.SaveChanges();
   }




  }

  public static void LogToFile(string s)
  {
   Console.WriteLine(s);
   var sw = new StreamWriter(@"c:\temp\log.txt");
   sw.WriteLine(DateTime.Now + ": " + s);
   sw.Close();
  }


  /// <summary>
  /// Logging witouth the extension method Log()
  /// </summary>
  public static void LoggingWithoutLogMethod()
  {
   CUI.MainHeadline(nameof(LoggingWithoutLogMethod));

   using (var ctx = new WWWingsContext())
   {
    // Create service provider 
    var serviceProvider = ctx.GetInfrastructure<IServiceProvider>();
    // Add logger factory 
    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
    loggerFactory.AddProvider(new FlexLoggerProvider(Log));

    var flight101 = ctx.FlightSet.Include(b => b.Pilot).SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine(flight101);
    flight101.FreeSeats--;
    ctx.SaveChanges();
   }

  }

 }
}
