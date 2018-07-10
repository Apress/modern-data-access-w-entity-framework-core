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
  [Key] // must have a PK only in EFC 1.0, 1.1 and 2.0
  public string Departure { get; set; }
  public int FlightCount { get; set; }
 }


 [Table("V_DepartureStatistics")]
 public class DepartureStatisticsView
 {
  public string Departure { get; set; }
  public int FlightCount { get; set; }
 }
}
