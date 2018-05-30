using AutoMapper;
using BO;
using EFC_Console.ViewModels;

namespace EFC_Console.AutoMapper
{
 /// <summary>
 /// Simple profile class for AutoMapper
 /// </summary
 [EFCBook()]
 public class AutoMapperProfile1 : Profile
  {
   public AutoMapperProfile1()
   {
    this.SourceMemberNamingConvention = new NoNamingConvention();
    this.DestinationMemberNamingConvention = new NoNamingConvention();
    this.CreateMap<Flight, FlightView>().ReverseMap();
    this.CreateMap<Passenger, PassengerView>().ReverseMap();
    this.CreateMap<Pilot, PilotView>().ReverseMap();
    this.CreateMap<Flight, FlightDTOShort>();
    this.CreateMap<Flight, FlightDTO>();
   }
 }
}
