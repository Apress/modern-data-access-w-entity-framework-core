using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ITVisions.Linq.Dynamic;
using BO;
using ITVisions;
using DA;
using System.Linq.Expressions;
using ITVisions.EFCore;

namespace EFC_Console
{
 class QueriesDynamic
 {

  /// <summary>
  /// LINQ Composition
  /// </summary>
  [EFCBook]
  public static void LINQComposition()
  {
   CUI.MainHeadline(nameof(LINQComposition));
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    string departure = "Paris";
    string destination = "";
    bool onlyWithFreeSeats = true;
    bool sortieren = true;

    // Base query
    IQueryable<Flight> flightQuery = (from f in ctx.FlightSet select f);

    // Adding optional condition
    if (!String.IsNullOrEmpty(departure)) flightQuery = from f in flightQuery where f.Departure == departure select f;
    if (!String.IsNullOrEmpty(destination)) flightQuery = from f in flightQuery where f.Destination == destination select f;
    // Adding optional condition using a method
    if (onlyWithFreeSeats) flightQuery = FreeSeatsMustBeGreaterZero(flightQuery);
    // Optional sorting
    if (sortieren) flightQuery = flightQuery.OrderBy(f => f.Date);

    // Send to the database now!
    List<Flight> flightSet = flightQuery.ToList();

    // Print the result set
    Console.WriteLine("Flights found:");
    foreach (var f in flightSet)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination}: {f.FreeSeats} free seats! Pilot: {f.PilotId} ");
    }
   }
  }

  static public IQueryable<Flight> FreeSeatsMustBeGreaterZero(IQueryable<Flight> query)
  {
   return query.Where(f => f.FreeSeats > 0);
  }

  /// <summary>
  /// Example of using expression trees to modify an existing LINQ query on EF Core
  /// </summary>
  [EFCBook]
  public static void ExpressionTreeTwoConditions()
  {
   CUI.MainHeadline(nameof(ExpressionTreeTwoConditions));

   string destination = "Rome";
   short? MindestzahlFreierPlaetze = 10;

   using (WWWingsContext ctx = new WWWingsContext())
   {

    // Base query
    IQueryable<BO.Flight> query = from flight in ctx.FlightSet where flight.FlightNo < 300 select flight;

    // Optional conditions
    if (!String.IsNullOrEmpty(destination) && MindestzahlFreierPlaetze > 0)
    {

     // Laufvariable definieren
     ParameterExpression f = Expression.Parameter(typeof(BO.Flight), "f");

     // Add first condition
     Expression left = Expression.Property(f, "Destination");
     Expression right = Expression.Constant(destination);
     Expression condition1 = Expression.Equal(left, right);

     // Add seconds condition
     left = Expression.Property(f, "FreeSeats");
     right = Expression.Constant((short?)MindestzahlFreierPlaetze, typeof(short?));
     Expression condition2 = Expression.GreaterThan(left, right);

     // Connect conditions with AND operator
     Expression predicateBody = Expression.And(condition1, condition2);

     // Build expression tree 
     MethodCallExpression whereCallExpression = Expression.Call(
         typeof(Queryable),
         "Where",
         new Type[] { query.ElementType },
         query.Expression,
         Expression.Lambda<Func<BO.Flight, bool>>(predicateBody, new ParameterExpression[] { f }));

     // Create query from expression tree
     query = query.Provider.CreateQuery<BO.Flight>(whereCallExpression);
    }

    ctx.Log();
    // Print the result set
    Console.WriteLine("Flights found:");
    foreach (BO.Flight f in query.ToList())
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination}: {f.FreeSeats} free seats! Pilot: {f.PilotId} ");
    }
   }
  }

  /// <summary>
  /// Example of using expression trees to modify an existing LINQ query on EF Core with a variable number of confitions
  /// </summary>
  [EFCBook]
  public static void ExpressionTreeNumerousConditions()
  {
   CUI.MainHeadline(nameof(ExpressionTreeNumerousConditions));

   // Input data
   var filters = new SortedDictionary<string, object>() { { "Departure", "Berlin" }, { "Destination", "Rome" }, { "PilotID", 57 } };

   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    // Base query
    var baseQuery = from flight in ctx.FlightSet where flight.FlightNo < 1000 select flight;

    ParameterExpression param = Expression.Parameter(typeof(BO.Flight), "f");

    Expression completeCondition = null;
    foreach (var filter in filters)
    {
     // Define condition
     Expression left = Expression.Property(param, filter.Key);
     Expression right = Expression.Constant(filter.Value);
     Expression condition = Expression.Equal(left, right);
     // Add to existing conditions using AND operator
     if (completeCondition == null) completeCondition = condition;
     else completeCondition = Expression.And(completeCondition, condition); 
    }

    // Create query from expression tree
    MethodCallExpression whereCallExpression = Expression.Call(
        typeof(Queryable),
        "Where",
        new Type[] { baseQuery.ElementType },
        baseQuery.Expression,
        Expression.Lambda<Func<BO.Flight, bool>>(completeCondition, new ParameterExpression[] { param }));

    // Create query from expression tree
    var Q_Endgueltig = baseQuery.Provider.CreateQuery<BO.Flight>(whereCallExpression);

    // Print the result set
    Console.WriteLine("Flights found:");
    foreach (var f in Q_Endgueltig)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination}: {f.FreeSeats} free seats! Pilot: {f.PilotId} ");
    }
   }
  }

  /// <summary>
  /// Use Dynamic LINQ
  /// </summary>
  [EFCBook]
  public static void DynamicLINQNumerousCondition()
  {
   CUI.MainHeadline(nameof(DynamicLINQNumerousCondition));
   // input data
   var filters = new SortedDictionary<string, object>() { { "Departure", "Berlin" }, { "Destination", "Rome" }, { "PilotID", 57 } };
   string sorting = "FreeSeats desc";

   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    // base query
    IQueryable<BO.Flight> query = from flight in ctx.FlightSet where flight.FlightNo < 300 select flight;

    // Add conditions
    foreach (var filter in filters)
    {
     Console.WriteLine(filter.Value.GetType().Name);
     switch (filter.Value.GetType().Name)
     {
      case "String":
       query = query.Where(filter.Key + " = \"" + filter.Value + "\""); break;

      default:
       query = query.Where(filter.Key + " = " + filter.Value); break;
     }
    }

    // optional sorting
    if (!String.IsNullOrEmpty(sorting)) query = query.OrderBy(sorting);

    // Print the result set
    Console.WriteLine("Flights found:");
    foreach (var f in query)
    {
     Console.WriteLine($"Flight Nr {f.FlightNo} from {f.Departure} to {f.Destination}: {f.FreeSeats} free seats!");
    }
   }
  }



  /// <summary>
  /// 
  /// </summary>
  [NotYetInTheBook]
  public static void DynamicLINQCompositionBasequery()
  {
   CUI.MainHeadline(nameof(DynamicLINQCompositionBasequery));
   using (WWWingsContext ctx = new WWWingsContext())
   {

    string departure = "Paris";
    string destination = "";

    IQueryable<Flight> flightBasisQuery = (from f in ctx.FlightSet select f);

    IQueryable<Flight> flightQueryEndgueltig = FreeSeatsMustBeGreaterZeroDynamic(flightBasisQuery);

    if (!String.IsNullOrEmpty(departure)) flightQueryEndgueltig = from f in flightQueryEndgueltig where f.Departure == departure select f;
    // or: System.Dynamic.Linq 
    //flightQueryEndgueltig = flightQueryEndgueltig.Where("it.Departure = \"Paris\"");

    if (!String.IsNullOrEmpty(destination)) flightQueryEndgueltig = from f in flightQueryEndgueltig where f.Destination == destination select f;

    // Send to the database now!
    List<Flight> flightSet = flightQueryEndgueltig.ToList();

    foreach (var f in flightSet)
    {
     Console.WriteLine("Flight: " + f.FlightNo + " from " + f.Departure + " to " +
                       f.Destination + " has " + f.FreeSeats + " free seats");
    }
   }
  }


  static public dynamic FreeSeatsMustBeGreaterZeroDynamic(IQueryable<object> query)
  {
   // Alternative 1:
   return query.Where("it.FreeSeats > 0");

   // it.FreeSeats > 240 || it.FreeSeats < 10

   // Alternative 2:
   //return DynamicQueryable.Where(query, "it.FreeSeats > 0");
  }

 }
}
