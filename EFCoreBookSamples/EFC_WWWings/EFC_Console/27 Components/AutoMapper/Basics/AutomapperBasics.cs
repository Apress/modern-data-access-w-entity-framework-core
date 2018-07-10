using AutoMapper;
using BO;
using DA;
using EFC_Console.ViewModels;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC_Console.AutoMapper
{


 public class AutoMapperBasics
 {

  [EFCBook()]
  public static void Demo_SingleObject()
  {
   CUI.MainHeadline(nameof(Demo_SingleObject));
   
   // take the first flight as an example
   var ctx = new WWWingsContext();
   var flight = ctx.FlightSet.Include(x=>x.Pilot).Include(x => x.AircraftType).ThenInclude(y=>y.Detail).FirstOrDefault();
   Console.WriteLine(flight);

   //################################################## 

   //Mapper.Initialize(cfg =>
   //{
   // cfg.CreateMap<Flight, FlightView>();
   // cfg.CreateMap<FlightView, Flight>();
   // cfg.CreateMap<Passenger, PassangerView>();
   // cfg.CreateMap<PassangerView, Passenger>();
   // cfg.CreateMap<Pilot, Pilot>();
   // cfg.CreateMap<Pilot, Pilot>();

   // cfg.CreateMap<Flight, FlightDTOShort>();
   // cfg.CreateMap<Flight, FlightDTO>();
   //});

   //Mapper.Initialize(cfg =>
   //{
   // cfg.CreateMap<Flight, FlightView>().ReverseMap();
   // cfg.CreateMap<Passenger, PassangerView>().ReverseMap();
   // cfg.CreateMap<Pilot, Pilot>().ReverseMap();
   //});


   Mapper.Initialize(cfg =>
   {
    //cfg.SourceMemberNamingConvention = new NoNamingConvention();
    //cfg.DestinationMemberNamingConvention = new NoNamingConvention();

    cfg.CreateMap<Flight, FlightView>().ReverseMap();
    cfg.CreateMap<Passenger, PassengerView>().ReverseMap();
    cfg.CreateMap<Pilot, PilotView>().ReverseMap();

    cfg.CreateMap<Flight, FlightDTOShort>();
    cfg.CreateMap<Flight, FlightDTO>();
   });



   //Mapper.Initialize(cfg =>
   //{
   // cfg.AddProfile<AutoMapperProfile1>();
   //});

   Mapper.Initialize(cfg =>
   {
    cfg.AddProfile<AutoMapperProfile2>();
   });


   // ---------------------- 
   CUI.Headline("Mapping to new object");
   FlightView flightView1 = Mapper.Map<FlightView>(flight);

   Console.WriteLine(flightView1);
   Console.WriteLine(flightView1.PilotSurname);
   Console.WriteLine(flightView1.SmokerInfo);
   Console.WriteLine(flightView1.PilotInfo);
   if (flightView1.Pilot == null) CUI.PrintError("No pilot!");
   else
   {
    Console.WriteLine(flightView1.Pilot?.Surname + " born " + flightView1.Pilot?.Birthday);
   }
   Console.WriteLine(flightView1.Memo);
   Console.WriteLine(flightView1.AircraftTypeDetailLength);

   FlightView flightView2 = (FlightView)Mapper.Map(flight, flight.GetType(), typeof(FlightView));

   Console.WriteLine(flightView2);
   Console.WriteLine(flightView2.PilotSurname);
   Console.WriteLine(flightView2.SmokerInfo);
   Console.WriteLine(flightView2.PilotInfo);
   if (flightView2.Pilot == null) CUI.PrintError("No pilot!");
   else
   {
    Console.WriteLine(flightView2.Pilot?.Surname + " born " + flightView2.Pilot?.Birthday);
   }
   Console.WriteLine(flightView2.AircraftTypeDetailLength);
   Console.WriteLine(flightView2.Memo);
   Console.WriteLine(flightView2.Copilot);

   // ---------------------- 
   CUI.Headline("Mapping to existing object");
   var flightView3 = new FlightView();
   Mapper.Map(flight, flightView3);

   Console.WriteLine(flightView3);
   Console.WriteLine(flightView3.PilotSurname);
   Console.WriteLine(flightView3.SmokerInfo);
   Console.WriteLine(flightView3.PilotInfo);
   if (flightView3.Pilot == null) CUI.PrintError("No pilot!");
   else
   {
    Console.WriteLine(flightView3.Pilot?.Surname + " born " + flightView3.Pilot?.Birthday);
   }
   Console.WriteLine(flightView3.Memo);



   CUI.Headline("Mapping with non-static types");
   var config = new MapperConfiguration(cfg => {
    cfg.CreateMap<Flight, FlightView>();
    cfg.CreateMap<Pilot, PilotView>();
    cfg.CreateMap<Passenger, PassengerView>();
    cfg.AddProfile<AutoMapperProfile2>();
   });
  // config.AssertConfigurationIsValid();
   
   IMapper mapper = config.CreateMapper();
   var flightView4 = mapper.Map<Flight, FlightView>(flight);
   Console.WriteLine(flightView4.PilotSurname);
   Console.WriteLine(flightView4.SmokerInfo);
   Console.WriteLine(flightView4.PilotInfo);
   if (flightView4.Pilot == null) CUI.PrintError("No pilot!");
   else
   {
    Console.WriteLine(flightView4.Pilot?.Surname + " born " + flightView4.Pilot?.Birthday);
   }
   Console.WriteLine(flightView4.Memo);

   // ------------------ Umgekehrtes Mapping
   CUI.Headline("Reverse Mapping");
   var flight2 = Mapper.Map<Flight>(flightView3);
   Console.WriteLine(flight2);
  }


  /// <summary>
  /// Mapping a list of Flight objects with related objects Booking/Passenger to FlightView with PassengerView
  /// </summary>
 [EFCBook()]
  public static void Demo_ListMapping()
  {
   CUI.Headline(nameof(Demo_ListMapping));

   Mapper.Initialize(cfg =>
   {
    cfg.AddProfile<AutoMapperProfile2>();
   });

   using (var ctx2 = new WWWingsContext())
   {
    var flightSet = ctx2.FlightSet.Include(f => f.Pilot).Include(f => f.BookingSet).ThenInclude(x => x.Passenger).Where(f => f.Departure == "Berlin").OrderBy(f => f.FlightNo).Take(5).ToList();
    // map all objects in this list
    List<FlightView> flightviewListe = Mapper.Map<List<FlightView>>(flightSet);
    foreach (var f in flightviewListe)
    {
     Console.WriteLine(f.ToString());
     if (f.Passengers != null)
     {
      foreach (var pas in f.Passengers)
      {
       Console.WriteLine("   - " + pas.GivenName + " " + pas.Surname + " has " + pas.FlightViewSet.Count + " Flights!");
      }
     }
    }
   }
  }
 }
}
