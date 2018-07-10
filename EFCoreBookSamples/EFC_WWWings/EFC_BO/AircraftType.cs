
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BO
{
 /// <summary>
 /// AircraftType has a dependent object  AircraftTypeDetail (1:1)
 /// AircraftTypeDetail uses the same primary key as AircraftType
 /// </summary>
 public class AircraftType
 {
  [Key]
  public byte TypeID { get; set; }
  public string Manufacturer { get; set; }
  public string Name { get; set; }
  // Navigation Property 1:N
  public virtual List<Flight> FlightSet { get; set; }
  // Navigation Property 1:1, unidirectional, no FK Property
  public virtual AircraftTypeDetail Detail { get; set; }

 }
}
