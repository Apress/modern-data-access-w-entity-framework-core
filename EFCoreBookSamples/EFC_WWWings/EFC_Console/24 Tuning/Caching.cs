using BO;
using DA;
using ITVisions;
using ITVisions.Caching;
using ITVisions.EFCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Z.EntityFramework.Plus;

// ReSharper disable SuggestUseVarKeywordEvident

namespace EFC_Console
{

 internal class Caching
 {
  /// <summary>
  /// GetFlight using System.Runtime.Caching.MemoryCache (5 seconds)
  /// </summary>
  [EFCBook()]
  public static void Demo_MemoryCache()
  {
   CUI.MainHeadline(nameof(Demo_MemoryCache));
   DateTime Start = DateTime.Now;
   do
   {
    var flightSet = GetFlight1("Rome");
    // you can process the flights here...
    Console.WriteLine("Processing " + flightSet.Count + " flights...");
    System.Threading.Thread.Sleep(500);
   } while ((DateTime.Now - Start).TotalSeconds < 30);

   CUI.Print("done!");
  }


  /// <summary>
  /// GetFlight with MemoryCache (5 sek)
  /// </summary>
  private static List<Flight> GetFlight1(string departure)
  {
   string cacheItemName = "FlightSet_" + departure;

   // Zugriff auf Cache-Eintrag
   System.Runtime.Caching.MemoryCache cache = System.Runtime.Caching.MemoryCache.Default;
   List<Flight> flightSet = cache[cacheItemName] as List<Flight>;
   if (flightSet == null) // Element ist NICHT im Cache
   {
    CUI.Print($"{DateTime.Now.ToLongTimeString()}: Cache missed", ConsoleColor.Red);
    using (var ctx = new WWWingsContext())
    {
     ctx.Log();
     // Load flights 
     flightSet = ctx.FlightSet.Where(x => x.Departure == departure).ToList();
    }
    // Store flights in cache 
    CacheItemPolicy policy = new CacheItemPolicy();
    policy.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
    //or: policy.SlidingExpiration = new TimeSpan(0,0,0,5);
    cache.Set(cacheItemName, flightSet, policy);
   }
   else // Data is already in cache
   {
    CUI.Print($"{DateTime.Now.ToLongTimeString()}: Cache hit", ConsoleColor.Green);
   }
   return flightSet;
  }

  // ############################################ 

  [EFCBook()]
  public static void Demo_CacheManager()
  {
   CUI.MainHeadline(nameof(Demo_CacheManager));
   DateTime Start = DateTime.Now;
   do
   {
    var flightSet = GetFlight2("Rome");
    // you can process the flights here...
    Console.WriteLine("Processing " + flightSet.Count + " flights...");
    System.Threading.Thread.Sleep(500);
   } while ((DateTime.Now - Start).TotalSeconds < 60);

   // Alternative demos:
   //GetFlight2("Rome");
   //GetFlight2("Rome");
   //GetFlight2("Rome");
   //GetFlight2("Paris");
   //GetFlight2("");
   //GetFlight2("");
   //GetFlight2("Rome");
   //GetFlight2("Paris");
  }

  /// <summary>
  /// GetFlight with CacheManager (5 sek)
  /// </summary>
  private static List<Flight> GetFlight2(string departure)
  {
   string cacheItemName = "FlightSet_" + departure;
   var cm = new CacheManager<List<Flight>>(5);
   cm.CacheHitEvent += (text) => { CUI.Print($"{DateTime.Now.ToLongTimeString()}: Cache hit: " + text, ConsoleColor.Green); };
   cm.CacheMissEvent += (text) => { CUI.Print($"{DateTime.Now.ToLongTimeString()}: Cache missed: " + text, ConsoleColor.Red); };
   return cm.Get(cacheItemName, GetFlight2Internal, departure);
  }

  private static List<Flight> GetFlight2Internal(object[] param)
  {
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    string departure = param[0] as string;
    // Load flights
    return ctx.FlightSet.Where(x => x.Departure == departure).ToList();
   }
  }

  // ############################################ 
  [EFCBook]
  public static void Demo_CacheManagerLambda()
  {
  CUI.MainHeadline(nameof(Demo_CacheManagerLambda));
   DateTime Start = DateTime.Now;
   do
   {
    var flightSet = GetFlight3("Rome");
    // you can process the flights here...
    Console.WriteLine("Processing " + flightSet.Count + " flights...");
    System.Threading.Thread.Sleep(500);

   } while ((DateTime.Now - Start).TotalSeconds < 60);
   // Alternative demos:
   //GetFlight2("Rome");
   //GetFlight2("Rome");
   //GetFlight2("Rome");
   //GetFlight2("Paris");
   //GetFlight2("");
   //GetFlight2("");
   //GetFlight2("Rome");
   //GetFlight2("Paris");
  }

