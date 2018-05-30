using System.ComponentModel.DataAnnotations;

namespace BO
{
 /// <summary>
 /// AircraftTypeDetail is a dependent object (1:1) of AircraftType
 /// AircraftTypeDetail uses the same primary key as AircraftType
 /// </summary>
 public class AircraftTypeDetail
 {
  [Key]
  public byte AircraftTypeID { get; set; }
  public byte? TurbineCount { get; set; }
  public float? Length { get; set; }
  public short? Tare { get; set; }
  public string Memo { get; set; }

  public AircraftType AircraftType { get; set; }
 }
}
