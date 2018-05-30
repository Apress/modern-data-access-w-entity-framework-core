using DA;
using BO;
using ITVisions;
using ITVisions.EFCore;
using System;

namespace EFC_Console
{
 class ForeignKeyAssociations
 {


  /// <summary>
  /// Assignment via an object reference
  /// </summary>
  [EFCBook]
  public static void ChangePilotUsingObjectAssignment()
  {
   CUI.MainHeadline(nameof(ChangePilotUsingObjectAssignment));
   var flightNo = 102;
   var newPilotID = 54;

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    Flight flight = ctx.FlightSet.Find(flightNo);
    Pilot newPilot = ctx.PilotSet.Find(newPilotID);
    flight.Pilot = newPilot;
    var count = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + count);
   }
  }

  /// <summary>
  /// Assignment via a foreign key property
  /// </summary>
  [EFCBook]
  public static void ChangePilotUsingFK()
  {
   CUI.MainHeadline(nameof(ChangePilotUsingFK));
   var flightNo = 102;
   var newPilotID = 55;

   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    Flight flight = ctx.FlightSet.Find(flightNo);
    flight.PilotId = newPilotID;
    var count = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + count);
   }
  }

  /// <summary>
  /// Assignment via the shadow key property of a foreign key column
  /// </summary>
  [EFCBook]
  public static void ChangePilotUsingFKShadowProperty()
  {
   CUI.Headline(nameof(ChangePilotUsingFKShadowProperty));
   var flightNo = 102;
   var neuerPilotNr = 123;
   using (var ctx = new WWWingsContext())
   {
    ctx.Log();
    Flight flight = ctx.FlightSet.Find(flightNo);
    ctx.Entry(flight).Property("PilotId").CurrentValue = neuerPilotNr;
    var count = ctx.SaveChanges();
    Console.WriteLine("Number of saved changes: " + count);
   }
  }
 }
}