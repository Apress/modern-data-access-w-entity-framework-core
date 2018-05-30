using System.Linq;
using ITV;
using DA;
using Microsoft.EntityFrameworkCore;

//**** NOTE: This sample is not in the English book. Therefore it has not been translated!

namespace EFC_Console
{
 public class PerformanceMeasurement
 {

  public int flightNo = 130;

  public void Run()
  {
   for (int i = 0; i < 50; i++)
   {
    flightNo++;
    Timer.Run("EagerLoading", Run_Demo_EagerLoading);
    Timer.Run("Preloading", Run_Demo_PreLoading);

   }
   ITV.Timer.Results();
  }

 

  private long Run_Demo_EagerLoading()
  {

  
   using (var ctx = new WWWingsContext())
   {
    // Lade den Flight und einige verbunde Objekte via Eager Loading
    var f = ctx.FlightSet
     .Include(b => b.BookingSet).ThenInclude(p => p.Passenger)
     .Include(b => b.Pilot).ThenInclude(p => p.FlightAsPilotSet)
     .Include(b => b.Copilot).ThenInclude(p => p.FlightAsCopilotSet)
     .SingleOrDefault(x => x.FlightNo == flightNo);
   }
   return 0;
  }

  private long Run_Demo_PreLoading()
  {
   using (var ctx = new WWWingsContext())
   {

    // 1. Load only the flight
    var f = ctx.FlightSet
     .SingleOrDefault(x => x.FlightNo == flightNo);

    // 2. Download both pilots
    ctx.PilotSet.Where(
     p => p.FlightAsPilotSet.Any(x => x.FlightNo == flightNo) || p.FlightAsCopilotSet.Any(x => x.FlightNo == flightNo)).ToList();

    // 3. Laden Sie die Flüge anderer Piloten herunter
    ctx.FlightSet.Where(x => x.PilotId == f.PilotId || x.CopilotId == f.CopilotId).ToList();

    // 4. Load BookingSet
    ctx.BookingSet.Where(x => x.FlightNo == flightNo).ToList();

    // 5. Load Passagers
    ctx.PassengerSet.Where(p => p.BookingSet.Any(x => x.FlightNo == flightNo)).ToList();
   }
   return 0;
  }


 }
}