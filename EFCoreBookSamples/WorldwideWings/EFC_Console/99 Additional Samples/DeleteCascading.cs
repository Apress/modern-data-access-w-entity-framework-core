using System;
using System.Linq;
using DA;
using BO;
using ITVisions.EFCore;
using ITVisions;
//**** NOTE: This sample is not in the English book. Therefore it has not been translated!
namespace EFC_Console
{
 class DeleteCascading
 {
  [NotYetInTheBook]
  public static void EinfachesUpdate()
  {

   // Set up demo: Create 1 flight type and 3 flights
   using (WWWingsContext ctx = new WWWingsContext())
   {

    if (!ctx.AircraftTypeSet.Any())
    {
     var ft = new AircraftType();
     ft.TypeID = 1;
     ft.Manufacturer = "Airbus";
     ft.Name = "A320";
     ctx.AircraftTypeSet.Add(ft);
     var anz1 = ctx.SaveChanges();
     Console.WriteLine("New aircrafttypes: " + anz1);
    }
    var ft2 = ctx.AircraftTypeSet.FirstOrDefault();
    ctx.Log();

    if (!ctx.FlightSet.Any(f => f.Departure == "Amsterdam"))
    {
     var p = new Pilot();
     p.GivenName = "Holger";
     p.Surname = "Schwichtenberg";
     p.HireDate = DateTime.Now;
     p.LicenseDate = DateTime.Now;
     var pd = new Persondetail();
     //pd.Planet = "Erde";
     p.Detail = pd;

     var maxflightNo = ctx.FlightSet.Max(x => x.FlightNo);
     for (int i = 0; i < 3; i++)
     {
      var f = new Flight();
      f.Pilot = p;
      f.FlightNo = maxflightNo + i;
      f.Departure = "Amsterdam";
      f.Destination = "Bucharest";
      f.Seats = 100;
      f.FreeSeats = 100;
      f.Date = DateTime.Now.AddDays(i);
      f.AircraftType = ft2;
      ctx.FlightSet.Add(f);
      var anz2 = ctx.SaveChanges();
      Console.WriteLine("New flights: " + anz2);
     }
    }

    foreach (var f in ctx.FlightSet.Where(f => f.Departure == "Amsterdam").ToList())
    {
     f.AircraftType = ft2;
     var anz3 = ctx.SaveChanges();
     Console.WriteLine("Neu aircraft type assigment: " + anz3);
    }

   }


   CUI.Headline("Delete with dependent objects of the N-side in RAM");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    Console.WriteLine("Total number of flights: " + ctx.FlightSet.Count());

    var ft = ctx.AircraftTypeSet.FirstOrDefault();
    //var ft = ctx.AircraftTypeSet.Include(x=>x.FlightSet).FirstOrDefault();
    ctx.FlightSet.Where(f => f.AircraftTypeID == ft.TypeID).ToList();
    ctx.ChangeTracker.DetectChanges();
    Console.WriteLine("Flights with this aircraft type: " + ft.FlightSet.Count);
    CUI.Print("Remove aircraft type now...", ConsoleColor.Red);
    ctx.AircraftTypeSet.Remove(ft);
    var anz1 = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + anz1);
    Console.WriteLine("Total number of flights: " + ctx.FlightSet.Count());
   }
  }


 }
}
