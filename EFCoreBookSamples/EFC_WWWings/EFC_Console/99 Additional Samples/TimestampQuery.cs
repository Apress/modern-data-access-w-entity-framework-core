using DA;
using ITVisions;
using ITVisions.EFCore;
using System;
using System.Linq;

//**** NOTE: This sample is not in the English book. Therefore it has not been translated!

namespace EFC_Console
{
 public static class ByteArrayExtensions
 {
  public static int Compare(this byte[] b1, byte[] b2)
  {
   return 1;
   //throw new NotImplementedException();
  }
 }

 class QueryDemosBesonderheiten
 {
  public static byte[] ts;
  public static void Print()
  {
   using (var ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.Where(f => f.Departure == "Paris").Take(3);
    foreach (var f in flightSet)
    {
     ts = f.Timestamp;
     Console.WriteLine(f.FlightNo + ": " + f.FreeSeats + "/" + f.Timestamp.ByteArrayToString());
    }
   }
  }

  public static void TimestampQuery()
  {

   CUI.Headline("Vor Änderung");
   Print();

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    var flightSet = ctx.FlightSet.Where(f => f.Departure == "Paris").OrderBy(x=>x.Timestamp).Take(1);
    foreach (var f in flightSet)
    {
     f.FreeSeats--;
    }
    ctx.SaveChanges();
    CUI.Headline("Nach Änderung");
    Print();

    CUI.Headline("Abfrage: TS > " + ts.ByteArrayToString());
    var geänderteflightSet = ctx.FlightSet.Where(x => x.Timestamp.Compare(ts) > 0).ToList();


    foreach (var f in flightSet)
    {
     Console.WriteLine(f.FlightNo + ": " + f.FreeSeats + "/" + f.Timestamp.ByteArrayToString());

    }
   }
  }
 }
}
