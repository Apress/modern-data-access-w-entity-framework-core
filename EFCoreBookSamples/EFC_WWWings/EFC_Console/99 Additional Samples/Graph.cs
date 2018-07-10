using System;
using System.Linq;
using BO;
using DA;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

//**** NOTE: This sample is not in the English book. Therefore it has not been translated!

namespace EFC_Console
{
 static internal class Graph
 {

  [NotYetInTheBook]
  public static void TrackGraph()
  {
   CUI.MainHeadline(nameof(TrackGraph));
   Flight f;

   using (WWWingsContext ctx1 = new WWWingsContext())
   {
    int flightNo = 150;
    f = ctx1.FlightSet.Include(x => x.Pilot).Where(x => x.FlightNo == flightNo).SingleOrDefault();
    Console.WriteLine(f);
    Console.WriteLine(f.Pilot);
   }

   using (WWWingsContext ctx2 = new WWWingsContext())
   {
    ctx2.ChangeTracker.TrackGraph(f, TrackGraph_Callback);
    var anz = ctx2.SaveChanges();
    Console.WriteLine(anz + " Changes saved!");

   }

  }

  [NotYetInTheBook]
  public static void TrackGraph_Callback(EntityEntryGraphNode e)
  {
   Console.WriteLine("TrackGraph_Callback: " + e.Entry.Entity + ": " + e.NodeState + ", " + e.Entry.State);
   e.Entry.State = EntityState.Modified;
   Console.WriteLine("TrackGraph_Callback: " + e.Entry.Entity + ": " + e.NodeState + ", " + e.Entry.State);
  }


  [NotYetInTheBook]
  public static void Demo_AddGraph()
  {
   CUI.Headline("Demo_AddGraph");

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    ctx.Database.ExecuteSqlCommand("Delete from Booking where FlightNo = 101");
    ctx.Database.ExecuteSqlCommand("Delete from Flight where FlightNo = 101");

    var pilot = new Pilot();
    pilot.Surname = "testpilot";
    //ctx.PilotSet.Add(pilot);

    var fneu = new Flight();
    fneu.FlightNo = 101;
    fneu.Departure = "Essen";
    fneu.Destination = "Darmstadt";
    fneu.Pilot = pilot;
    fneu.Seats = 100;
    fneu.Copilot = pilot;
    fneu.FreeSeats = 100;

    ctx.ChangeTracker.TrackGraph(fneu, (obj) => obj.NodeState = obj.Entry.State = EntityState.Added);
    //ctx.ChangeTracker.TrackGraph(fneu, TrackGraph_CallbackAdded);
    ctx.FlightSet.Add(fneu);
    EFC_Util.PrintChangeInfo(ctx);

    var anz = ctx.SaveChanges();

    Console.WriteLine("Saved changes: " + anz);
   }
  }
 }
}