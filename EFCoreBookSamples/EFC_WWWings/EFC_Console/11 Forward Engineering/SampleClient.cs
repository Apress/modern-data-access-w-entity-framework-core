using DA;
using BO;
using System;
using System.Linq;

namespace EFC_Console
{
 class SampleClientForward
 {
  public static void Run()
  {
   Console.WriteLine("Start...");
   using (var ctx = new WWWingsContext())
   {
    // Create database at runtime, if not available!
    var e = ctx.Database.EnsureCreated();
    if (e) Console.WriteLine("Database has been created!");
    // Create passenger object
    var newPassenger = new Passenger();
    newPassenger.GivenName = "Holger";
    newPassenger.Surname = "Schwichtenberg";
    // Append Passenger to EFC context
    ctx.PassengerSet.Add(newPassenger);
    // Save object
    var count = ctx.SaveChanges();
    Console.WriteLine("Number of changes: " + count);
    // Read all passengers from the database
    var passengerSet = ctx.PassengerSet.ToList();
    Console.WriteLine("Number of passengers: " + passengerSet.Count);
    // Filter with LINQ-to-Objects
    foreach (var p in passengerSet.Where(x => x.Surname == "Schwichtenberg").ToList())
    {
     Console.WriteLine(p.PersonID + ": " + p.GivenName + " " + p.Surname);
    }
   }
   Console.WriteLine("Done!");
   Console.ReadLine();
  }

 }
}
