using AutoMapper;
using ITVisions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EFC_Console.AutoMapper.Speed
{

 /// <summary>
 /// Ausgangsklassen
 /// </summary>
 public class Flight1
 {

  public int flightNo { get; set; }
  public string departure { get; set; }
  public string Zielort { get; set; }
  public System.DateTime Datum { get; set; }
  public bool NonSmokingFlight { get; set; }
  public short Plaetze { get; set; }
  public Nullable<short> FreeSeats { get; set; }
  public Nullable<int> Auslastung { get; set; }
  public Nullable<int> Pilot_PersonID { get; set; }
  public Nullable<System.DateTime> Ankunft { get; set; }
  public string Memo { get; set; }
  public Nullable<bool> Bestreikt { get; set; }
  public byte[] Timestamp { get; set; }

 }


 /// <summary>
 /// Ergebnisklasse
 /// </summary>
 public  class Flight2
 {

  public int flightNo { get; set; }
  public string departure { get; set; }
  public string Zielort { get; set; }
  public System.DateTime Datum { get; set; }
  public bool NonSmokingFlight { get; set; }
  public short Plaetze { get; set; }
  public Nullable<short> FreeSeats { get; set; }

  public Nullable<int> Auslastung { get; set; }
  public Nullable<int> Pilot_PersonID { get; set; }
  public Nullable<System.DateTime> Ankunft { get; set; }
  public string Memo { get; set; }
  public Nullable<bool> Bestreikt { get; set; }
  public byte[] Timestamp { get; set; }
 }



 class AutoMapperSpeedTest
 {
  static List<int> AnzahlListe = new List<int>() { 1, 10, 100, 1000, 10000, 100000 };


  public static void run()
  {

   for (int i = 0; i < 5; i++)
   {
    foreach (var anz in AnzahlListe)
    {
     runInternal(anz);
    }
   }



  }


   public static void runInternal(int Anzahl)
  {
   CUI.MainHeadline("Count: " + Anzahl);
   var sw0 = new Stopwatch();
   sw0.Start();
   List<Flight1> Ausgangsliste = new List<Flight1>();

   for (int i = 0; i <= Anzahl; i++)
   {
    var f = new Flight1();
    f.flightNo = i;
    f.departure = i.ToString();
    f.Zielort = i.ToString();
    f.FreeSeats = 10;

    f.Plaetze = 1000;
    Ausgangsliste.Add(f);
   }
   sw0.Stop();
   //Console.WriteLine("Ausgangsliste erzeugen: " + sw0.ElapsedMilliseconds);

   //---------------------------------------- 
   var sw1 = new Stopwatch();
   sw1.Start();

   List<Flight2> e1 = new List<Flight2>();

   foreach (var f in Ausgangsliste)
   {
    var fv = MapFlight(f);
    e1.Add(fv);
   }

   sw1.Stop();
   Console.WriteLine("Explizites Mapping für " + e1.Count + " Objekte: " + sw1.ElapsedMilliseconds);
   //---------------------------------------- 
   var sw2 = new Stopwatch();
   sw2.Start();

   List<Flight2> e2 = new List<Flight2>();

   foreach (var f in Ausgangsliste)
   {
    var fv = f.CopyTo<Flight2>();
    e2.Add(fv);
   }

   sw2.Stop();
   Console.WriteLine("Reflection-Mapping für " + e2.Count + " Objekte: " + sw2.ElapsedMilliseconds);

   //---------------------------------------- 
   var sw3 = new Stopwatch();
   sw3.Start();

   Mapper.Initialize(cfg => { 
   cfg.CreateMap<Flight1, Flight2>(); });
   var cm = sw3.ElapsedMilliseconds;
   List<Flight2> e3 = Mapper.Map<List<Flight2>>(Ausgangsliste);

   sw3.Stop();
   Console.WriteLine("Automapper für " + e3.Count + " Objekte: Map: " + sw3.ElapsedMilliseconds + " (davon für CreateMap():" + cm + ")");

  }


  /// <summary>
  /// Explizites Mapping
  /// </summary>
  public static Flight2 MapFlight(Flight1 f)
  {
   var fv = new Flight2();
   fv.departure = f.departure;
   fv.Ankunft = f.Ankunft;
   fv.Auslastung = f.Auslastung;
   fv.Bestreikt = f.Bestreikt;
   fv.Datum = f.Datum;
   fv.flightNo = f.flightNo;
   fv.FreeSeats = f.FreeSeats;
   fv.Memo = f.Memo;
   fv.NonSmokingFlight = f.NonSmokingFlight;
   fv.Pilot_PersonID = f.Pilot_PersonID;
   fv.Plaetze = f.Plaetze;
   fv.Timestamp = f.Timestamp;
   fv.Zielort = f.Zielort;
   return fv;
  }
 }
}
