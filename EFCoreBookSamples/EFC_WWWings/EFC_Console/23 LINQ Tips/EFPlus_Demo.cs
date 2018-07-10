using DA;
using BO;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace EFC_Console
{

 /// <summary>
 /// Diese Demos laufen noch nicht mit EFC 2.0, weil es EFPlus dafür noch nicht gibt
 /// https://www.nuget.org/packages/Z.EntityFramework.Plus.EFCore/
 /// https://github.com/zzzprojects/EntityFramework-Plus
 /// </summary>
 class EFPlus
 {

  public static void Run()
  {
   EFPlus_FutureQuery();
   return;

   BulkOperations.BulkUpdateEFPlus();
   Run_Filter();
   Run_Caching();
   Run_Audit();
  }



  [EFCBook()]
  public static void EFPlus_FutureQuery()
  {
   CUI.MainHeadline(nameof(EFPlus_FutureQuery));
   using (var ctx = new DA.WWWingsContext())
   {
    ctx.Log();
    CUI.Headline("Define three future queries ... nothing happens in the database");
    QueryFutureEnumerable<Pilot> qAllPilots = ctx.PilotSet.Future();
    QueryFutureEnumerable<Flight> qFlightSetRome = ctx.FlightSet.Where(x => x.Departure == "Rome").Future();
    QueryFutureEnumerable<Flight> qFlightSetBerlin = ctx.FlightSet.Where(x => x.Departure == "Berlin").Future();
    CUI.Headline("Access the pilots:");
    var allePilots = qAllPilots.ToList();
    Console.WriteLine(allePilots.Count + " Pilots are loaded!");
    CUI.Headline("Access the flights from Rome:");
    var flightSetRom = qFlightSetRome.ToList();
    Console.WriteLine(flightSetRom.Count + " flights from Berlin are loaded!");
    CUI.Headline("Define another two future queries ... nothing happens in the database");
    QueryFutureEnumerable<Flight> qFugSetLondon = ctx.FlightSet.Where(x => x.Departure == "London").Future();
    QueryFutureEnumerable<Flight> qflightSetParis = ctx.FlightSet.Where(x => x.Departure == "Paris").Future();
    CUI.Headline("Access the flights from Berlin:");
    var flightSetBerlin = qFlightSetBerlin.ToList();
    Console.WriteLine(flightSetBerlin.Count + " flights from Rome are loaded!");
    CUI.Headline("Access the flights from London:");
    var flightSetLondon = qFugSetLondon.ToList();
    Console.WriteLine(flightSetLondon.Count + " flights from London are loaded!");
    CUI.Headline("Access the flights from Paris:");
    var flightSetParis = qflightSetParis.ToList();
    Console.WriteLine(flightSetParis.Count + " flights from Paris are loaded!");
   }
  }


  [EFCBook("5.3")]
  public static void EFPlus_FutureQuery_SingleObjectsAndValues()
  {
   CUI.MainHeadline(nameof(EFPlus_FutureQuery_SingleObjectsAndValues));
   using (var ctx = new DA.WWWingsContext())
   {
    ctx.Log();
    CUI.Headline("Define three future queries ... nothing happens in the database");
    QueryFutureValue<int> qPilotCount = ctx.PilotSet.DeferredCount().FutureValue();
    QueryFutureValue<DateTime?> qBirthdayOfOldestPilot = ctx.PilotSet.DeferredMin(x => x.Birthday).FutureValue();
    QueryFutureValue<Pilot> qOldestPilot = ctx.PilotSet.OrderBy(x => x.Birthday).DeferredFirstOrDefault().FutureValue();

    CUI.Headline("We need the values now!");
    Console.WriteLine("Number of pilots: " + qPilotCount.Value);
    Console.WriteLine("Birthday of oldest pilot: " + qBirthdayOfOldestPilot.Value.Value.ToShortDateString());
    Console.WriteLine("Name of of oldest pilot: " + qOldestPilot.Value.FullName);

   }
  }

  public static void Run_Audit()
  {

   using (var ctx = new DA.WWWingsContext())
   {
    ctx.Log();

    var flightSet = ctx.FlightSet.Where(x => x.Departure == "Berlin").ToList();
    foreach (var f in flightSet)
    {
     f.FreeSeats--;
     f.Memo = "Last change: " + DateTime.Now;
    }

    var audit = new Audit();
    ctx.SaveChanges(audit);

    // Changes stehen nun im audit-Objekt bereit
    foreach (var a in audit.Entries)
    {
     Console.WriteLine("---------------" + a.Entity.ToString());
     foreach (var p in a.Properties)
     {
      Console.WriteLine(p.PropertyName + ": " + p.OldValue + "->" + p.NewValue);
     }
    }

   }
  }

  public static void Run_Filter()
  {

   // CREATE global filter
   QueryFilterManager.Filter<Flight>(x => x.Where(c => c.Departure == "Rome"));

   var ctx = new DA.WWWingsContext();
   ctx.Log();


   // TIP: Add this line in EntitiesContext constructor instead
   QueryFilterManager.InitilizeGlobalFilter(ctx);

   // SELECT * FROM Customer WHERE IsActive = true
   var customer = ctx.FlightSet.ToList();

   foreach (var f in customer)
   {
    Console.WriteLine(f);
   }

  }

  public static void Run_Caching()
  {
   var ctx = new DA.WWWingsContext();
   ctx.Log();
   for (int i = 0; i < 10; i++)
   {
    //QueryCacheManager.
    // The query is cached using default QueryCacheManager options
    var flightSet1 = ctx.FlightSet.Where(x => x.Departure == "Rome").FromCache();
    Console.WriteLine(flightSet1.Count());
    // (EF7) The query is cached for 2 hours without any activity
    var options = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(2) };
    var flightSet2 = ctx.FlightSet.Where(x => x.Departure == "Rome").FromCache();
    Console.WriteLine(flightSet2.Count());
   }
  }
 }
}
