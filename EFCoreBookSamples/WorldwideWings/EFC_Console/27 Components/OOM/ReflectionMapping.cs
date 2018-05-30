using System;
using System.Linq;
using DA;

namespace EFC_Console.OOM
{
 public class FlightDTO
 {
  public int FlightNo { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
  public DateTime Date { get; set; }
 }

 public static class ReflectionMapping
 {

  public static void Run()
  {
   using (var ctx = new WWWingsContext())
   {
    var flightSet = ctx.FlightSet.Where(x => x.Departure == "Berlin").ToList();
    foreach (var flight in flightSet)
    {
     var dto = flight.CopyTo<FlightDTO>();
     Console.WriteLine(dto.FlightNo + ": " + dto.Departure +"->" + dto.Destination + ": " + dto.Date.ToShortDateString());
    }
   }
  }
 }
}