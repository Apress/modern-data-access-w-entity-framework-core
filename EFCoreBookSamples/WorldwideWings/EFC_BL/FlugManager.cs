using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DA;

namespace BL
{
 //public class DepartureStatistics
 //{
 // public string City { get; set; }
 // public int Anzahl { get; set; }
 //}

 /// <summary>
 /// Datenmanager für Flight-Entitäten
 /// </summary>
 public class FlugManager 
 {
  public FlugManager() 
  {

  }

  WWWingsContext ctx = new WWWingsContext();
  
  public Flight GetFlug(int FlugID)
  {
     return ctx.FlightSet.Find(FlugID);
  }

  public List<Flight> GetFlightSet(string departure, string destination)
  {
   // Nutzung der Query-Hilfsmethode aus der Basisklasse
   var query = from f in ctx.FlightSet select f;

   // Eigentliche Logik für das Zusammensetzen der Abfrage
   if (!String.IsNullOrEmpty(departure)) query = from f in query
                                                   where f.Departure == departure
                                                   select f;

   if (!String.IsNullOrEmpty(destination)) query = query.Where(f => f.Destination == destination);

   // Ausführen der Abfrage
   List<Flight> result = query.ToList();

   return result;
  }

  public List<string> GetFlughaefen()
  {
   var l1 = ctx.FlightSet.Select(f => f.Departure).Distinct();
   var l2 = ctx.FlightSet.Select(f => f.Destination).Distinct();
   var l3 = l1.Union(l2).Distinct();
   return l3.OrderBy(z => z).ToList();
  }


  /// <summary>
  /// Implementierung einer Kapselung from SaveChanges() direkt in einer konkreten Datenzugriffsmanagerklasse
  /// Rückgabewert ist die Summe der Anzahl der gespeicherten neuen, geänderten und gelöschten Datensätze
  /// </summary>
  /// <returns></returns>
  public int Save()
  {
   return ctx.SaveChanges();
  }

  /// <summary>
  /// In dieser Überladung wird geprüft, ob es in der Liste Objekte gibt, die nicht dem Kontext angehören. Diese werden mit Add() ergänzt.
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
   return Save(); // ruft ctx.SaveChanges();
  }

  public void RemoveFlug(Flight f)
  {
   ctx.Remove(f);
  }

  public void Add(Flight f)
  {
   ctx.Add(f);
  }


  /// <summary>
  /// Reduziert die Anzahl der free Seatsn auf dem genannten Flight, sofern die Plätze noch verfügbar sind. Liefert true, wenn  erfolgreich, sonst false.
  /// </summary>
  /// <param name="FlugID"></param>
  /// <param name="PlatzAnzahl"></param>
  /// <returns>true, wenn erfolgreich</returns>
  public bool ReducePlatzAnzahl(int FlugID, short PlatzAnzahl)
  {
   var einzelnerFlug = GetFlug(FlugID);
   if (einzelnerFlug != null)
   {
    if (einzelnerFlug.FreeSeats >= PlatzAnzahl)
    {
     einzelnerFlug.FreeSeats -= PlatzAnzahl;
     ctx.SaveChanges();
     return true;
    }
   }
   return false;

  }



 }
}
