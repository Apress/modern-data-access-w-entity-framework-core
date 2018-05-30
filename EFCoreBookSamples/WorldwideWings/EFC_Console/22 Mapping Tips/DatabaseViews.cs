using DA;
using ITVisions;
using System;
using System.Linq;

namespace EFC_Console
{
 public class DatabaseViews
 {
  [EFCBook]
  public static void DatabaseViewWithPseudoEntity()
  {
   CUI.MainHeadline(nameof(DatabaseViewWithPseudoEntity));

   using (var ctx = new WWWingsContext())
   {
    var query = ctx.DepartureStatisticsSet.Where(x => x.FlightCount > 0);
    var liste = query.ToList();
    foreach (var stat in liste)
    {
     Console.WriteLine($"{stat.FlightCount:000} Flights departing from {stat.Departure}.");
    }
   }
  }
 }
}
