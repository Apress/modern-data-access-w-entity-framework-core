using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using BO;
using DA;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.SqlClient;
using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using ITVisions.EFCore;

namespace EFC_Console
{
 class Conflicts
 {

  /// <summary>
  /// Simple update for showing [ConcurrencyCheck] and [Timestamp]
  /// </summary>
  [EFCBook("Concurrency")]
  public static void ChangeFlightOneProperty()
  {
   CUI.MainHeadline(nameof(ChangeFlightOneProperty));

   int flightNo = 101;
   using (WWWingsContext ctx = new WWWingsContext())
   {

    // Load flight
    var f = ctx.FlightSet.Find(flightNo);

    Console.WriteLine($"Before changes: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    // Change object in RAM
    f.FreeSeats -= 2;

    Console.WriteLine($"After changes: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! State of the flight object: " + ctx.Entry(f).State);

    // Persist changes
    try
    {
     var anz = ctx.SaveChanges();
     if (anz == 0)
     {
      Console.WriteLine("Problem: No changes saved!");
     }
     else
     {
      Console.WriteLine("Number of saved changes: " + anz);
      Console.WriteLine($"After saving: Flight #{f.FlightNo}: {f.Departure}->{f.Destination} has {f.FreeSeats} free seats! Zustand des Flight-Objekts: " + ctx.Entry(f).State);
     }
    }
    catch (Exception ex)
    {
     Console.WriteLine("Error: " + ex.ToString());
    }
   }
  }

  /// <summary>
  /// Start this code in two processes!
  /// </summary>
  [EFCBook("Concurrency,Conflict")] [Article("dnp78 (09 2017)")]
  public static void ConflictWhileChangingFlight()
  {
   CUI.MainHeadline(nameof(ConflictWhileChangingFlight));
   Console.WriteLine("Process.PersonID=" + Process.GetCurrentProcess().Id);
   Console.Title = nameof(ConflictWhileChangingFlight) + ": Process-PersonID=" + Process.GetCurrentProcess().Id;

   // Flight, where the conflict should arise
   int flightNo = 151;
   
   using (WWWingsContext ctx = new WWWingsContext()) 
   {
    // that is not possible: Read-only!
    //ctx.Model.FindEntityType(typeof(Flight)).FindProperty("FreeSeats").IsConcurrencyToken = true;
    
    // --- Load flight
    Flight flight = ctx.FlightSet.Find(flightNo);
    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": free seats Before: " + flight.FreeSeats);

    short seats = 0;
    string input = "";
    do
    {
     Console.WriteLine("How many seats do you need at this flight?");
     input = Console.ReadLine(); // wait (time to start another process)
    } while (!Int16.TryParse(input, out seats));

    // --- change the free seats
    flight.FreeSeats -= seats;
    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": free seats NEW: " + flight.FreeSeats);

    try
    {
     // --- try to save
     EFC_Util.PrintChangedProperties(ctx.Entry(flight));
     var anz = ctx.SaveChanges();
     Console.WriteLine("SaveChanges: Number of saved changes: " + anz);
    }
    catch (DbUpdateConcurrencyException ex)
    {
     Console.ForegroundColor = ConsoleColor.Red;
     CUI.PrintError(DateTime.Now.ToLongTimeString() + ": Error: another user has already changed the flight!");

     CUI.Print("Conflicts with the following properties:");
     EFC_Util.PrintChangedProperties(ex.Entries.Single());

     // --- Ask the user
     Console.WriteLine("What do you want to do?");
     Console.WriteLine("Key 1: Accept the values of the other user");
     Console.WriteLine("Key 2: Override the values of the other user");
     Console.WriteLine("Key 3: Calculate new value from both records");

     ConsoleKeyInfo key = Console.ReadKey();
     switch(key.Key)
     {
      case ConsoleKey.D1: // Accept the values ​​of the other user
       {
       Console.WriteLine("You have chosen: Option 1: Accept");
       ctx.Entry(flight).Reload();
       break;
      }
      case ConsoleKey.D2: // Override the values ​​of the other user
       {
       Console.WriteLine("You have chosen: Option 2: Override");
       ctx.Entry(flight).OriginalValues.SetValues(ctx.Entry(flight).GetDatabaseValues());
       // wie RefreshMode.ClientWins bei ObjectContext
       EFC_Util.PrintChangeInfo(ctx);
       int anz = ctx.SaveChanges();
       Console.WriteLine("SaveChanges: Saved changes: " + anz);
       break;
      }
      case ConsoleKey.D3: // Calculate new value from both records
       {

        Console.WriteLine("You have chosen: Option 3: Calculate");
        var FreeSeatsOrginal = ctx.Entry(flight).OriginalValues.GetValue<short?>("FreeSeats");
        var FreeSeatsNun = flight.FreeSeats.Value;
        var FreeSeatsInDB = ctx.Entry(flight).GetDatabaseValues().GetValue<short?>("FreeSeats");
        flight.FreeSeats = (short) (FreeSeatsOrginal - 
                            (FreeSeatsOrginal - FreeSeatsNun) -
                            (FreeSeatsOrginal - FreeSeatsInDB));
        EFC_Util.PrintChangeInfo(ctx);
        ctx.Entry(flight).OriginalValues.SetValues(ctx.Entry(flight).GetDatabaseValues());
        int anz = ctx.SaveChanges();
        Console.WriteLine("SaveChanges: Saved changes: " + anz);
        break;
       }
     }
    }
    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": free seats after: " + flight.FreeSeats);