  public static List<Flight> GetFlight3(string departure)
  {
   string cacheItemName = "FlightSet_" + departure;
   Func<string[], List<Flight>> getData = (a) =>
   {
    using (var ctx = new WWWingsContext())
    {
     // Load flights
     return ctx.FlightSet.Where(x => x.Departure == departure).ToList();
    }
   };

   var cm = new CacheManager<List<Flight>>(5);
   cm.CacheHitEvent += (text) => { CUI.Print($"{DateTime.Now.ToLongTimeString()}: Cache Hit: " + text, ConsoleColor.Green); };
   cm.CacheMissEvent += (text) => { CUI.Print($"{DateTime.Now.ToLongTimeString()}: Cache Miss: " + text, ConsoleColor.Red); };

   return cm.Get(cacheItemName, getData);
  }

  // ############################################ 

[NotYetInTheBook]
  public static void GetFlight_StandardCacheNurLesen(string departure = "Rome")
  {
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    CUI.Headline($"Alle Flights from {departure} laden!");
    // Alle Flights laden
    var flightSet = ctx.FlightSet.Where(x => x.Departure == departure && x.FlightNo < 300).ToList();

    var flightNo = 190; // FlightSet 190 ist ein FlightSet from Rom, der schon geladen wurde // flightSet.ElementAt
    CUI.Headline("Ein FlightSet laden mit Find() - allein aus Cache!");
    var flight1 = ctx.FlightSet.Find(flightNo);
    CUI.Print(flight1?.ToShortString(), ConsoleColor.White);

    CUI.Headline("Ein FlightSet laden mit SingleOrDefault() -> Query!");
    var flight2 = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == flightNo);
    CUI.Print(flight2?.ToShortString(), ConsoleColor.White);

    CUI.Headline("Ein FlightSet laden mit FirstOrDefault() -> Query!");
    var flight3 = ctx.FlightSet.FirstOrDefault(x => x.FlightNo == flightNo);
    CUI.Print(flight3?.ToShortString(), ConsoleColor.White);

    CUI.Headline("Ein FlightSet laden mit Where/SingleOrDefault() -> Query!");
    var flight4 = ctx.FlightSet.Where(x => x.FlightNo == flightNo).SingleOrDefault();
    CUI.Print(flight4.ToShortString(), ConsoleColor.White);

    CUI.Headline("Cacheinhalt");
    //ctx.FlightSet.Local.Clear();
    Console.WriteLine("Flights im Cache: " + ctx.FlightSet.Local.Count);
    foreach (var f in ctx.FlightSet.Local) // Local erlaubt Zugriff auf den Cacheinhalt!
    {
     Console.WriteLine(f.ToShortString());
    }
   }
  }

  // ##############################################

  /// <summary>
  /// uses EFPlus.Core
  /// <summary>
  [EFCBook()]
  public static void Demo_SecondLevelCache()
  {
   CUI.MainHeadline(nameof(Demo_SecondLevelCache));
   DateTime Start = DateTime.Now;
   do
   {
    var flightSet = GetFlight4("Rome");
    // you can process the flights here...
    Console.WriteLine("Processing " + flightSet.Count + " flights...");
    System.Threading.Thread.Sleep(500);
   } while ((DateTime.Now - Start).TotalSeconds < 30);

   return;
   GetFlight4("Rome");
   GetFlight4("Rome");
   GetFlight4("Rome");
   GetFlight4("Paris");
   GetFlight4("Mailand");
   GetFlight4("Mailand");
   GetFlight4("Rome");
   GetFlight4("Paris");
  }

  /// <summary>
  /// Caching with EFPlus FromCache() / 5 seconds
  /// </summary>
  /// <param name="departure"></param>
  /// <returns></returns>
  public static List<Flight> GetFlight4(string departure)
  {
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    var options = new MemoryCacheEntryOptions() { AbsoluteExpiration = DateTime.Now.AddSeconds(5) };
    // optional: QueryCacheManager.DefaultMemoryCacheEntryOptions = options;
    Console.WriteLine("Lade Flights from " + departure + "...");
    var flightSet = ctx.FlightSet.Where(x => x.Departure == departure).FromCache(options).ToList();
    Console.WriteLine(flightSet.Count + " Flights im RAM!");
    return flightSet;
   }
  }

  // ##############################################
 }
}
