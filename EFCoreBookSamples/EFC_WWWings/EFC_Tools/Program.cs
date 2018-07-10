using EFC_Console;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ITVisions.EFC;
using CommandLine;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
// Required NUGET packages: CommandLine (https://github.com/commandlineparser/commandline)

namespace EFC_Tools
{
 class Program
 {
  static int Main(string[] args)
  {
   CUI.H1("EFCTools v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
   CUI.Print("(C) Dr. Holger Schwichtenberg 2017-" + System.DateTime.Now.Year);
   return CommandLine.Parser.Default.ParseArguments<MigrateVerb, CreateTestDataVerb>(args)
 .MapResult(
   (MigrateVerb opts) => opts.Migrate(),
      (BothVerb opts) => opts.Both(),
   (CreateTestDataVerb opts) => opts.CreateTestData(),
   errs => 1);
  }
 }

 [Verb("Both", HelpText = "Migrate the database and create test data")]
 public class BothVerb
 {
  [Option('c', "connectionstring", Default = "", HelpText = "Connection String for database")]
  public string ConnectionString { get; set; } = "";
  public int Both()
  {
   if (!(String.IsNullOrEmpty(this.ConnectionString)))
   {
    DA.WWWingsContext.ConnectionString = this.ConnectionString;
   }
   new MigrateVerb().Migrate();
   new CreateTestDataVerb().CreateTestData();
   return 0;
  }
 }

 [Verb("migrate", HelpText = "Migrate the database")]
 public class MigrateVerb
 {
  [Option('c', "connectionstring", Default = "", HelpText = "Connection String for database")]
  public string ConnectionString { get; set; } = "";

  public int Migrate()
  {
   CUI.H1("Migrate Database...");
   try
   {
    if (!(String.IsNullOrEmpty(this.ConnectionString)))
    {
     DA.WWWingsContext.ConnectionString = this.ConnectionString;
    }
    CUI.Print("Connection String=" + DA.WWWingsContext.ConnectionString);

    using (var ctx = new DA.WWWingsContext())
    {


     IEnumerable<string> mset = ctx.Database.GetMigrations();
     CUI.H2("Available Migrations: " + mset.Count());
     foreach (var m in mset)
     {
      Console.WriteLine(m);
     }


     IEnumerable<string> appliedMigrations = ctx.Database.GetAppliedMigrations();
     CUI.H2("Existing Migrations in this database: " + appliedMigrations.Count());
     foreach (var m in appliedMigrations)
     {
      Console.WriteLine(m);
     }


     //var migrator = ctx.GetService<IMigrator>();
     //var script = migrator.GenerateScript("v5", "v8", true);
     var migrator = ctx.GetService<IMigrator>();
     migrator.Migrate("v8");
     migrator.MigrateAsync("v8");

     PrintMigrationStatus(ctx);
     CUI.H2("Starting Migration...");
     //ctx.Database.EnsureCreated(); // DO NOT USE THIS METHOD BEFORE!
     ctx.Database.Migrate();
     CUI.PrintGreen("Migrations done!");

     PrintMigrationStatus(ctx);
     return 0;
    } // end using
   }
   catch (Exception ex)
   {
    CUI.PrintError("Migration Error: " + ex.ToString());
    System.Environment.Exit(2);
    return 2;
   }
  } // end Mirgate()


  private static void PrintMigrationStatus(DA.WWWingsContext ctx)
  {
   CUI.H2("Migration Status");
   try
   {
    Dictionary<string, string> migrationsStatus = new Dictionary<string, string>();
    var migrations = ctx.Database.GetMigrationStatus();

    foreach (var item in migrations)
    {
     if (item.Value) CUI.PrintGreen(item.Key + ":" + " Applied");
     else CUI.PrintRed(item.Key + ":" + " TODO");
    }
   }
   catch (Exception)
   {
    CUI.PrintError("Database not available!");
   }

  }
 } // end class

 [Verb("createtestdata", HelpText = "Create test data")]
 public class CreateTestDataVerb
 {
  [Option('c', "connectionstring", Default = "", HelpText = "Connection String for database")]
  public string ConnectionString { get; set; } = "";

  [Option('f', "flightcount", Default = 1000, HelpText = "Number of Flights to create")]
  public int FlightCount { get; set; } = 1000;

  [Option('p', "passengercount", Default = 1000, HelpText = "Number of Passengers to create")]
  public int PassengerCount { get; set; } = 2000;

  [Option('i', "pilotcount", Default = 100, HelpText = "Number of Pilots to create")]
  public int PilotCount { get; set; } = 100;

  public int CreateTestData()
  {
   CUI.H1("Create Test Data");
   if (!(String.IsNullOrEmpty(this.ConnectionString)))
   {
    DA.WWWingsContext.ConnectionString = this.ConnectionString;
   }
   CUI.PrintSuccess("Connection String=" + DA.WWWingsContext.ConnectionString);

   try
   {
    DataGenerator.Run(true, this.FlightCount, this.PilotCount, this.PassengerCount);
    CUI.PrintSuccess("CreateTestData done!");
    return 0;
   }
   catch (Exception)
   {

    return 1;
   }

  }
  //public static void PrintInfo(string s)
  //{
  // CUI.PrintDebugWarning(s);
  //}

  //public static void PrintError(string s, Exception ex = null)
  //{
  // // VSTS/TFS Logging Commands https://github.com/Microsoft/vsts-tasks/blob/master/docs/authoring/commands.md
  // s = s += "##vso[task.logissue type=error;]" + s + (ex != null ? ": " + ex.Message : "");
  // CUI.PrintError(s);
  //}
 } // end class

} // End namespace
