using DA;
using ITVisions;
using System;
using System.Linq;

namespace EFC_Console
{
 public class DatabaseViews
 {

  /// <summary>
  /// Read Data from View
  /// EFC 1.x/2.0
  /// </summary>
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

  /// Read Data from View
  /// EFC >= 2.1>
  [EFCBook]
  public static void DatabaseView()
  {
   CUI.MainHeadline(nameof(DatabaseView));

   using (var ctx = new WWWingsContext())
   {
    // Composition SQL on  VIEW :-)
    var query = ctx.DepartureStatisticsView.Where(x => x.FlightCount > 0).OrderBy(x => x.FlightCount);
    var liste = query.ToList();
    foreach (var stat in liste)
    {
     Console.WriteLine($"{stat.FlightCount:000} Flights departing from {stat.Departure}.");
    }
   }
   }
  }
}
