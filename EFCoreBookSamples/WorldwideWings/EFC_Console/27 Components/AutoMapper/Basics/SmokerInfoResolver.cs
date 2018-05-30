using AutoMapper;
using EFC_Console.AutoMapper;
using BO;
using EFC_Console.ViewModels;

namespace EFC_Console.AutoMapper
{
 /// <summary>
 /// Value Resolver for Automapper, converts true/false to
 /// string property "SmokerInfo"
 /// </summary>
 public class SmokerInfoResolver : IValueResolver<Flight, FlightView, string>
 {
  public string Resolve(Flight source, FlightView destination, string member, ResolutionContext context)
  {
   if (source.NonSmokingFlight.GetValueOrDefault()) destination.SmokerInfo = "This is a non-smoking flight!";
   else destination.SmokerInfo = "Smoking is allowed.";
   return destination.SmokerInfo;
  }
 }
}

//public class SmokerInfoResolver : ITypeConverter<bool, string>
//{

// public string Convert(ResolutionContext context)
// {
//  return ((bool)context.SourceValue) ? "Dies ist ein Nicht-Raucher-Flight!" : "Rauchen ist erlaubt.";
// }
//}

//public class DateFormatter : IValueFormatter
//{
// public string FormatValue(ResolutionContext context)
// {
//  return ((DateTime)context.SourceValue).ToShortDateString();
// }
//}