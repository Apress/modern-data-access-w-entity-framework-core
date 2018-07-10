
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;

namespace EFC_Console.AutoMapper
{


 /// <summary>
 /// Ausgangsklassen
 /// </summary>
 public class Flight1
 {

  public Flight1()
  {

  }

  public int flightNo { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
  public System.DateTime Date;
  public bool NonSmokingFlight { get; set; }
  public short Seats { get; set; }
  public Nullable<short> Freeseats { get; set; }
  public Nullable<short> FreeSeats { get; set; }

  //public Nullable<short> Free_Seats { get; set; }
  //public Nullable<short> Free_seats { get; set; }

  public Nullable<int> Utilization { get; set; }
  public Nullable<int> Pilot_PersonID { get; set; }
  
  private string memo = "xxx";

  
  public string Memo
  {
   private get { return memo; }
   set { memo = value; }
  }

  public Nullable<bool> Bestreikt { get; set; }
  public byte[] Timestamp { get; set; }

 }


 /// <summary>
 /// Ergebnisklasse
 /// </summary>
 public class Flight2
 {

  public Flight2()
  {

  }

  public int Flight_Nr { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
  public System.DateTime Date;
  public bool Non_Smoking_Flight { get; set; }
  public short Plaetze { get; set; }

  public Nullable<short> Seats { get; set; }
  public Nullable<short> FreeSeats { get; set; }
  public Nullable<short> Free_seats { get; set; }
  public Nullable<short> Free_Seats { get; set; }

  public Nullable<int> Utilization { get; set; }
  public Nullable<int> Pilot_PersonID { get; set; }


  private string memo;

  public string Memo
  {
 private  get { return memo; }
  set { memo = value; }
  }


  public Nullable<bool> Bestreikt { get; set; }
  public byte[] Timestamp { get; set; }

  public override string ToString()
  {
   return "Flight " + this.Flight_Nr + " on " + this.Date + ": " + this.Departure + "->" + this.Destination + ": " + this.FreeSeats + "/" + this.FreeSeats + "/" + this.Free_Seats + "/" + this.Free_seats + " (" + this.Memo + ")";



  }
 }

 partial class Demo
 {

  const int Anzahl = 10;


  public static void run()
  {
   Console.WriteLine("Count: " + Anzahl);
   var sw0 = new Stopwatch();
   sw0.Start();
   List<Flight1> Ausgangsliste = new List<Flight1>();

   for (int i = 0; i <= Anzahl; i++)
   {
    var f = new Flight1();
    f.flightNo = i;
    f.Departure = i.ToString();
    f.Destination = i.ToString();

    f.FreeSeats = 1;
    f.FreeSeats = 2;
    //f.Free_Seats = 3;
    //f.Free_seats = 4;
    f.Date = DateTime.Now;
    f.Seats = 200;
    //f.Memo = "test " + i;
    Ausgangsliste.Add(f);
   }
   sw0.Stop();
   Console.WriteLine("Ausgangsliste erzeugen: " + sw0.ElapsedMilliseconds);


   //---------------------------------------- 

   //Mapper.CreateProfile("first_profile"
   //, expression =>
   //{
   // expression.SourceMemberNamingConvention
   //    = new NoNamingConvention();
   // expression.DestinationMemberNamingConvention
   //= new NoNamingConvention();

   //});



   //Mapper.Initialize(cfg =>
   //{
   // Neu in AutoMappre 4.0
   // cfg.BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
   // cfg.AddProfile<AutoMapperFlightProfile>();
   // cfg.Prefixes. = new List<string> { "FL_" };
   // cfg.ReplaceMemberName("FL_", "");
   //});
   //Mapper.CreateMap<Flight1, Flight2>(); // .WithProfile("first_profile");
   //Mapper.AssertConfigurationIsValid();




   List<Flight2> e3 = Mapper.Map<List<Flight2>>(Ausgangsliste);
   Console.WriteLine("Ergebnismenge: " + e3.Count);


   foreach (var f in e3.Take(1))
   {
    Console.WriteLine(f);
    Console.WriteLine("f.FreeSeats = " + f.FreeSeats);
    Console.WriteLine("f.FreeSeats = " + f.FreeSeats);
    Console.WriteLine("f.Free_Seats = " + f.Free_Seats);
    Console.WriteLine("f.Free_seats = " + f.Free_seats);
   }
  }


 }
}
