using System;
using System.Linq;
using DA;
using ITVisions;

namespace EFC_Console
{
 static internal class Timestamps
 {
  /// <summary>
  /// EFC reads the new timestamp after any update
  /// </summary>
  [NotYetInTheBook]
  public static void ShowUpdatedTimeStamp()
  {
   CUI.MainHeadline(nameof(ShowUpdatedTimeStamp));
   using (WWWingsContext ctx = new WWWingsContext())
   {
    var f = ctx.FlightSet.Take(1).SingleOrDefault();

    Console.WriteLine("Before: " + f.ToString() + " Timestamp: " + f.Timestamp.ByteArrayToString());
    f.FreeSeats--; // Change #1
    Console.WriteLine("After change: " + f.ToString() + " Timestamp: " + f.Timestamp.ByteArrayToString());
    var anz1 = ctx.SaveChanges();
    Console.WriteLine("After saving: " + f.ToString() + " Timestamp: " + f.Timestamp.ByteArrayToString());
   CUI.PrintSuccess("Number of saved changes: " + anz1);

    Console.WriteLine("Before: " + f.ToString() + " Timestamp: " + f.Timestamp.ByteArrayToString());
    f.FreeSeats--; // Change #2
    Console.WriteLine("After change: " + f.ToString() + " Timestamp: " + f.Timestamp.ByteArrayToString());
    var anz2 = ctx.SaveChanges();
    Console.WriteLine("After saving: " + f.ToString() + " Timestamp: " + f.Timestamp.ByteArrayToString());
    CUI.PrintSuccess("Number of saved changes: " + anz2);
   }
  }
 }
}