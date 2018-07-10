using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DA;

namespace BL
{

 /// <summary>
 /// Repository class for Flight entities
 /// </summary>
 public class FlightManager  : IDisposable
 {
  public FlightManager() 
  {
   // create instance of context when FlightManager is created
   ctx = new WWWingsContext();
  }

  // keep one EFCore context per instance
  private WWWingsContext ctx;

  /// <summary>
  /// Dispose context if FlightManager is disposed
  /// </summary>
  public void Dispose() { ctx.Dispose(); }


  /// <summary>
  /// Get one flight
  /// </summary>

  public Flight GetFlight(int flightID)
  {
     return ctx.FlightSet.Find(flightID);
  }

  /// <summary>
  /// Get all flights on a route
  /// </summary>
  public List<Flight> GetFlightSet(string departure, string destination)
  {
   var query = GetAllAvailableFlightsInTheFuture();
   if (!String.IsNullOrEmpty(departure)) query = from f in query
                                                   where f.Departure == departure
                                                   select f;
   if (!String.IsNullOrEmpty(destination)) query = query.Where(f => f.Destination == destination);
   List<Flight> result = query.ToList();
   return result;
  }

  /// <summary>
  /// Base query that callre can extend
  /// </summary>
  public IQueryable<Flight> GetAllAvailableFlightsInTheFuture()
  {
   var now = DateTime.Now;
   var query = (from x in ctx.FlightSet
                where x.FreeSeats > 0 && x.Date > now
                select x);
   return query;
  }

  /// <summary>
  /// Get the combined list of all departures and all destinations
  /// </summary>
  /// <returns></returns>
  public List<string> GetAirports()
  {
   var l1 = ctx.FlightSet.Select(f => f.Departure).Distinct();
   var l2 = ctx.FlightSet.Select(f => f.Destination).Distinct();
   var l3 = l1.Union(l2).Distinct();
   return l3.OrderBy(z => z).ToList();
  }

  /// <summary>
  /// Delegate SaveChanges() to the context class
  /// </summary>
  /// <returns></returns>
  public int Save()
  {
   return ctx.SaveChanges();
  }

  /// <summary>
  /// This overload checks if there are objects in the list that do not belong to the context. These are inserted with Add().
  /// </summary>
  public int Save(List<Flight> flightSet)
  {
   foreach (Flight f in flightSet)
   {
    if (ctx.Entry(f).State == EntityState.Detached)
    {
     ctx.FlightSet.Add(f);
    }
   }
   return Save(); 
  }

  /// <summary>
  /// Remove flight (Delegated to context class)
  /// </summary>
  /// <param name="f"></param>
  public void RemoveFlight(Flight f)
  {
   ctx.Remove(f);
  }

  /// <summary>
  /// Add flight (Delegated to context class)
  /// </summary>
  /// <param name="f"></param>
  public void Add(Flight f)
  {
   ctx.Add(f);
  }
  
  /// <summary>
  ///   Reduces the number of free seats on the  flight, if seats are still available. Returns true if successful, false otherwise.
  /// </summary>
  /// <param name="flightID"></param>
  /// <param name="numberOfSeats"></param>
  /// <returns>true, wenn erfolgreich</returns>
  public bool ReducePlatzAnzahl(int flightID, short numberOfSeats)
  {
   var f = GetFlight(flightID);
   if (f != null)
   {
    if (f.FreeSeats >= numberOfSeats)
    {
     f.FreeSeats -= numberOfSeats;
     ctx.SaveChanges();
     return true;
    }
   }
   return false;
  }
 }
}