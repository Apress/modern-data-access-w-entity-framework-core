using System;
using System.Linq;

namespace EFC_PowerTools
{
 class Program
 {
  static void Main(string[] args)
  {

   using (var ctx = new Wwwingsv2_ENContext())
   {
    var flightSet = ctx.Flight.Take(10).ToList();
    foreach (var f in flightSet)
    {
     Console.WriteLine(f.FlightNo);
    }
   }

   Console.ReadLine();

   using (var ctx = new Wwwingsv2_ENContext())
   {
    //var path = System.IO.Path.GetTempFileName() + ".dgml";
    //System.IO.File.WriteAllText(path, ctx.AsDgml(), System.Text.Encoding.UTF8);
    //Console.WriteLine("file saved:" + path);
   }




  }
 }
}
