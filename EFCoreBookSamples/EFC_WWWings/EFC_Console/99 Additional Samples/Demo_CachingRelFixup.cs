using DA;
using ITVisions;
using System;
using System.Linq;


//**** NOTE: This sample is not in the English book. Therefore it has not been translated!

namespace EFC_Console
{
 class CachingRelFixup
 {
  [NotYetInTheBook]
  public static void Demo_FirstLevelCaching()
  {
   CUI.MainHeadline(nameof(Demo_FirstLevelCaching));

   using (var ctx = new WWWingsContext())
   {
    //ctx.Log();

    var alleFlight = ctx.FlightSet.ToList();
    Console.WriteLine("Loaded Flights: " + alleFlight.Count);

    // jetzt noch mal einen Flight anfordern
    var flight101 = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");

    // Objekt jetzt ändern
    // flight101.FreeSeats--;

    // jetzt alle Variablen zurücksetzen
    alleFlight = null;
    flight101 = null;

    // jetzt gleiche Anfragen nochmal

    alleFlight = ctx.FlightSet.ToList();
    Console.WriteLine("Geladene Flights: " + alleFlight.Count);
    flight101 = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == 101);
    Console.WriteLine($"Flight Nr {flight101.FlightNo} from {flight101.Departure} to {flight101.Destination} has {flight101.FreeSeats} free seats!");

    // Ergebnis: Auch wenn EF die DB noch mal fragen, ist das geänderte Objekt noch da
   }
  }

  [NotYetInTheBook]
  public static void Demo_RelFixup()
  {
   CUI.MainHeadline("First Level Cache");

   using (var ctx = new WWWingsContext())
   {
    var pilot = ctx.PilotSet.SingleOrDefault(x => x.PersonID == 5);
    Console.WriteLine("Pilot: " + pilot);
    pilot = null;
    // der Pilot sollte nun weg sein
    var flight = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == 101);
    // ist er aber nicht !!!
    Console.WriteLine(flight);
    Console.WriteLine("Pilot: " + flight.Pilot);
   }
  }
 }
}
