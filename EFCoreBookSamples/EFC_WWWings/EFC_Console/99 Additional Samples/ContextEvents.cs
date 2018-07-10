using System.Linq;
using DA;
using EFC_Console;
using ITVisions;

static internal class ContextEvents
{
 [NotYetInTheBook]
 public static void EreignisFolge()
 {
  var ctx1 = new WWWingsContext();
  CUI.Print("1. Kontext 1. Abfrage");
  ctx1.FlightSet.FirstOrDefault();
  CUI.Print("1. Kontext 2. Abfrage");
  ctx1.FlightSet.FirstOrDefault();
  var ctx2 = new WWWingsContext();
  CUI.Print("2. Kontext 1. Abfrage");
  ctx2.FlightSet.FirstOrDefault();
  CUI.Print("2. Kontext 2. Abfrage");
  ctx2.FlightSet.FirstOrDefault();
  var ctx3 = new WWWingsContext();
  CUI.Print("3. Kontext 1. Abfrage");
  ctx3.FlightSet.FirstOrDefault();
  CUI.Print("3. Kontext 2. Abfrage");
  ctx3.FlightSet.FirstOrDefault();
 }
}