using System;
using System.Linq;
using DA;
using ITVisions.EFCore;

namespace EFC_Console
{
 static internal class FieldMapping
 {

  [NotYetInTheBook]
  internal static void UsePrivateField()
  {
   Console.WriteLine(nameof(UsePrivateField));
   using (WWWingsContext ctx = new WWWingsContext())
   {
    ctx.Log();
    var m = ctx.PilotSet.Where(x => x.PassportNumber == null).FirstOrDefault();
    Console.WriteLine("Pilot: " + m.ToString());
    m.SetPassportNumber("WW123");
    var anz = ctx.SaveChanges();
    Console.WriteLine("Saved changes: " + anz);
    var m2 = ctx.PilotSet.Find(m.PersonID);
    Console.WriteLine("PassportNumber: " + m2.PassportNumber);
   }
  }
 }
}