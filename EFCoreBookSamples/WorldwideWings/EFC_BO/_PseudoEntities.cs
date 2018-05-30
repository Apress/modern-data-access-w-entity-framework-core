using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO
{

 public class DepartureGrouping
 {
  [Key] // must have a PK
  public string Departure { get; set; }
  public int FlightCount { get; set; }
 }


 [Table("V_DepartureStatistics")]
 public class DepartureStatistics
 {
  [Key] // must have a PK
  public string Departure { get; set; }
  public int FlightCount { get; set; }
 }
}
