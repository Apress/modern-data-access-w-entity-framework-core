using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO
{
 public class Airline
 {
  [StringLength(3)]
  [Key] // PK
  public string Code { get; set; }
  [StringLength(100)]
  public string Name { get; set; }

  public List<Flight> FlightSet { get; set; }

  public override string ToString()
  {
   return "Airline " + Name + " (" + Code + ")";
  }
 }
}