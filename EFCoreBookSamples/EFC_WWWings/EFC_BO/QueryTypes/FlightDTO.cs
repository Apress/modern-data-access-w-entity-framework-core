using System;

namespace BO
{
public class FlightDTO
 {
  public int FlightNo { get; set; }
  public string Departure { get; set; }
  public string Destination { get; set; }
  public DateTime Date { get; set; }

  public override string ToString()
  {
   return $"Flight {this.FlightNo}: {this.Departure}->{this.Destination} at {this.Date}";
  }
 }
}
