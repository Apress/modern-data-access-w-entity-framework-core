using System;
using System.Diagnostics;
using DA;
using BO;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace EFC_Console
{

 /// <summary>
 /// Einfachste Version einer Repository-Klasse für Flights
 /// </summary>
 class Repository : IDisposable
 {
  private WWWingsContext ctx = new WWWingsContext();
  public void Dispose()
  {
   ctx.Dispose();
  }

  public Flight GetFlight(int id)
  {
   return ctx.FlightSet.Find(id);
  }

  public int Save()
  {
   return ctx.SaveChanges();
  }
 }

 class AttachedVsDetachedObjectsDemo
 {

  public static void Attached()
  {
   CUI.Headline("Attached");
   using (WWWingsContext ctx = new WWWingsContext())
   {
    // Objekt laden
    var flight = ctx.FlightSet.Find(150);
    // Objekt ändern
    flight.FreeSeats--;
    flight.Date = flight.Date.AddHours(2);
    // Speichern
    var count = ctx.SaveChanges();
    Console.WriteLine("Saved changes: " + count.ToString());
   }
  }


  public static void Attached_Repository()
  {
   CUI.Headline("Attached_Repository");
   using (Repository rep = new Repository())
   {
    // Objekt laden
    var flight = rep.GetFlight(111);
    // Objekt ändern
    flight.FreeSeats--;
    flight.Date = flight.Date.AddHours(2);
    // Speichern
    var count = rep.Save();
    Console.WriteLine("Saved changes: " + count.ToString());
   }
  }

  /// <summary>
  ///  Attach()/EntityState.Modified --> All Spalten zurückschreiben  
  /// </summary>
  public static void Attached_Flight()
  {
   CUI.MainHeadline(nameof(Attached_Flight));
   Flight f;
   CUI.Print("Lade Objekt in Kontextinstanz #1");
   using (WWWingsContext ctx1 = new WWWingsContext())
   {
    ctx1.Log();
    f = ctx1.FlightSet.Find(101);

   
   CUI.Print("Objekt ändern");
   CUI.Print(f.ToString());
   f.Memo = "last changed at " + DateTime.Now;
   f.FreeSeats--;
   CUI.Print(f.ToString());

  
    var count = ctx1.SaveChanges();
    Console.WriteLine("Saved changes: " + count.ToString());
    if (count != 1) Debugger.Break();
   }
  }

  /// <summary>
  ///  Attach()/EntityState.Modified --> All Spalten zurückschreiben  
  /// </summary>
  public static void Detached_Flight()
  {
   CUI.MainHeadline(nameof(Detached_Flight));
   Flight f;
   CUI.Print("Lade Objekt in Kontextinstanz #1");
   using (WWWingsContext ctx1 = new WWWingsContext())
   {
    ctx1.Log();
    f = ctx1.FlightSet.Find(101);
   }
   // Flight ist nun "Detached"!

   CUI.Print("Objekt ändern");
   CUI.Print(f.ToString());
   f.Memo = "last changed at " + DateTime.Now;
   f.FreeSeats--;
   CUI.Print(f.ToString());

   CUI.Print("Objekt nun speichern mit Kontextinstanz #2");
   using (WWWingsContext ctx2 = new WWWingsContext())
   {
  
    CUI.Print(ctx2.Entry(f).State.ToString());
    ctx2.FlightSet.Attach(f);

    CUI.Print(ctx2.Entry(f).State.ToString());
    ctx2.Entry(f).State = EntityState.Modified;
    CUI.Print(ctx2.Entry(f).State.ToString());
    var count = ctx2.SaveChanges();
    Console.WriteLine("Saved changes: " + count.ToString());
    if (count != 1) Debugger.Break();
   }
  }



  /// <summary>
  /// Nur geänderte Spalten zurückschreiben
  /// Attach()/IsModified=true
  /// achtung: Geht nicht wenn Concurrency Check auf FreeSeats, da da EF den neuen Wert auch als alten Wert annimmt!
  /// </summary>
  public static void Detached_Flight_EinzelneProperties()
  {
   CUI.MainHeadline(nameof(Detached_Flight_EinzelneProperties));
   Flight flight;
   CUI.Print("Lade Objekt in Kontextinstanz #1");
   using (WWWingsContext ctx1 = new WWWingsContext())
   {
    ctx1.Log();
    //ctx1.Configuration.LazyLoadingEnabled = false;
    flight = ctx1.FlightSet.Find(110);
   }

   CUI.Print("Objekt ändern");
   CUI.Print(flight.ToString());
   flight.Memo = "last changed at " + DateTime.Now;
   flight.FreeSeats--;
   CUI.Print(flight.ToString());

   CUI.Print("Objekt nun speichern mit Kontextinstanz #2");
   using (WWWingsContext ctx2 = new WWWingsContext())
   {
    ctx2.Log();
    ctx2.FlightSet.Attach(flight);
    CUI.Print(ctx2.Entry(flight).State.ToString());

    // Zustand einzelner Properties ändern
    ctx2.Entry(flight).Property(x => x.Memo).IsModified = true;
    ctx2.Entry(flight).Property(x => x.FreeSeats).IsModified = true;
    CUI.Print(ctx2.Entry(flight).State.ToString());
    var count = ctx2.SaveChanges();
    Console.WriteLine("Saved changes: " + count.ToString());
    if (count != 1) Debugger.Break();
   }
  }

  /// <summary>
  /// Variante 3 UNSINNIG !!!
  /// Wenn Concurrency Check nicht auf Timestamp, sondern auf Wertespalten (außer Timestamp) aktiv, gehen Varianten 1 und 2 nicht, weil EF die neuen Werte auch als Originalwerte annehmen wird und die nicht in DB finden kann
  /// Hier wird aber fehlerhafterweise der Inhalt der DB nochmal gelesen, weil den Concurreny Check absurd macht
  /// </summary>
  public static void Detached_Flight_ConcurrencyCheck_AusDB()
  {
   CUI.MainHeadline(nameof(Detached_Flight_ConcurrencyCheck_AusDB));
   Flight flight;
   CUI.Print("Lade Objekt in Kontextinstanz #1");
   using (WWWingsContext ctx1 = new WWWingsContext())
   {
    ctx1.Log();
    //ctx1.Configuration.LazyLoadingEnabled = false;
    flight = ctx1.FlightSet.Find(110);
   }

   CUI.Print("Objekt ändern");
   CUI.Print(flight.ToString());
   flight.FreeSeats--;
   flight.Date = flight.Date.AddHours(2);
   CUI.Print(flight.ToString());

   CUI.Print("Objekt nun speichern mit Kontextinstanz #2");
   using (WWWingsContext ctx2 = new WWWingsContext())
   {
    ctx2.Log();
    // Wegen des Concurreny Checks brauchen wir die Originalwerte entweder aus dem Objekt selbst oder der DB
    // Aus DB ist aber keine gute Lösung, denn damit ist der Concurreny Check auf den Spalten unsinnig
    // hier aus DB
    var flightOrg = ctx2.FlightSet.Find(110);
    CUI.Print(ctx2.Entry(flightOrg).State.ToString());

    // Nun Werte des alten Objekts auf das neue kopieren
    ctx2.Entry(flightOrg).CurrentValues.SetValues(flight);
    CUI.Print(ctx2.Entry(flightOrg).State.ToString());

    var count = ctx2.SaveChanges();
    Console.WriteLine("Saved changes: " + count.ToString());
    if (count != 1) Debugger.Break();
   }
  }

  // Nun prüfen, ob Timestamp noch stimmt (sonst müsste ich mein altes Objekt noch haben, um All Werte zu vergleichen!
  // Achtung: Vergleich mit == geht hier nicht, weil Byte-Array!
  //if (!flightOrginal.Timestamp.SequenceEqual(flight.Timestamp))
  //{
  // throw new System.Data.DBConcurrencyException("Flight " + flight.FlightNo + " wurde from jemand anderem geändert!");
  //}


  /// <summary>
  /// Variante 3b fehlerhaft (Richtig: Original-Objekt noch haben)
  /// </summary>
  public static void Detached_Flight_ConcurrencyCheck_CloneOrg()
  {
   CUI.MainHeadline(nameof(Detached_Flight_ConcurrencyCheck_CloneOrg));
   Flight flight;
   Flight flightOrginal;
   CUI.Print("Lade Objekt in Kontextinstanz #1");
   using (WWWingsContext ctx1 = new WWWingsContext())
   {
    ctx1.Log();
    //ctx1.Configuration.LazyLoadingEnabled = false;
    flight = ctx1.FlightSet.Find(110);
    // Kopie anlegen
    flightOrginal = Cloner.Clone<Flight>(flight);
   }

   CUI.Print("Objekt ändern");
   CUI.Print(flight.ToString());
   flight.FreeSeats--;
   flight.Date = flight.Date.AddHours(2);
   CUI.Print(flight.ToString());

   CUI.Print("Objekt nun speichern mit Kontextinstanz #2");
   using (WWWingsContext ctx2 = new WWWingsContext())
   {
    ctx2.Log();
    ctx2.FlightSet.Attach(flight);

    var entry = ctx2.Entry(flight);
    CUI.Print(ctx2.Entry(flight).State.ToString());
    // Nun Originalwerte im Kontext ablegen. Besser als die aktuellen Werte aus DB laden!

    // Variante 1:
    ctx2.Entry(flight).OriginalValues.SetValues(flightOrginal);

    // Variante 2:
    //var flightOrgDic = ObjectToDictionaryHelper.ToDictionary(flightOrginal);
    //foreach (string propertyName in entry.OriginalValues.PropertyNames)
    //{
    // entry.Property(propertyName).OriginalValue = flightOrgDic[propertyName];
    //}

    CUI.Print(ctx2.Entry(flight).State.ToString());


    var count = ctx2.SaveChanges();
    Console.WriteLine("Saved changes: " + count.ToString());
    if (count != 1) Debugger.Break();
   }
  }
 }

}
