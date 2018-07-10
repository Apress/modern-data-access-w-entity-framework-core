using AutoMapper;
using BO;

namespace EFC_Console.AutoMapper
{
 /// <summary>
 /// Converts a Pilot to a string
 /// </summary>
 public class PilotStringConverter : ITypeConverter<Pilot, string>
 {
  public string Convert(Pilot pilot, string s, ResolutionContext context)
  {
   if (pilot == null) return "(Not assigned)";
   return "Pilot # " + pilot.PersonID;
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