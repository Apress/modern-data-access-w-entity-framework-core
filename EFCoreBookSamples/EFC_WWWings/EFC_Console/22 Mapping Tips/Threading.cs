using System;
using System.Threading.Tasks;
using DA;
using System.Linq;

namespace EFC_Console
{
 class Threading
 {
  [NotYetInTheBook]
  public static void EF_MT()
  {

   using (WWWingsContext ctx = new WWWingsContext())
   {
    // Lade viele Flights
    var liste = ctx.FlightSet.Take(1000);
    try
    {
     // Versuch, Changes parallel zu speichern
     Parallel.ForEach(liste, f =>
     {

      Console.WriteLine("Before: " + f.ToString());
      f.FreeSeats += 1;
      Console.WriteLine("After: " + f.ToString());
      var anz = ctx.SaveChanges();
      Console.WriteLine("Anzahl gespeicherter Ändeurngen: " + anz);
     });
    }
    catch (Exception ex)
    {
     Console.WriteLine("Multi-Threading geht nicht: " + ex.ToString());
    }

   }
  }
 }
}