    // --- Cross check the final state in the database
    using (WWWingsContext ctx2 = new WWWingsContext())
    {
     var f = ctx.FlightSet.Where(x => x.FlightNo == flightNo).SingleOrDefault();
     Console.WriteLine(DateTime.Now.ToLongTimeString() + ": free seats cross check: " + f.FreeSeats);

    } // End using-Block -> Dispose()
   }
  }

  /// <summary>
  /// ConflictWhileChangingFlight
  /// </summary>
  public static void EF_Konflikt_BeiFlightMitVermeidungsOption(bool konfliktVermeidung = false)
  {

   CUI.MainHeadline(nameof(ConflictWhileChangingFlight));
   Console.WriteLine("Process PersonID=" + Process.GetCurrentProcess().Id);
   int flightNo = 151;

   // Create context instance unter Verwendung der Verbindungszeichenfolge aus Konfigurationsdatei
   using (WWWingsContext ctx = new WWWingsContext())
   {
    // das geht nicht: Read-only!
    //ctx.Model.FindEntityType(typeof(Flight)).FindProperty("FreeSeats").IsConcurrencyToken = true;
   ctx.Log(); // Protokollierung an der Konsole einschalten über eigene Erweiterungsmethode

    // Load flight
    Flight flight = null;
    // optional: LeseSperre. Achtung: Worst Practices!!!
    IDbContextTransaction t = null;
    if (konfliktVermeidung)
    {
     t = ctx.Database.BeginTransaction(); // Standard ist System.Data.IsolationLevel.ReadCommitted
     Console.WriteLine("Transaktion im Modus " + t.GetDbTransaction().IsolationLevel);
     flight = ctx.FlightSet.FromSql("SELECT * FROM dbo.Flight WITH (UPDLOCK) WHERE FlightNo = {0}", flightNo).SingleOrDefault();
    }
    else
    {
     flight = ctx.FlightSet.SingleOrDefault(x => x.FlightNo == flightNo);
    }

    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": free seats Before: " + flight.FreeSeats);
    Console.ReadLine(); // Warten (zum Starten eines zweiten Processes!)

    // Änderung
    flight.FreeSeats = (short)(new Random(DateTime.Now.Millisecond).Next(1, 100));
    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": free seats NEW: " + flight.FreeSeats);
    try
    {
     // Speicherversuch
     EFC_Util.PrintChangedProperties(ctx.Entry(flight));
     ctx.SaveChanges();
    }
    catch (DbUpdateConcurrencyException ex)
    {
     Console.ForegroundColor = ConsoleColor.Red;
     CUI.PrintError(DateTime.Now.ToLongTimeString() + ": Error: Ein anderer Benutzer has bereits geändert!");

     CUI.Print("Konflikte bei folgenden Eigenschaften:");
     EFC_Util.PrintChangedProperties(ex.Entries.Single());

     // Frage beim Benutzer nach
     Console.WriteLine(
      "Möchten Sie den Wert des anderen Benutzers übernehmen (Taste '1') oder überschreiben (Taste '2')?");
     ConsoleKeyInfo key = Console.ReadKey();
     if (key.Key == ConsoleKey.D1)
     {
      Console.WriteLine("Sie haben gewählt: Option 1: übernehmen");
      // Werte des anderen Benutzers übernehmen
      ctx.Entry(flight).Reload(); // wie RefreshMode.StoreWins bei ObjectContext
     }
     else
     {
      Console.WriteLine("Sie haben gewählt: Option 2: überschreiben");
      Console.ForegroundColor = ConsoleColor.Gray;
      // Werte des anderen Benutzers überschreiben
      ctx.Entry(flight).OriginalValues.SetValues(ctx.Entry(flight).GetDatabaseValues());
      // wie RefreshMode.ClientWins bei ObjectContext
      EFC_Util.PrintChangeInfo(ctx);
      int anz = ctx.SaveChanges();
      Console.WriteLine("SaveChanges: Saved changes: " + anz);
     }
    }
    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": FreeSeats After: " + flight.FreeSeats);

    if (konfliktVermeidung) t.Commit();

    using (WWWingsContext ctx2 = new WWWingsContext())
    {
     var f = ctx.FlightSet.Where(x => x.FlightNo == flightNo).SingleOrDefault();
     Console.WriteLine(DateTime.Now.ToLongTimeString() + ": FreeSeats: " + f.FreeSeats);

    } // End using-Block -> Dispose()
   }
  }
  /// <summary>
  /// EF_Konflikt_BeiMetadaten_MitRowversion
  /// </summary>
  //public static void EF_Konflikt_BeiMetadaten_MitRowversion()
  //{

  // Console.WriteLine("EFTest_Speichern/Beispiel5_Konflikt in Process PersonID=" + System.Diagnostics.Process.GetCurrentProcess().Id);
  // string Surname = "LetzteAktion";

  // // Create context instance unter Verwendung der Verbindungszeichenfolge aus Konfigurationsdatei
  // using (WWWingsContext modell = new WWWingsContext())
  // {

  //  var m1 = modell.Metadaten.Where(x => x.Surname == Surname).SingleOrDefault();

  //  Console.WriteLine(DateTime.Now.ToLongTimeString() + ": Letzte Aktion - Wert: " + m1.Value);
  //  Console.ReadLine(); // Warten (zum Starten eines zweiten Processes!)

  //  // Änderung
  //  m1.Value = DateTime.Now.ToString();
  //  Console.WriteLine(DateTime.Now.ToLongTimeString() + ":  Letzte Aktion - Wert NEW: " + m1.Value);
  //  try
  //  {
  //   // Speicherversuch
  //   EF_Util.PrintChangeInfo(modell);
  //   int anz = modell.SaveChanges();
  //   Console.WriteLine("SaveChanges: Saved changes: " + anz);
  //  }
  //  catch (DbUpdateConcurrencyException ex)
  //  {
  //   Console.ForegroundColor = ConsoleColor.Red;
  //   // Zeige aktuellen Wert aus DB
  //   Console.WriteLine(DateTime.Now.ToLongTimeString() + ": Error: Ein anderer Benutzer has bereits geändert!");
  //   // Eigenes Modell, um den aktuellen Wert zu holen
  //   WWWings6Entities modell2 = new WWWings6Entities();
  //   var m2 =  (Metadaten)ex.Entries.Single().GetDatabaseValues().ToObject();
  //   Console.WriteLine(DateTime.Now.ToLongTimeString() + ": Der andere Benutzer has gespeichert: Wert: " + m2.Value);

  //   // Frage nach
  //   Console.WriteLine("Möchten Sie den Wert des anderen Benutzers übernehmen (Taste '1') oder überschreiben (Taste '2')?");
  //   ConsoleKeyInfo key = Console.ReadKey();
  //   if (key.Key == ConsoleKey.D1)
  //   {
  //    Console.WriteLine("Sie haben gewählt: Option 1: übernehmen");
  //    modell.Entry(m1).Reload(); // wie RefreshMode.StoreWins bei ObjectContext
  //   }
  //   else
  //   {
  //    Console.WriteLine("Sie haben gewählt: Option 2: überschreiben");

  //    Console.ForegroundColor = ConsoleColor.Gray;
  //    modell.Entry(m1).OriginalValues.SetValues(modell.Entry(m1).GetDatabaseValues()); // wie RefreshMode.ClientWins bei ObjectContext
  //    EF_Util.PrintChangeInfo(modell);
  //    int anz = modell.SaveChanges();
  //    Console.WriteLine("SaveChanges: Saved changes: " + anz);
  //   }

  //  }
  //  Console.WriteLine(DateTime.Now.ToLongTimeString() + ": Letzte Aktion -  Wert After: " + m1.Value);

  // } // End using-Block -> Dispose()
  //}
 }
}
