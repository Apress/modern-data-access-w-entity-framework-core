using BO;
using DA;
using ITVisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVisions.EFC;
using System.Data.Common;
using ITVisions.EFCore;

namespace EFC_Console
{
 public class ValueConverters
 {


  /// <summary>
  /// Test for FrequentFlyer Property
  /// </summary>
  [EFCBook("5.0", "2.1")]
  public static void ConvertStringToBoolean()
  {
   CUI.MainHeadline(nameof(ConvertStringToBoolean));
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();

    // Add new Passenger
    CUI.Headline("Add new Passenger");
   var p = new Passenger();
    p.GivenName = "Max";
    p.Surname = "Müller";
    p.Birthday = new DateTime(2070, 2, 3);
    p.Status = 'A';
    p.FrequentFlyer = "Yes";
    ctx.PassengerSet.Add(p);
    var count = ctx.SaveChanges();

    Console.WriteLine("Saved Changes: " + count);
    Console.WriteLine("Added new Passenger #" + p.PersonID);

    // Get raw data from Database as DataReader
    CUI.Headline("Raw Data");
    var r = ctx.Database.ExecuteSqlQuery("Select p.PersonID, p.Surname, p.Birthday, p.FrequentFlyer from Passenger as p where p.personID= " + p.PersonID);
    DbDataReader dr = r.DbDataReader;
    while (dr.Read())
    {
     Console.WriteLine("{0}\t{1}\t{2}\t{3} \n", dr[0], dr[1], dr[2], dr[3]);
    }
    dr.Dispose();

    // Get all Frequent Travellers
   CUI.Headline("All Frequent Travellers:");
    var ft = ctx.PassengerSet.Where(x => x.FrequentFlyer == "Yes").ToList();
    foreach (var pas in ft)
    {
     Console.WriteLine(pas);
    }

    // Get all Frequent Travellers
    CUI.Headline("All Frequent Travellers: -> StartsWith() does not work!");
    var ft2 = ctx.PassengerSet.Where(x => x.FrequentFlyer.StartsWith("Y")).ToList();
    if (ft2.Count == 0) CUI.PrintError("No passengers :-(");
    foreach (var pas in ft2)
    {
     Console.WriteLine(pas);
    }
   }

  }

  [EFCBook("5.0", "2.1")]
  public static void ConvertEnumToString()
  {
   CUI.MainHeadline(nameof(ConvertEnumToString));
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    // Add new Pilot
    var p = new Pilot();
    p.GivenName = "Max";
    p.Surname = "Müller";
    p.Birthday = new DateTime(2070, 2, 3);
    p.PilotLicenseType = PilotLicenseType.FlightInstructor;
    //p.PilotLicenseType2 = PilotLicenseType2.FlightInstructor;
    ctx.PilotSet.Add(p);
    var count = ctx.SaveChanges();

    Console.WriteLine("Saved Changes: " + count);
    Console.WriteLine("Added new Pilot #" + p.PersonID);

    // Get all Flight Instructors **ERROR in PREVIEW1**
    Console.WriteLine("All Flight Instructors:");
    var ft2 = ctx.PilotSet.Where(x => x.PilotLicenseType == PilotLicenseType.FlightInstructor).ToList();
    foreach (var pas in ft2)
    {
     Console.WriteLine(pas);
    }

    // Get raw data from Database as DataReader
    var r = ctx.Database.ExecuteSqlQuery("Select p.PersonID, p.Surname, p.PilotLicenseType, p.Birthday from Employee as p where p.personID= " + p.PersonID);
    DbDataReader dr = r.DbDataReader;
    while (dr.Read())
    {
     Console.WriteLine("{0}\t{1}\t{2}\t{3} \n", dr[0], dr[1], dr[2], dr[3]);
    }
    dr.Dispose();
   }

  }
 }
}
