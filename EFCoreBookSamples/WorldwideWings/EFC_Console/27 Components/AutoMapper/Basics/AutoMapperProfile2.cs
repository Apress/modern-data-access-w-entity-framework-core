using BO;
using System;
using System.Linq;
using AutoMapper;
using EFC_Console.ViewModels;

namespace EFC_Console.AutoMapper
{

 [EFCBook("3.2")]
 public class AutoMapperProfile2 : Profile
 {

  /// <summary>
  /// Complex profile class for AutoMapper
  /// </summary>
  public AutoMapperProfile2()
  {


   #region Mappings for class Flight

   CreateMap<Flight, FlightView>()

    // 1. Set Memo to static value
    .ForMember(z => z.Memo,
     q => q.UseValue("Loaded from Database: " + DateTime.Now))

    // 2. Mapping for a bool property
    .ForMember(z => z.BookedUp, q => q.MapFrom(f => f.FreeSeats <= 0))

    // 3. Mapping with calculation
    .ForMember(z => z.FlightUtilization,
     q => q.MapFrom(f => (int) Math.Abs(((decimal) f.FreeSeats / (decimal) f.Seats) * 100)))

    // 4. Mapping to a method result
    .ForMember(z => z.PilotInfo, m => m.MapFrom(
     q => q.Pilot.ToString()))

    // 5. Mapping to a method result with object construction
    .ForMember(z => z.Pilot,
     m => m.MapFrom(
      q => new Pilot { PersonID = q.Pilot.PersonID, Surname = q.Pilot.FullName, Birthday = q.Pilot.Birthday.GetValueOrDefault() }))

    // 6. Mapping with a value resolver
    .ForMember(z => z.SmokerInfo,
     m => m.ResolveUsing<SmokerInfoResolver>())

    // 7. Mapping if source value is null
    .ForMember(z => z.Destination, q => q.NullSubstitute("unknown"))

    // 8. No Mapping for existing values
    .ForMember(z => z.Timestamp, q => q.UseDestinationValue())

    // 9. Conditional Mapping
    .ForMember(z => z.Seats, x => x.Condition(q => q.FreeSeats < 250))

    // 10. Map n:m to zu 1:n (for Flight->Booking->Passenger) 
    .ForMember(dto => dto.Passengers, opt => opt.MapFrom(x => x.BookingSet.Select(y => y.Passenger).ToList()))

    // 11. Include reverse Mapping
    .ReverseMap();

   #endregion

   #region Other class mappings
   CreateMap<Pilot, PilotView>();
   CreateMap<Pilot, string>().ConvertUsing<PilotStringConverter>();
   // Map n:m to zu 1:n (for Passenger->Booking->Flight)  
   CreateMap<Passenger, PassengerView>()
    .ForMember(z => z.FlightViewSet, m => m.MapFrom(q => q.BookingSet.Select(y => y.Flight)));

   #endregion

   #region Typkonvertierungen

   CreateMap<byte, long>().ConvertUsing(Convert.ToInt64);
   CreateMap<byte, long>().ConvertUsing(ConvertByteToLong);
   CreateMap<DateTime, Int32>().ConvertUsing(ConvertDateTimeToInt);

   #endregion
  }


  /// <summary>
  /// Converts bytes to long with special case 0
  /// </summary>
  /// <param name="b">Byte value</param>
  /// <returns></returns>
  public static long ConvertByteToLong(byte b)
  {
   if (b == 0) return -1;
   else return (long) b;
  }

  /// <summary>
  /// Converts bytes to long with special case 0
  /// </summary>
  /// <param name="d">DateTime value</param>
  /// <returns></returns>
  public static Int32 ConvertDateTimeToInt(DateTime d)
  {
   return d.Year;
  }
 }
}

